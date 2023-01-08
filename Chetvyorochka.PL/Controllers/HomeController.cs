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

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                //Response.Redirect("/Login/Index");
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

        [Authorize]
        ///[Route("/api/logout")]
        public IActionResult Logout()
        {
            _logger.LogInformation($"Пользователь {User.Identity.Name} вышел из аккаунта");
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete("Token");
            //Response.Cookies.Delete("UserType");
            return RedirectToAction("Index", "Login");
        }
    }
}
