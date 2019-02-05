using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using eventPublisher.domain.contracts;
using eventPublisher.domain.dataTransferObjects;
using eventPublisher.domain.exceptions;
using Newtonsoft.Json;

namespace eventPublisher.domain.services
{
    public class JwtManager : IManageJwts
    {
        public async Task<JwtValidationResultDto> ValidateJwtAsync(string jwt)
        {
            using (var client = new HttpClient())
            {
                // POST to FncConnect
                var url = "https://uat.fncconnect.com/externalapi/jwt/validate/true";
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + jwt);
                var stringContent = new StringContent(JsonConvert.SerializeObject(jwt), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, stringContent).ConfigureAwait(false);
                
                if (!response.IsSuccessStatusCode) throw new InvalidJwtException("JWT is invalid.");
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var validationResultDto = JsonConvert.DeserializeObject<JwtValidationResultDto>(responseContent);
                return validationResultDto;
            }
        }
    }
}