using System.Threading.Tasks;
using eventPublisher.domain.contracts;
using Microsoft.AspNetCore.Mvc;

namespace eventPublisher.web.Controllers
{
    // [TypeFilter(typeof(JwtFilter))]
    public class ConfigurationsController : BaseController
    {
        private IManageConfigurations _configurationsManager;
        public ConfigurationsController(IManageClaims claims, IManageConfigurations configurationsManager) : base(claims)
        {
            _configurationsManager = configurationsManager;
        }

        [HttpGet]
        [Route("/api/configurations")]
        public async Task<ObjectResult> GetConfigurationsAsync()
        {
            var result = await _configurationsManager.GetConfigurationsAsync();
            return result.Match(err => err.Content(this), r => new CreatedResult("", r));
        }
    }
}