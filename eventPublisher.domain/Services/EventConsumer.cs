using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        private int _httpAttempt;

        public EventConsumer(IRepository repository)
        {
            _repository = repository;
        }

        public void ReceiveEvents()
        {
            IEnumerable<string> topics = _repository.GetTopics();
            var factory = new ConnectionFactory() { HostName = "localhost", Port = Protocols.DefaultProtocol.DefaultPort, DispatchConsumersAsync = true };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                Console.WriteLine(" Listening for Topic {0}", topics.First());
                channel.QueueDeclare(queue: topics.First(), durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.Received += async (model, @event) =>
                {
                    // get message body
                    var body = @event.Body;
                    var message = Encoding.UTF8.GetString(body);

                    // get event
                    IDictionary<string, object> headers = @event.BasicProperties.Headers;
                    int eventId = headers.ContainsKey("eventId") ? Convert.ToInt32(headers["eventId"]) : 0; // todo: fail?
                    Console.WriteLine(" Received {0} for eventId {1}", message, eventId);

                    // make callback(s)
                    IEnumerable<Subscription> subscriptions = _repository.GetSubscriptions(eventId);
                    foreach (var subscription in subscriptions)
                    {
                        await HandleSubscriptionAsync(subscription, message).ConfigureAwait(false);
                    }
                    channel.BasicAck(deliveryTag: @event.DeliveryTag, multiple: false);
                };

                while (true)
                {
                    Thread.Sleep(5000);
                    channel.BasicConsume(queue: topics.First(), autoAck: false, consumer: consumer);
                }
            }
        }

        private async Task HandleSubscriptionAsync(Subscription subscription, string message)
        {
            _httpAttempt = 0;
            while (_httpAttempt < 5)
            {
                HttpResponseMessage response = await MakeHttpPostAsync(subscription, message).ConfigureAwait(false);
                if (response.IsSuccessStatusCode) return; // todo: log
                if ((int)response.StatusCode >= 300 && (int)response.StatusCode < 500) return; // todo: log
            }
        }

        private async Task<HttpResponseMessage> MakeHttpPostAsync(Subscription subscription, string message)
        {
            _httpAttempt++;
            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(message, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(subscription.CallbackUrl, content).ConfigureAwait(false);
                Console.WriteLine(" HTTP Response from {0}: {1}", subscription.CallbackUrl, (int)response.StatusCode);

                return response;
            }
        }
    }
}