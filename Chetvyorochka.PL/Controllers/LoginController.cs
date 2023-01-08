using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Chetvyorochka.BL.Services;
using Chetvyorochka.DAL.Repositories;
using Chetvyorochka.DAL.Entities;
using Chetvyorochka.BL.Models;

namespace Chetvyorochka.PL.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IUserRequest _userRequest;
        public LoginController(IUserDbRepository<User> dbRepository,
            IUserRequest userRequest,
            ILogger<LoginController> logger)
        {
            _logger = logger;
            _userRequest = userRequest;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Index()
        {
            return View("~/Views/Login/LoginPage.cshtml");
        }

        [HttpPost]
        [Route("/api/login")]
        public async Task<IActionResult> Login([FromBody] LoginDataModel loginDataModel)
        {
            try
            {
                string token = await _userRequest.LoginAsync(loginDataModel);
                Response.Cookies.Append("Token", token);
                return Ok(new { errorText = "Ok" });
            }
            finally
            {
                _userRequest.Dispose();
            }
        }

        [HttpPost]
        [Route("/api/register")]
        public async Task<IActionResult> Register([FromBody] RegisterDataModel registerDataModel)
        {
            try
            {
                string token = await _userRequest.RegisterAsync(registerDataModel);
                Response.Cookies.Append("Token", token);
                return Ok(new { errorText = "Ok" });
            }
            finally
            {
                _userRequest.Dispose();
            }
        }
    }
}
