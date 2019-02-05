using System;
using eventPublisher.domain.contracts;
using eventPublisher.domain.models;
using Microsoft.AspNetCore.Mvc;

namespace eventPublisher.web.Controllers
{
    public class BaseController : Controller
	{
        private IManageClaims _claims;

		public BaseController(IManageClaims claims)
		{
			_claims = claims ?? throw new ArgumentNullException("IManageClaims");
		}

        public Identity GetIdentityFromClaims()
		{
			return _claims.GetIdentityFromClaims(Request.HttpContext.User);
		}
    }
}