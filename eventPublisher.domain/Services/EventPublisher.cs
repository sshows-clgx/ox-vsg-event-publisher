using System;
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
                
                var output = "dotnet ../eventPublisher.producer/bin/debug/netcoreapp2.0/eventPublisher.producer.dll test test2".Bash();


                return correlationId;
            }).ConfigureAwait(false);
        }
    }
}