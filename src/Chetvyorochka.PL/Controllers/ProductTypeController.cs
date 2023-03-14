using Chetvyorochka.BL.Services;
using Chetvyorochka.DAL.Entities;
using Chetvyorochka.PL.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

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
        [MyAuthorize(Roles = "admin")]
        public async Task<IActionResult> Add([FromBody] ProductType productType)
        {
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
        [MyAuthorize]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Json(await _productTypeRequest.GetAllProductTypeAsync());
            }
            finally
            {
                _productTypeRequest.Dispose();
            }
        }

        [HttpGet]
        [MyAuthorize(Roles = "admin")]
        public async Task<IActionResult> Get(int id)
        {
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
        [MyAuthorize(Roles = "admin")]
        public async Task<IActionResult> Edit([FromBody] ProductType productType)
        {
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
        [MyAuthorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
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
