using System;
using System.Threading.Tasks;
using eventPublisher.domain.contracts;
using eventPublisher.domain.dataTransferObjects;
using eventPublisher.web.filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace eventPublisher.web.Controllers
{
    [TypeFilter(typeof(JwtFilter))]
    public class EventsController : BaseController
    {
        private IPublishEvents _publisher;
        public EventsController(IPublishEvents publisher, IManageClaims claims) : base(claims)
        {
            _publisher = publisher;
        }

        [HttpPost]
        [Route("/api/events/{eventId}")]
        [SwaggerResponse(StatusCodes.Status201Created, typeof(Guid), "Successfully published event. CorrelationId is returned for tracking.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, typeof(string), "Event not found for Application")]
        public async Task<ObjectResult> PublishEventAsync(int eventId, [FromBody] object data)
        {
            Guid correlationId = Guid.NewGuid();
            var result = await _publisher.PublishEventAsync<object>(correlationId, GetIdentityFromClaims(), eventId, data);
            return result.Match(err => err.Content(this), r => new CreatedResult("", r));
        }
    }
}