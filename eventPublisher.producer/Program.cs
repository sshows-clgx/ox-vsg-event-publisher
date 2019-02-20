using System;
using Confluent.Kafka;

namespace eventPublisher.producer
{
    class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                var topic = args[0];
                var data = args[1];
                Console.WriteLine($"Topic: {topic} with data: {data}");

                var conf = new ProducerConfig { BootstrapServers = "localhost:9092" };

                Action<DeliveryReportResult<Null, string>> handler = r =>
                    Console.WriteLine(!r.Error.IsError
                        ? $"Delivered message to {r.TopicPartitionOffset}"
                        : $"Delivery Error: {r.Error.Reason}");

                using (var p = new Producer<Null, string>(conf))
                {
                    p.BeginProduce(topic, new Message<Null, string> { Value = data }, handler);

                    // wait for up to 10 seconds for any inflight messages to be delivered.
                    p.Flush(TimeSpan.FromSeconds(10));
                }
            }
        }
    }
}
