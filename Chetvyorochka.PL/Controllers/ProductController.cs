using Chetvyorochka.BL.Services;
using Chetvyorochka.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chetvyorochka.PL.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRequest _productRequest;
        private readonly ILogger<ProductController> _logger;

        public ProductController(
            IProductRequest productRequest,
            ILogger<ProductController> logger)
        {
            _productRequest = productRequest;
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        ///[Route("/api/logout")]
        public async Task<IActionResult> Add([FromBody] Product product)
        {
            if (!User.IsInRole("admin"))
            {
                //return RedirectToAction("Index", "Home");
                return Unauthorized(new { errorText = "Нет доступа" });
            }

            try
            {
                await _productRequest.AddProductAsync(product);
                return Ok(new { errorText = "Продукт добавлен" });
            }
            finally
            {
                _productRequest.Dispose();
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                await _productRequest.DeleteProductAsync(id);
                return Ok(new { errorText = "Продукт удалён" });
            }
            finally
            {
                _productRequest.Dispose();
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {

            if (!User.IsInRole("admin"))
            {
                return Unauthorized(new { errorText = "Нет доступа" });

                //return RedirectToAction("Index", "Home");
            }

            try
            {
                Product resultRequest = await _productRequest.GetProductAsync(id);
                return Json(resultRequest);
            }
            finally
            {
                _productRequest.Dispose();
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                if (User.IsInRole("admin") || User.IsInRole("customer"))
                {
                    return Json(await _productRequest.GetAllProductAsync());
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            finally
            {
                _productRequest.Dispose();
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit([FromBody] Product product)
        {
            if (!User.IsInRole("admin"))
            {
                return Unauthorized(new { errorText = "Нет доступа" });
            }

            try
            {
                await _productRequest.EditProductAsync(product);
                return Ok(new { errorText = "Продукт изменен" });
            }
            finally
            {
                _productRequest.Dispose();
            }
        }
    }
}
