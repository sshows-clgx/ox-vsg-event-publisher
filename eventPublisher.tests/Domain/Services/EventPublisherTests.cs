using System;
using eventPublisher.domain.contracts;
using eventPublisher.domain.dataTransferObjects;
using eventPublisher.domain.exceptions;
using eventPublisher.domain.models;
using eventPublisher.domain.services;
using Moq;
using Xunit;

namespace eventPublisher.tests.domain.services
{
    [Trait("Category", "Unit")]
    public class EventPublisherTests
    {
        private Mock<IRepository> _repo = new Mock<IRepository>();


        [Fact]
        public void EventPublisher_Ctor_RequiresRepository()
        {
            // arrange 
            ResetMocks();

            //act & assert
            Assert.Throws<ArgumentNullException>(() => new EventPublisher((IRepository)null));
        }

        [Fact]
        public void EventPublisher_PublishEventAsync_ThrowsNotFoundExceptionIfEventIsNotFound()
        {
            // arrange 
            ResetMocks();
            _repo.Setup(x => x.GetApplicationEvent(It.IsAny<long>(), It.IsAny<int>())).Returns((ApplicationEvent)null);
            var target = new EventPublisher(_repo.Object);

            //act & assert
            Assert.ThrowsAsync<NotFoundException>(async () => await target.PublishEventAsync<object>(Guid.NewGuid(), new Identity(), 1, new object()));
        }

        private void ResetMocks()
        {
            _repo.Reset();
        }
    }
}