using System;
using System.Threading.Tasks;
using eventPublisher.domain.contracts;
using eventPublisher.domain.dataTransferObjects;
using eventPublisher.domain.exceptions;
using eventPublisher.domain.models;

namespace eventPublisher.domain.services
{
    public class AuthorizationService : IAuthorizeRequests
    {
        private IManageJwts _jwtManager;
        private IRepository _repository;
        public AuthorizationService(IManageJwts jwtManager, IRepository repository)
        {
            _jwtManager = jwtManager ?? throw new ArgumentNullException("jwtManager");
            _repository = repository ?? throw new ArgumentNullException("repository");
        }

        public async Task<Identity> ValidateRequestAsync(string jwt)
        {
            if (string.IsNullOrWhiteSpace(jwt)) throw new ArgumentNullException("jwt");

            // Validate JWT
            JwtValidationResultDto fncConnectResult = await _jwtManager.ValidateJwtAsync(jwt).ConfigureAwait(false);
            ApplicationDto appInfo = fncConnectResult.JwtData.AppInfo;

            // check if Application is configured in EventPublisher
            Application application = await _repository.GetApplicationAsync(fncConnectResult.JwtData.AppInfo.ImmutableAppID).ConfigureAwait(false);
            if (application == null) throw new NotAuthorizedException(string.Format("Application {0} is not configured.", appInfo.AppName));

            return new Identity()
            {
                Id = appInfo.ImmutableAppID,
                Name = appInfo.AppName
            };
        }
    }
}