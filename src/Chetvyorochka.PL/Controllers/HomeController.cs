using Chetvyorochka.PL.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chetvyorochka.PL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [AllowAnonymous]
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Login");
            }

            if (User.IsInRole("admin"))
            {
                return View("~/Views/Home/AdminPage.cshtml");
            }
            else
            {
                return View("~/Views/Home/CustomerPage.cshtml");
            }
        }

        [HttpGet]
        [MyAuthorize]
        public IActionResult Logout()
        {
            _logger.LogInformation($"{DateTime.Now}: Пользователь {User.Identity.Name} вышел из аккаунта");
            Response.Cookies.Delete("Token");
            return RedirectToAction("Index", "Login");
        }
    }
}
