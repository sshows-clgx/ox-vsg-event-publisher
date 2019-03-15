using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eventPublisher.web.Controllers
{
    [AllowAnonymous]
    public class HomeController: Controller
    {
        public IActionResult Index()
        {
            return View("~/wwwroot/app/index.html");
        }
    }
}