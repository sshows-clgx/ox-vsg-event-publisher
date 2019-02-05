using eventPublisher.domain.dataTransferObjects;
using eventPublisher.web.filters;
using Microsoft.AspNetCore.Mvc;

namespace eventPublisher.web.Controllers
{
    [TypeFilter(typeof(JwtFilter))]
    [Route("api/[controller]")]
    public class EventsController : Controller
    {
        // POST api/events
        [HttpPost]
        public void Post([FromBody] EventDto eventDto)
        {
        }
    }
}