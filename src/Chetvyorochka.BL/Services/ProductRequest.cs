using Chetvyorochka.BL.CustomExceptions;
using Chetvyorochka.DAL.Entities;
using Chetvyorochka.DAL.Models;
using Chetvyorochka.DAL.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chetvyorochka.BL.Services
{
    public class ProductRequest : IProductRequest
    {
        private readonly IProductDbRepository<Product> _productDbRepository;
        private readonly ILogger<ProductRequest> _logger;
        public ProductRequest(IProductDbRepository<Product> productDbRepository,
            ILogger<ProductRequest> logger)
        {
            _productDbRepository = productDbRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Product>> GetAllProductAsync()
        {
            return await _productDbRepository.GetAllAsync();
        }

        public async Task<Product> GetProductAsync(int id)
        {
            var product = await _productDbRepository.GetAsync(id);
            if (product == null)
            {
                throw new NotFoundException("Такого продукта больше нет");
            }

            return product;
        }

        public async Task AddProductAsync(Product product)
        {
            var productEntity = await _productDbRepository.GetByNameAsync(product.Name);
            if (productEntity != null)
                throw new BadRequestException("Такой продукт уже есть");

            await _productDbRepository.CreateAsync(new Product
            {
                Name = product.Name,
                Description = product.Description,
                ProductTypeId = product.ProductTypeId,
                Price = product.Price,
                Count = product.Count
            });
            await _productDbRepository.SaveAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _productDbRepository.GetAsync(id);
            if (product == null)
                throw new NotFoundException("Нельзя удалить несуществующий продукт");

            _productDbRepository.Delete(product);
            await _productDbRepository.SaveAsync();
        }

        public async Task EditProductAsync(Product product)
        {
            var prod = await _productDbRepository.GetAsync(product.Id);
            if (prod == null)
                throw new NotFoundException("Такого продукта больше нет");

            prod.Name = product.Name;
            prod.Description = product.Description;
            prod.ProductTypeId = product.ProductTypeId;
            prod.Price = product.Price;
            prod.Count = product.Count;

            await _productDbRepository.SaveAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {

            if (disposed) return;
            if (disposing)
            {
                // Освобождаем управляемые ресурсы
                _productDbRepository.Dispose();
            }
            // освобождаем неуправляемые объекты
            disposed = true;
        }

        ~ProductRequest()
        {
            Dispose(false);
        }
    }
}
