using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using eventPublisher.domain.contracts;
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
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [x] Received {0}", message);
                    };

                while (true)
                {
                    Thread.Sleep(5000);
                    channel.BasicConsume(queue: topics.First(), autoAck: true, consumer: consumer);
                }
            
            }
        }
    }
}