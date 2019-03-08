using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eventPublisher.data.entities;
using eventPublisher.domain.contracts;
using eventPublisher.domain.models;
using Microsoft.EntityFrameworkCore;

namespace eventPublisher.data
{
    public class EventPublisherRepository : IRepository
    {
        private IContext _context;
        public EventPublisherRepository(IContext eventPublisherContext)
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

        public ApplicationEvent GetApplicationEvent(long applicationId, int eventId)
        {
            ApplicationEventEntity entity = _context.ApplicationEvents.Include("TopicNav").Include("ApplicationNav").SingleOrDefault(x => x.EventId == eventId && x.ApplicationId == applicationId);
            return entity == null ? null : new ApplicationEvent(entity.ApplicationId, entity.ApplicationNav.Name, entity.EventId, entity.TopicNav.Name);
        }

        public IEnumerable<Subscription> GetSubscriptions(int eventId)
        {
            return _context.Subscriptions.Include("ApplicationEventNav.TopicNav").Include("ApplicationNav")
                .Where(s => s.EventId == eventId)
                .Select(x => new Subscription(x.EventId, x.ApplicationId, x.ApplicationNav.Name, x.ApplicationEventNav.TopicNav.Name, x.CallbackUrl, x.ApplicationEventNav.FailedCommandCallbackUrl))
                .ToList();
        }

        public Subscription GetSubscription(long applicationId, int eventId)
        {
            SubscriptionEntity entity = _context.Subscriptions.Include("ApplicationEventNav.TopicNav").Include("ApplicationNav").SingleOrDefault(x => x.EventId == eventId && x.ApplicationId == applicationId);
            return entity == null ? null : new Subscription(entity.EventId, entity.ApplicationId, entity.ApplicationNav.Name, entity.ApplicationEventNav.TopicNav.Name, entity.CallbackUrl, entity.ApplicationEventNav.FailedCommandCallbackUrl);
        }

        public IEnumerable<string> GetTopics()
        {
            return _context.Topics.Select(t => t.Name);
        }
    }
}