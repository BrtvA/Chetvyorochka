using Chetvyorochka.BL.Services;
using Chetvyorochka.PL.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Chetvyorochka.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRequest _userRequest;

        public UserController(IUserRequest userRequest)
        {
            _userRequest = userRequest;
        }

        [HttpGet]
        [MyAuthorize]
        public async Task<IActionResult> GetInfo() // Информация о имени и счете покупателя
        {
            try
            {
                return Json(await _userRequest.GetUserInfoAsync(User.Identity.Name));
            }
            finally
            {
                _userRequest.Dispose();
            }
        }

        [HttpPost]
        [MyAuthorize(Roles = "customer")]
        public async Task<IActionResult> AddMoney([FromBody] decimal money)
        {
            try
            {
                await _userRequest.AddMoneyAsync(User.Identity.Name, money);
                return Ok(new { errorText = "Cчёт пополнен" });
            }
            finally
            {
                _userRequest.Dispose();
            }
        }
    }
}
