using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eventPublisher.domain.contracts;
using eventPublisher.domain.dataTransferObjects;
using eventPublisher.domain.utilities;

namespace eventPublisher.domain.services
{
    public class AdminService: IManageConfigurations
    {
        private IRepository _repository;

        public AdminService(IRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException("repository");
        }

        public async Task<Either<HttpStatusCodeErrorResponse, IEnumerable<ConfigurationDto>>> GetConfigurationsAsync()
        {
            return await ExceptionHandler.HandleExceptionAsync(async () =>
            {
                return await _repository.GetConfigurationsAync();
            }).ConfigureAwait(false);
        }
    }
}