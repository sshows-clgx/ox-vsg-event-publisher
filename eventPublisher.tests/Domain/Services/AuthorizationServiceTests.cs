using System;
using System.Threading.Tasks;
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
    public class AuthorizationServiceTests
    {
        private Mock<IManageJwts> _jwtManager = new Mock<IManageJwts>();
        private Mock<IRepository> _repo = new Mock<IRepository>();

        [Fact]
        public void AuthorizationService_Ctr_RequiresIAuthenticateJwtRequests()
        {
            Assert.Throws<ArgumentNullException>(() => new AuthorizationService(null, null));
        }

        [Fact]
        public void AuthorizationService_Ctr_RequiresIRepository()
        {
            Assert.Throws<ArgumentNullException>(() => new AuthorizationService(_jwtManager.Object, null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task AuthorizationService_ValidateRequestAsync_ThrowsArgumentNullExceptionWhenMissingJwt(string jwt)
        {
            // arrange
            ResetMocks();
            var target = new AuthorizationService(_jwtManager.Object, _repo.Object);

            // act
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await target.ValidateRequestAsync(jwt));
        }

        [Fact]
        public async Task AuthorizationService_ValidateRequestAsync_ThrowsNotAuthorizedIfApplicationIsNotFound()
        {
            // arrange
            ResetMocks();
            var jwt = "jwt";
            var fncConnectResult = new JwtValidationResultDto()
            {
                JwtData = new FullJwtDataDto()
                {
                    AppInfo = new ApplicationDto()
                    {
                        ImmutableAppID = 1
                    }
                }
            };

            _jwtManager.Setup(x => x.ValidateJwtAsync(It.IsAny<string>())).ReturnsAsync(fncConnectResult);
            _repo.Setup(x => x.GetApplicationAsync(It.IsAny<long>())).ReturnsAsync((Application)null);

            var target = new AuthorizationService(_jwtManager.Object, _repo.Object);

            // act
            await Assert.ThrowsAsync<NotAuthorizedException>(async () => await target.ValidateRequestAsync(jwt));

        }

        [Fact]
        public async Task AuthorizationService_ValidateRequestAsync_ReturnsIdentityOfApplication()
        {
            // arrange
            ResetMocks();
            var jwt = "jwt";
            var applicationId = 1;
            var fncConnectResult = new JwtValidationResultDto()
            {
                JwtData = new FullJwtDataDto()
                {
                    AppInfo = new ApplicationDto()
                    {
                        ImmutableAppID = applicationId
                    }
                }
            };
            var application = new Application(applicationId, "test");
            _jwtManager.Setup(x => x.ValidateJwtAsync(It.IsAny<string>())).ReturnsAsync(fncConnectResult);
            _repo.Setup(x => x.GetApplicationAsync(It.IsAny<long>())).ReturnsAsync(application);

            var target = new AuthorizationService(_jwtManager.Object, _repo.Object);

            // act
            var result = await target.ValidateRequestAsync(jwt);

            // assert
            Assert.NotNull(result);
            Assert.Equal(applicationId, result.Id);
        }

        private void ResetMocks()
        {
            _jwtManager.Reset();
            _repo.Reset();
        }
    }
}