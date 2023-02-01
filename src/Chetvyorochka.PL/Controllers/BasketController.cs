using Chetvyorochka.BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chetvyorochka.PL.Controllers
{
    public class BasketController : Controller
    {
        private readonly IBasketRequest _basketRequest;
        private readonly ILogger<BasketController> _logger;

        public BasketController(
            IBasketRequest basketRequest,
            ILogger<BasketController> logger)
        {
            _basketRequest = basketRequest;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Buy()
        {

            if (!User.IsInRole("customer"))
            {
                return RedirectToAction("Index", "Home");
            }

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
        [Authorize]
        public async Task<IActionResult> IncreaseCount([FromBody] int id)
        {
            if (!User.IsInRole("customer"))
            {
                return RedirectToAction("Index", "Home");
            }

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
        [Authorize]
        public async Task<IActionResult> ReduceCount([FromBody] int id)
        {
            if (!User.IsInRole("customer"))
            {
                return RedirectToAction("Index", "Home");
            }

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
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            if (!User.IsInRole("customer"))
            {
                return RedirectToAction("Index", "Home");
            }

            return Json(await _basketRequest.GetAllBasketAsync(User.Identity.Name));
        }
    }
}
