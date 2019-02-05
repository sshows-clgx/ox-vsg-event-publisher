using eventPublisher.domain.dataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace eventPublisher.web.Controllers
{
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