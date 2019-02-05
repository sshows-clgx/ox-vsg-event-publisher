using System;
using System.Threading.Tasks;
using eventPublisher.data.entities;
using eventPublisher.domain.contracts;
using eventPublisher.domain.models;

namespace eventPublisher.data {
    public class EventPublisherRepository : IRepository
    {
        private EventPublisherContext _context;
        public EventPublisherRepository(EventPublisherContext eventPublisherContext)
        {
            _context = eventPublisherContext ?? throw new ArgumentNullException("EventPublisherContext");
        }
        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<Application> GetApplicationAsync(long applicationId)
        {
            ApplicationEntity entity = await _context.Applications.FindAsync(applicationId).ConfigureAwait(false);
            return entity == null ? null : new Application(entity.ApplicationId, entity.Name);
        }
    }
}