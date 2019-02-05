using System.Security.Claims;
using eventPublisher.domain.models;

namespace eventPublisher.domain.contracts
{
    public interface IManageClaims {
        ClaimsPrincipal CreateClaims(Identity identity);
        Identity GetIdentityFromClaims(ClaimsPrincipal user);
    }
}