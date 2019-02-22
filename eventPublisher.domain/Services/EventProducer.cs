using System;
using System.Text;
using eventPublisher.domain.contracts;
using RabbitMQ.Client;

namespace eventPublisher.domain.services
{
    public class EventProducer : IProduceEvents
    {
        public EventProducer()
        {
        }

        public void SendEvent(string topic, string data)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: topic, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var body = Encoding.UTF8.GetBytes(data);
                channel.BasicPublish(exchange: "", routingKey: topic, basicProperties: null, body: body);
                Console.WriteLine(" [x] Sent {0}", body);
            }
        }
    }
}