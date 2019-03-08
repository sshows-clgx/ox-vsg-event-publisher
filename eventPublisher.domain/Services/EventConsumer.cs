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
        private ConnectionFactory _factory;
        private string _exchangeName;

        public EventConsumer(IRepository repository)
        {
            _repository = repository;
            _factory = new ConnectionFactory() { HostName = "localhost", Port = Protocols.DefaultProtocol.DefaultPort, DispatchConsumersAsync = true };
            _exchangeName = "Consumer.PortsUser";
        }

        public void ReceiveEvents()
        {
            IEnumerable<string> topics = _repository.GetTopics();
            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    // channel.QueueDeclare(queue: _queue, durable: false, exclusive: false, autoDelete: false, arguments: null);
                    channel.ExchangeDeclare(exchange: _exchangeName, type: "fanout");

                    var queueName = channel.QueueDeclare().QueueName;
                    channel.QueueBind(queue: queueName,
                              exchange: _exchangeName,
                              routingKey: "");

                    var consumer = new AsyncEventingBasicConsumer(channel);
                    consumer.Received += async (model, @event) =>
                    {
                        // get message body
                        var body = Encoding.UTF8.GetString(@event.Body);

                        // get event info
                        IDictionary<string, object> headers = @event.BasicProperties.Headers;
                        long applicationId = headers.ContainsKey("applicationId") ? Convert.ToInt64(headers["applicationId"]) : 0; // todo: fail?
                        int eventId = headers.ContainsKey("eventId") ? Convert.ToInt32(headers["eventId"]) : 0; // todo: fail?
                        Console.WriteLine(" Received {0} for eventId {1}", body, eventId);

                        // make callback
                        Subscription subscription = _repository.GetSubscription(applicationId, eventId);
                        await HandleSubscriptionAsync(subscription, body).ConfigureAwait(false);

                        channel.BasicAck(deliveryTag: @event.DeliveryTag, multiple: false);
                    };

                    while (true)
                    {
                        channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
                    }
                }
            }
        }

        private async Task HandleSubscriptionAsync(Subscription subscription, string message)
        {
            _httpAttempt = 0;
            var isComplete = false;
            while (_httpAttempt < 5 || isComplete)
            {
                HttpResponseMessage response = await MakeHttpPostAsync(subscription, message).ConfigureAwait(false);
                if (response.IsSuccessStatusCode) isComplete = true; ; // todo: log
                if ((int)response.StatusCode >= 300 && (int)response.StatusCode < 500) isComplete = true; // todo: log
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