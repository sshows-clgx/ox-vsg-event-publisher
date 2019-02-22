using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using eventPublisher.domain.contracts;
using eventPublisher.domain.models;
using eventPublisher.domain.utilities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace eventPublisher.domain.services
{
    public class EventConsumer : IConsumeEvents
    {
        private IRepository _repository;

        public EventConsumer(IRepository repository)
        {
            _repository = repository;
        }

        public void ReceiveEvents()
        {
            IEnumerable<string> topics = _repository.GetTopics();
            var factory = new ConnectionFactory() { HostName = "localhost", Port = Protocols.DefaultProtocol.DefaultPort, };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                Console.WriteLine(" Topic {0}", topics.First());
                channel.QueueDeclare(queue: topics.First(), durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    // get message body
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    // get event
                    IDictionary<string, object> headers = ea.BasicProperties.Headers;
                    int eventId = headers.ContainsKey("eventId") ? Convert.ToInt32(headers["eventId"]) : 0; // todo: fail?
                    Console.WriteLine(" Received {0} for eventId {1}", message, eventId);

                    // make callback(s)
                    IEnumerable<Subscription> subscriptions = _repository.GetSubscriptions(eventId);

                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                };

                while (true)
                {
                    Thread.Sleep(5000);
                    channel.BasicConsume(queue: topics.First(), autoAck: false, consumer: consumer);
                }

            }
        }
    }
}