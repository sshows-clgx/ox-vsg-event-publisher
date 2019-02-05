using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using eventPublisher.domain.contracts;
using eventPublisher.domain.exceptions;
using eventPublisher.domain.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace eventPublisher.web.filters
{
    public class JwtFilter : ActionFilterAttribute
    {
        private IAuthorizeRequests _authorizeService;
        private IManageClaims _claims;

        public JwtFilter(IAuthorizeRequests authorizeService, IManageClaims claims)
        {
            _authorizeService = authorizeService;
            _claims = claims;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            StringValues authorizeHeader;
            var headers = context.HttpContext.Request.Headers;
            if (headers.TryGetValue("Authorization", out authorizeHeader))
            {
                var x = authorizeHeader.FirstOrDefault();
                if (!String.IsNullOrEmpty(x))
                {
                    var headerValue = x.ToString();
                    if (headerValue.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        var jwt = headerValue.Replace("Bearer ", string.Empty);
                        try
                        {
                            Identity identity = _authorizeService.ValidateRequestAsync(jwt).Result;
                            ClaimsPrincipal claimsPrincipal = _claims.CreateClaims(identity);
                            context.HttpContext.User = claimsPrincipal;
                            return;
                        }
                        catch (AggregateException exs)
                        {
                            exs.Handle((ex) =>
                            {
                                context.Result = new ObjectResult(ex.Message) { StatusCode = (int)HttpStatusCode.Unauthorized };
                                return true;
                            });

                            return;
                        }
                    }
                }
            }

            context.Result = new ObjectResult("Must provide Authorization Header: Bearer {JWT}.") { StatusCode = (int)HttpStatusCode.Unauthorized };
        }
    }
}