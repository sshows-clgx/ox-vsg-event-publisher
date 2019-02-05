using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using eventPublisher.domain.models;
using eventPublisher.domain.services;
using Xunit;

namespace eventPublisher.tests.domain.services
{
    [Trait("Category", "Unit")]
    public class ClaimsManagerTests
    {
        [Fact]
        public void ClaimsService_CreateClaims_CreatesClaims()
        {
            // arrange 
            var identity = new Identity();
            identity.Id = 1;
            identity.Name = "test";
            ClaimsManager target = new ClaimsManager();

            // act
            ClaimsPrincipal result = target.CreateClaims(identity);

            // assert
            var claims = result.Claims;
            var claimsId = claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid);
            var claimsName = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

            Assert.Equal(identity.Id.ToString(), claimsId.Value);
            Assert.Equal(identity.Name, claimsName.Value);
        }

        [Fact]
        public void ClaimsService_GetIdentityFromClaims_ReturnsPopulatedIdentity()
        {
            // arrange 
            var clientId = 1;
            var name = "test name";
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Sid, clientId.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, name));
            var id = new ClaimsIdentity(claims);
            var user = new ClaimsPrincipal(id);
            ClaimsManager target = new ClaimsManager();

            // act
            Identity result = target.GetIdentityFromClaims(user);

            // assert
            Assert.Equal(clientId, result.Id);
            Assert.Equal(name, result.Name);
        }
    }
}