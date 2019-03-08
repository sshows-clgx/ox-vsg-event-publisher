using System;
using System.Collections.Generic;
using System.Text;
using eventPublisher.domain.contracts;
using eventPublisher.domain.models;
using RabbitMQ.Client;

namespace eventPublisher.domain.services
{
    public class EventProducer : IProduceEvents
    {
        private IRepository _repository;
        private ConnectionFactory _factory;
        public EventProducer(IRepository repository)
        {
            _repository = repository;
            _factory = new ConnectionFactory() { HostName = "localhost" };
        }

        public void SendEvent(ApplicationEvent applicationEvent, string data)
        {
            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    IBasicProperties props = channel.CreateBasicProperties();
                    props.ContentType = "text/plain";
                    props.DeliveryMode = 2;
                    props.Headers = new Dictionary<string, object>();
                    props.Headers.Add("applicationId", applicationEvent.ApplicationId);
                    props.Headers.Add("eventId", applicationEvent.EventId);

                    IEnumerable<Subscription> subscriptions = _repository.GetSubscriptions(applicationEvent.EventId);
                    foreach (var subscription in subscriptions)
                    {
                        string exchangeName = string.Format("{0}.{1}", subscription.ApplicationName, subscription.TopicName);
                        channel.ExchangeDeclare(exchangeName, "fanout");

                        // channel.QueueDeclare(queue: string.Format("{0}.{1}", subscription.ApplicationName, subscription.TopicName), durable: false, exclusive: false, autoDelete: false, arguments: null);
                        channel.BasicPublish(exchange: exchangeName, routingKey: exchangeName, basicProperties: props, body: Encoding.UTF8.GetBytes(data));
                        Console.WriteLine("Publishing for {0}", exchangeName);
                    }
                }
            }
        }
    }
}