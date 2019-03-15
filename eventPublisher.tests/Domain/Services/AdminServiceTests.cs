using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eventPublisher.domain.contracts;
using eventPublisher.domain.dataTransferObjects;
using eventPublisher.domain.services;
using Moq;
using Xunit;

namespace eventPublisher.tests.domain.services
{
    [Trait("Category", "Unit")]
    public class AdminServiceTests
    {
        private Mock<IRepository> _repo= new Mock<IRepository>();

        [Fact]
        public void AdminService_ctor_RequiresRepository()
        {
            // act & asert
            Assert.Throws<ArgumentNullException>(() => new AdminService((IRepository)null));
        }

        [Fact]
        public async Task AdminService_GetConfigurationsAsync_CallsRepositoryGetConfigurationsAsync()
        {
            // Arrange
            ResetMocks();
            _repo.Setup(x => x.GetConfigurationsAync()).ReturnsAsync(new List<ConfigurationDto>());
            var target = new AdminService(_repo.Object);

            // Act
            var result = await target.GetConfigurationsAsync();
            
            //Assert
            _repo.Verify(x => x.GetConfigurationsAync(), Times.Once);
        }

        private void ResetMocks()
        {
            _repo.Reset();
        }
    }
}