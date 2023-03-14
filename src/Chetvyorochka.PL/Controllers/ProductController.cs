using Chetvyorochka.BL.Services;
using Chetvyorochka.DAL.Entities;
using Chetvyorochka.PL.Filters;
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
        [MyAuthorize(Roles = "admin")]
        public async Task<IActionResult> Add([FromBody] Product product)
        {
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
        [MyAuthorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
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
        [MyAuthorize(Roles = "admin")]
        public async Task<IActionResult> Get(int id)
        {
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
        [MyAuthorize]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Json(await _productRequest.GetAllProductAsync());
            }
            finally
            {
                _productRequest.Dispose();
            }
        }

        [HttpPost]
        [MyAuthorize(Roles = "admin")]
        public async Task<IActionResult> Edit([FromBody] Product product)
        {
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
