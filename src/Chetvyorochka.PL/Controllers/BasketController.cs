using Chetvyorochka.BL.Services;
using Chetvyorochka.PL.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Chetvyorochka.PL.Controllers
{
    public class BasketController : Controller
    {
        private readonly IBasketRequest _basketRequest;

        public BasketController(IBasketRequest basketRequest)
        {
            _basketRequest = basketRequest;
        }

        [HttpGet]
        [MyAuthorize(Roles = "customer")]
        public async Task<IActionResult> Buy()
        {
            try
            {
                await _basketRequest.BuyProductsAsync(User.Identity.Name);
                return Ok(new { errorText = "Платёж совершён" });
            }
            finally
            {
                _basketRequest.Dispose();
            }
        }

        [HttpPost]
        [MyAuthorize(Roles = "customer")]
        public async Task<IActionResult> IncreaseCount([FromBody] int id)
        {
            try
            {
                await _basketRequest.IncreaseCountProductAsync(User.Identity.Name, id);
                return Ok(new { errorText = "Товар добавлен в корзину" });
            }
            finally
            {
                _basketRequest.Dispose();
            }
        }

        [HttpPost]
        [MyAuthorize(Roles = "customer")]
        public async Task<IActionResult> ReduceCount([FromBody] int id)
        {
            try
            {
                await _basketRequest.ReduceCountProductAsync(User.Identity.Name, id);
                return Ok("Товар убран из корзины");
            }
            finally
            {
                _basketRequest.Dispose();
            }
        }

        [HttpGet]
        [MyAuthorize(Roles = "customer")]
        public async Task<IActionResult> GetAll()
        {
            return Json(await _basketRequest.GetAllBasketAsync(User.Identity.Name));
        }
    }
}
