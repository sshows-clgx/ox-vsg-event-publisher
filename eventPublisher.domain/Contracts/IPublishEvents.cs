using System;
using System.Threading.Tasks;
using eventPublisher.domain.dataTransferObjects;
using eventPublisher.domain.models;
using eventPublisher.domain.utilities;

namespace eventPublisher.domain.contracts
{
    public interface IPublishEvents
    {
        Task<Either<HttpStatusCodeErrorResponse, Guid>> PublishEventAsync<T>(Guid correlationId, Identity identiity, int eventId, T data);
    }
}