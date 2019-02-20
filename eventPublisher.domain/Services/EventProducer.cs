using System;
using Confluent.Kafka;
using eventPublisher.domain.contracts;

namespace eventPublisher.domain.services
{
    public class EventProducer: IProduceEvents
    {
        private ProducerConfig _config;

        public EventProducer() {
            _config = new ProducerConfig { BootstrapServers = "localhost:9092" };
        }
        public void SendEvent(string topic, string data)
        {
            Action<DeliveryReport<Null, string>> handler = r => 
            Console.WriteLine(!r.Error.IsError
                ? $"Delivered message to {r.TopicPartitionOffset}"
                : $"Delivery Error: {r.Error.Reason}");

            using (var p = new ProducerBuilder<Null, string>(_config).Build())
            {
                p.BeginProduce(topic, new Message<Null, string> { Value = data }, handler);

                // wait for up to 10 seconds for any inflight messages to be delivered.
                p.Flush(TimeSpan.FromSeconds(10));
            }

        }
    }
}