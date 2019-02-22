using System;
using System.Linq;
using System.Threading.Tasks;
using eventPublisher.domain.contracts;
using eventPublisher.domain.dataTransferObjects;
using eventPublisher.domain.exceptions;
using eventPublisher.domain.models;
using eventPublisher.domain.utilities;
using Newtonsoft.Json;

namespace eventPublisher.domain.services
{
    public class EventPublisher : IPublishEvents
    {
        private IRepository _repository;
        private IProduceEvents _producer;

        public EventPublisher(IRepository repository, IProduceEvents producer)
        {
            _repository = repository ?? throw new ArgumentNullException("repository");
            _producer = producer ?? throw new ArgumentNullException("producer");
        }

        public async Task<Either<HttpStatusCodeErrorResponse, Guid>> PublishEventAsync<T>(Guid correlationId, Identity identity, int eventId, T data)
        {
            return await ExceptionHandler.HandleExceptionAsync(async () =>
            {
                ApplicationEvent applicationEvent = _repository.GetApplicationEvent(identity.Id, eventId);
                if (applicationEvent == null) throw new NotFoundException("Application Event was not found.");

                _producer.SendEvent(applicationEvent.TopicName, eventId, JsonConvert.SerializeObject(data));
                
                return correlationId;
            }).ConfigureAwait(false);
        }
    }
}