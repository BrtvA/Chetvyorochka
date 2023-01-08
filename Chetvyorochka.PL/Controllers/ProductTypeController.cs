using Chetvyorochka.BL.Services;
using Chetvyorochka.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chetvyorochka.PL.Controllers
{
    public class ProductTypeController : Controller
    {
        private readonly IProductTypeRequest _productTypeRequest;

        public ProductTypeController(IProductTypeRequest productTypeRequest)
        {
           _productTypeRequest = productTypeRequest;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] ProductType productType)
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                await _productTypeRequest.AddProductTypeAsync(productType.Name);
                return Ok(new { errorText = "Категория добавлена" });
            }
            finally
            {
                _productTypeRequest.Dispose();
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
                    return Json(await _productTypeRequest.GetAllProductTypeAsync());
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            finally
            {
                _productTypeRequest.Dispose();
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                return Json(await _productTypeRequest.GetProductTypeAsync(id));
            }
            finally
            {
                _productTypeRequest.Dispose();
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit([FromBody] ProductType productType)
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                await _productTypeRequest.EditProductTypeAsync(productType);
                return Ok(new { errorText = "Категория изменена" });
            }
            finally
            {
                _productTypeRequest.Dispose();
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
                await _productTypeRequest.DeleteProductTypeAsync(id);
                return Ok(new { errorText = "Категория изменена" });
            }
            finally
            {
                _productTypeRequest.Dispose();
            }
        }
    }
}
