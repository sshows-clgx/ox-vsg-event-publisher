using System.Threading.Tasks;
using eventPublisher.domain.models;

namespace eventPublisher.domain.contracts
{
    public interface IAuthorizeRequests
    {
        Task<Identity> ValidateRequestAsync(string jwt);
    }
}