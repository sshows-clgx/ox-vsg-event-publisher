using System.Collections.Generic;
using System.Threading.Tasks;
using eventPublisher.domain.dataTransferObjects;
using eventPublisher.domain.utilities;

namespace eventPublisher.domain.contracts
{
    public interface IManageConfigurations
    {
        Task<Either<HttpStatusCodeErrorResponse, IEnumerable<ConfigurationDto>>> GetConfigurationsAsync();
    }
}