using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Chetvyorochka.BL.Services;
using Chetvyorochka.DAL.Repositories;
using Chetvyorochka.DAL.Entities;
using Chetvyorochka.BL.Models;
using Microsoft.AspNetCore.Authorization;

namespace Chetvyorochka.PL.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserRequest _userRequest;
        public LoginController(IUserDbRepository<User> dbRepository,
                               IUserRequest userRequest)
        {
            _userRequest = userRequest;
        }

        //[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [AllowAnonymous]
        public IActionResult Index()
        {
            var token = Request.Cookies["Token"];
            if (!String.IsNullOrEmpty(token))
            {
                Response.Cookies.Delete("Token");
            }
            return View("~/Views/Login/LoginPage.cshtml");
        }

        [HttpPost]
        [Route("/Login")]
        [AllowAnonymous]
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
        [Route("/Register")]
        [AllowAnonymous]
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
