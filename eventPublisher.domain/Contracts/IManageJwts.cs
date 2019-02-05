using System.Threading.Tasks;
using eventPublisher.domain.dataTransferObjects;

namespace eventPublisher.domain.contracts
{
    public interface IManageJwts
    {
        Task<JwtValidationResultDto> ValidateJwtAsync(string jwt);
    }
}