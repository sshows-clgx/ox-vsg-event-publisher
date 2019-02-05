using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using eventPublisher.domain.contracts;
using eventPublisher.domain.exceptions;
using eventPublisher.domain.models;

namespace eventPublisher.domain.services
{
    public class ClaimsManager : IManageClaims
    {
        public ClaimsPrincipal CreateClaims(Identity identity)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Sid, identity.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, identity.Name));

            var id = new ClaimsIdentity(claims);
            return new ClaimsPrincipal(id);
        }

        public Identity GetIdentityFromClaims(ClaimsPrincipal user)
        {
            if (user == null || !user.Claims.Any()) throw new NotAuthorizedException("Access denied. User is required.");
            var claims = user.Claims;

            // get Id
            var claimsId = claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid);
            Int64.TryParse(claimsId.Value, out long id);

            // get Name
            var claimsName = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

            return new Identity()
            {
                Id = id,
                Name = claimsName?.Value
            };
        }
    }
}