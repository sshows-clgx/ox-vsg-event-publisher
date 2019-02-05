using System;
using System.Net.Http;
using System.Threading.Tasks;
using eventPublisher.domain.exceptions;
using eventPublisher.domain.services;
using Newtonsoft.Json;
using Xunit;

namespace eventPublisher.tests.domain.services
{
    [Trait("Category", "Integration")]
    public class JwtManagerTests
    {
        // [Fact]
        // public async Task FncConnectService_ValidateJwt_ValidatesJwt()
        // {
        //     // arrange
        //     string jwt;
        //     var target = new JwtManager();
        //     using (var client = new HttpClient())
        //     {
        //         var generateJwtDto = new GenerateJwtDto()
        //         {
        //             AppKey = settings["FNCCONNECTAPPKEY"],
        //             AppSecret = settings["FNCCONNECTAPPSECRET"],
        //             ExpireTimeInMinutes = 1,
        //             AdditionalData = new List<KeyValuePair<string, string>>() { }
        //         };

        //         var url = "https://uat.fncconnect.com/externalapi/jwt";
        //         var stringContent = StringHelpers.CreateStringContent(generateJwtDto);
        //         var response = await client.PostAsync(url, stringContent).ConfigureAwait(false);
        //         var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        //         jwt = JsonConvert.DeserializeObject<string>(responseContent);
        //     }

        //     Assert.NotEmpty(jwt);

        //     // act
        //     var result = await target.ValidateJwtAsync(jwt);

        //     // assert
        //     Assert.True(result.Success);
        // }

        [Fact]
        public async Task FncConnectService_ValidateJwt_ThrowsInvlidJwtExceptionWithBadJwt()
        {
            // arrange
            var target = new JwtManager();

            var invalidJwt = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOiIyMDE4LTA0LTE1VDE4OjI0OjAyLjQwNDg4NzRaIiwiYXBwa2V5IjoiRU1IT09" +
                        "DTVRSNzhER0NKODhBVlU2RDFXV1dVUENJUDVBNU9UUldTUCIsImJsYWhLIjoiYmxhaFYifQ.0GNV8R6jVxDq3R9yfzseYWM5_Cc5Uy5AvyHQZMYZOuY";

            // act & Assert
            Exception ex = await Assert.ThrowsAsync<InvalidJwtException>(async () => await target.ValidateJwtAsync(invalidJwt));
            Assert.Equal("JWT is invalid.", ex.Message);
        }
    }
}