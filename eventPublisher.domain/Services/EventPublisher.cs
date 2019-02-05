using System;
using System.Threading.Tasks;
using eventPublisher.domain.contracts;
using eventPublisher.domain.dataTransferObjects;
using eventPublisher.domain.exceptions;
using eventPublisher.domain.models;
using eventPublisher.domain.utilities;

namespace eventPublisher.domain.services
{
    public class EventPublisher : IPublishEvents
    {
        private IRepository _repository;

        public EventPublisher(IRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException("repository");
        }

        public async Task<Either<HttpStatusCodeErrorResponse, Guid>> PublishEventAsync<T>(Guid correlationId, Identity identity, int eventId, T data)
        {
            return await ExceptionHandler.HandleExceptionAsync(async () =>
            {
                ApplicationEvent applicationEvent = _repository.GetApplicationEvent(identity.Id, eventId);
                if (applicationEvent == null) throw new NotFoundException("Application Event was not found.");
                
                return correlationId;
            }).ConfigureAwait(false);
        }
    }
}