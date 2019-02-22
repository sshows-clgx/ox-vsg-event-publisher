using System;
using System.Collections.Generic;
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

        public void SendEvent(string topic, int eventId, string data)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: topic, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var body = Encoding.UTF8.GetBytes(data);
                IBasicProperties props = channel.CreateBasicProperties();
                props.ContentType = "text/plain";
                props.DeliveryMode = 2;
                props.Headers = new Dictionary<string, object>();
                props.Headers.Add("eventId", eventId);

                channel.BasicPublish(exchange: "", routingKey: topic, basicProperties: props, body: body);
                Console.WriteLine(" [x] Sent {0}", body);
            }
        }
    }
}