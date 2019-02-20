using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Confluent.Kafka;
using eventPublisher.domain.contracts;
using eventPublisher.domain.utilities;

namespace eventPublisher.domain.services
{
    public class EventConsumer : IConsumeEvents
    {
        private IRepository _repository;
        private ConsumerConfig _config;
        private IEnumerable<string> _topics;

        public EventConsumer(IRepository repository)
        {
            _repository = repository;
            _config = new ConsumerConfig
            {
                GroupId = "test-consumer-group",
                BootstrapServers = "localhost:9092",
                // Note: The AutoOffsetReset property determines the start offset in the event
                // there are not yet any committed offsets for the consumer group for the
                // topic/partitions of interest. By default, offsets are committed
                // automatically, so in this example, consumption will only start from the
                // earliest message in the topic 'my-topic' the first time you run the program.
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _topics = _repository.GetTopics();
        }

        public void ReceiveEvents()
        {
            using (Consumer<string, string> c = new ConsumerBuilder<string, string>(_config).Build())
            {
                if(_topics.Any()) c.Subscribe(_topics);
                CancellationTokenSource cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true; // prevent the process from terminating.
                    cts.Cancel();
                };

                try
                {
                    while (true)
                    {
                        try
                        {
                            var cr = c.Consume(cts.Token);
                            Console.WriteLine($"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occured: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // Ensure the consumer leaves the group cleanly and final offsets are committed.
                    c.Close();
                }


                // c.OnMessage += (_, msg) =>
                //             {

                //                 Console.WriteLine($"Topic: {msg.Topic} Partition: {msg.Partition} Offset: {msg.Offset} {msg.Value}");

                //                 // c.Consume();
                //             };



                // bool consuming = true;
                // // The client will automatically recover from non-fatal errors. You typically
                // // don't need to take any action unless an error is marked as fatal.
                // c.OnError += (_, e) => consuming = !e.IsFatal;

                // while (consuming)
                // {
                //     try
                //     {
                //         var cr = c.Consume();
                //         Console.WriteLine($"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");
                //     }
                //     catch (ConsumeException e)
                //     {
                //         Console.WriteLine($"Error occured: {e.Error.Reason}");
                //     }
                // }

                // Ensure the consumer leaves the group cleanly and final offsets are committed.
                c.Close();
            }
        }
    }
}