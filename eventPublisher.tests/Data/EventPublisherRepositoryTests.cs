using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eventPublisher.data;
using eventPublisher.data.entities;
using eventPublisher.domain.dataTransferObjects;
using eventPublisher.domain.models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Xunit;

namespace eventPublisher.tests.data
{
    [Trait("Category", "Unit")]
    public class EventPublisherRepositoryTests
    {
        [Fact]
        public void EventPublisherRepository_ctor_RequiresContext()
        {
            // act & asert
            Assert.Throws<ArgumentNullException>(() => new EventPublisherRepository((EventPublisherContext)null));
        }

        [Fact]
        public async Task EventPublisherRepository_GetApplicationAsync_ReturnsNullWhenNotFound()
        {
            // arrange
            EventPublisherContext context = GetContext();
            var target = new EventPublisherRepository(context);

            // act
            Application result = await target.GetApplicationAsync(1);

            // assert
            Assert.Null(result);
        }

        [Fact]
        public async Task EventPublisherRepository_GetApplicationAsync_ReturnsApplicationWhenFound()
        {
            // arrange
            EventPublisherContext context = GetContext();
            var applicationId = context.Applications.Add(new ApplicationEntity
            {

            }).Entity.ApplicationId;
            await context.SaveChangesAsync().ConfigureAwait(false);
            var target = new EventPublisherRepository(context);

            // act
            Application result = await target.GetApplicationAsync(applicationId);

            // assert
            Assert.NotNull(result);
        }

        [Fact]
        public void EventPublisherRepository_GetApplicationEvent_ReturnsNullWhenNotFound()
        {
            // arrange
            EventPublisherContext context = GetContext();
            var target = new EventPublisherRepository(context);

            // act
            ApplicationEvent result = target.GetApplicationEvent(1, 1);

            // assert
            Assert.Null(result);
        }

        [Fact]
        public async Task EventPublisherRepository_GetApplicationEvent_ReturnsNullIfEventDoesNotBelongToApplication()
        {
            // arrange
            EventPublisherContext context = GetContext();
            var applicationId = context.Applications.Add(new ApplicationEntity
            {

            }).Entity.ApplicationId;
            var eventId = context.ApplicationEvents.Add(new ApplicationEventEntity
            {
                ApplicationId = applicationId + 1
            }).Entity.EventId;
            await context.SaveChangesAsync().ConfigureAwait(false);
            var target = new EventPublisherRepository(context);

            // act
            ApplicationEvent result = target.GetApplicationEvent(applicationId, eventId);

            // assert
            Assert.Null(result);
        }

        [Fact]
        public async Task EventPublisherRepository_GetApplicationEvent_ReturnsApplicationEventWhenFound()
        {
            // arrange
            EventPublisherContext context = GetContext();
            var applicationId = context.Applications.Add(new ApplicationEntity
            {

            }).Entity.ApplicationId;
            var topicId = context.Topics.Add(new TopicEntity
            {
                Name = "Test",
            }).Entity.TopicId;
            var eventId = context.ApplicationEvents.Add(new ApplicationEventEntity
            {
                ApplicationId = applicationId,
                Name = "Test",
                TopicId = topicId
            }).Entity.EventId;
            await context.SaveChangesAsync().ConfigureAwait(false);
            var target = new EventPublisherRepository(context);

            // act
            ApplicationEvent result = target.GetApplicationEvent(applicationId, eventId);

            // assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task EventPublisherRepository_GetTopics_ReturnsTopicNames()
        {
            // arrange
            EventPublisherContext context = GetContext();
            context.Topics.Add(new TopicEntity
            {
                Name = "Test",
            });
            context.Topics.Add(new TopicEntity
            {
                Name = "Test2",
            });
            context.Topics.Add(new TopicEntity
            {
                Name = "Test3",
            });
            await context.SaveChangesAsync().ConfigureAwait(false);
            var target = new EventPublisherRepository(context);

            // act
            IEnumerable<string> result = target.GetTopics();

            // assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task EventPublisherRepository_GetSubscriptions_ReturnsSubscriptions()
        {
            // arrange
            EventPublisherContext context = GetContext();

            // applications
            var applicationId1 = context.Applications.Add(new ApplicationEntity
            {
                Name = "Application 1"
            }).Entity.ApplicationId;
            var applicationId2 = context.Applications.Add(new ApplicationEntity
            {
                Name = "Application 2"
            }).Entity.ApplicationId;

            var topicId = context.Topics.Add(new TopicEntity
            {
                Name = "Test",
            }).Entity.TopicId;

            // events
            var eventId1 = context.ApplicationEvents.Add(new ApplicationEventEntity
            {
                ApplicationId = applicationId1,
                Name = "Test",
                TopicId = topicId
            }).Entity.EventId;
            var eventId2 = context.ApplicationEvents.Add(new ApplicationEventEntity
            {
                ApplicationId = applicationId1,
                Name = "Test2",
                TopicId = topicId
            }).Entity.EventId;

            // subscriptions
            context.Subscriptions.Add(new SubscriptionEntity
            {
                ApplicationId = applicationId1,
                EventId = eventId1,
                CallbackUrl = "https://google.com"
            });
            context.Subscriptions.Add(new SubscriptionEntity
            {
                ApplicationId = applicationId2,
                EventId = eventId1,
                CallbackUrl = "https://google.com"
            });
            context.Subscriptions.Add(new SubscriptionEntity
            {
                ApplicationId = applicationId2,
                EventId = eventId2,
                CallbackUrl = "https://google.com"
            });

            await context.SaveChangesAsync().ConfigureAwait(false);
            var target = new EventPublisherRepository(context);

            // act
            IEnumerable<Subscription> result = target.GetSubscriptions(eventId1);

            // assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task EventPublisherRepository_GetConfigurationsAync_ReturnsConfigurations()
        {
            // arrange
            EventPublisherContext context = GetContext();

            // applications
            var publisherId = context.Applications.Add(new ApplicationEntity
            {
                Name = "Application 1",
            }).Entity.ApplicationId;
            var subscriberId = context.Applications.Add(new ApplicationEntity
            {
                Name = "Subscriber"
            }).Entity.ApplicationId;

            // topic
            var topicId = context.Topics.Add(new TopicEntity
            {
                Name = "Test",
            }).Entity.TopicId;

            // events
            var eventId1 = context.ApplicationEvents.Add(new ApplicationEventEntity
            {
                ApplicationId = publisherId,
                Name = "Test",
                TopicId = topicId,
                PublisherCallbackUrl = "https://google.com"
            }).Entity.EventId;
            var eventId2 = context.ApplicationEvents.Add(new ApplicationEventEntity
            {
                ApplicationId = publisherId,
                Name = "Test2",
                TopicId = topicId
            }).Entity.EventId;

            // subscriptions
            context.Subscriptions.Add(new SubscriptionEntity
            {
                ApplicationId = publisherId,
                EventId = eventId1,
                CallbackUrl = "https://google.com"
            });
            context.Subscriptions.Add(new SubscriptionEntity
            {
                ApplicationId = subscriberId,
                EventId = eventId1,
                CallbackUrl = "https://google.com"
            });
            context.Subscriptions.Add(new SubscriptionEntity
            {
                ApplicationId = subscriberId,
                EventId = eventId2,
                CallbackUrl = "https://google.com"
            });

            await context.SaveChangesAsync().ConfigureAwait(false);
            var target = new EventPublisherRepository(context);
            var expectedDto1 = new ConfigurationDto
            {
                EventId = eventId1,
                EventName = "Test",
                ApplicationId = publisherId,
                ApplicationName = "Application 1",
                TopicId = topicId,
                TopicName = "Test",
                PublisherCallbackUrl = "https://google.com",
                Subscribers = new List<SubscriptionDto> {
                    new SubscriptionDto {
                        ApplicationName = "Application 1",
                        CallbackUrl = "https://google.com",
                    },
                    new SubscriptionDto {
                        ApplicationName = "Subscriber",
                        CallbackUrl = "https://google.com",
                    }
                }
            };

            // act
            IEnumerable<ConfigurationDto> result = await target.GetConfigurationsAync();

            // assert
            Assert.Equal(2, result.Count());
            var dto = result.Single(x => x.EventId == eventId1);
            Assert.Equal(JsonConvert.SerializeObject(expectedDto1), JsonConvert.SerializeObject(dto));
        }

        private EventPublisherContext GetContext()
        {
            // NOTE: Keys in DB do not automatically reset with new in memory db, see:  https://github.com/aspnet/EntityFrameworkCore/issues/6872
            return new EventPublisherContext(new DbContextOptionsBuilder<EventPublisherContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        }
    }

}