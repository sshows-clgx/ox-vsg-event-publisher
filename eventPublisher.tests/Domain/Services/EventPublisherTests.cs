using System;
using eventPublisher.domain.contracts;
using eventPublisher.domain.dataTransferObjects;
using eventPublisher.domain.exceptions;
using eventPublisher.domain.models;
using eventPublisher.domain.services;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace eventPublisher.tests.domain.services
{
    [Trait("Category", "Unit")]
    public class EventPublisherTests
    {
        private Mock<IRepository> _repo = new Mock<IRepository>();
        private Mock<IProduceEvents> _producer = new Mock<IProduceEvents>();


        [Fact]
        public void EventPublisher_Ctor_RequiresRepository()
        {
            // arrange 
            ResetMocks();

            //act & assert
            Assert.Throws<ArgumentNullException>(() => new EventPublisher((IRepository)null, _producer.Object));
        }


        [Fact]
        public void EventPublisher_Ctor_RequiresProducer()
        {
            // arrange 
            ResetMocks();

            //act & assert
            Assert.Throws<ArgumentNullException>(() => new EventPublisher(_repo.Object, (IProduceEvents)null));
        }

        [Fact]
        public void EventPublisher_PublishEventAsync_ThrowsNotFoundExceptionIfEventIsNotFound()
        {
            // arrange 
            ResetMocks();
            _repo.Setup(x => x.GetApplicationEvent(It.IsAny<long>(), It.IsAny<int>())).Returns((ApplicationEvent)null);
            var target = new EventPublisher(_repo.Object, _producer.Object);

            //act & assert
            Assert.ThrowsAsync<NotFoundException>(async () => await target.PublishEventAsync<object>(Guid.NewGuid(), new Identity(), 1, new object()));
        }

        [Fact]
        public async void EventPublisher_PublishEventAsync_CallsProducerWithTopicAndData()
        {
            // arrange 
            ResetMocks();
            ApplicationEvent applicationEvent = new ApplicationEvent(1, 1, "Topic");
            var data = new object();
            _repo.Setup(x => x.GetApplicationEvent(It.IsAny<long>(), It.IsAny<int>())).Returns(applicationEvent);
            var target = new EventPublisher(_repo.Object, _producer.Object);

            //act
            var result = await target.PublishEventAsync<object>(Guid.NewGuid(), new Identity(), 1, data);

            // assert
            result.Match(
                err => { },
                x =>
                {
                    _producer.Verify(s => s.SendEvent(applicationEvent.TopicName, JsonConvert.SerializeObject(data)), Times.Once);
                }
            );
        }

        private void ResetMocks()
        {
            _repo.Reset();
            _producer.Reset();
        }
    }
}