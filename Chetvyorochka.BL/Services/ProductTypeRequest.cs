using Chetvyorochka.BL.CustomExceptions;
using Chetvyorochka.DAL.Entities;
using Chetvyorochka.DAL.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chetvyorochka.BL.Services
{
    public class ProductTypeRequest : IProductTypeRequest
    {
        private readonly IProductTypeDbRepository<ProductType> _productTypeDbRepository;
        private readonly ILogger<ProductTypeRequest> _logger;
        public ProductTypeRequest(IProductTypeDbRepository<ProductType> productTypeDbRepository,
            ILogger<ProductTypeRequest> logger)
        {
            _productTypeDbRepository = productTypeDbRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductType>> GetAllProductTypeAsync()
        {
            return await _productTypeDbRepository.GetAllAsync();
        }

        public async Task<ProductType> GetProductTypeAsync(int id)
        {
            var productType = await _productTypeDbRepository.GetAsync(id);
            if (productType == null)
                throw new NotFoundException("Такой категории больше нет");
            return productType;
        }

        public async Task EditProductTypeAsync(ProductType productType)
        {
            var prodType = await _productTypeDbRepository.GetAsync(productType.Id);
            if (prodType == null)
                throw new NotFoundException("Такой категории больше нет");

            prodType.Id = productType.Id;
            prodType.Name = productType.Name;

            //_productTypeDbRepository.Update(new ProductType
            //{
            //    Id = productType.Id,
            //    Name = productType.Name
            //});
            await _productTypeDbRepository.SaveAsync();
        }

        public async Task DeleteProductTypeAsync(int id)
        {
            var product = await _productTypeDbRepository.GetAsync(id);
            if (product == null)
                throw new NotFoundException("Нельзя удалить несуществующую категорию");

            _productTypeDbRepository.Delete(product);
            await _productTypeDbRepository.SaveAsync();
        }

        public async Task AddProductTypeAsync(string name)
        {
            var productTypeEntity = await _productTypeDbRepository.GetByNameAsync(name);
            if (productTypeEntity != null)
                throw new BadRequestException("Такая категория уже есть");

            await _productTypeDbRepository.CreateAsync(new ProductType
            {
                Name = name
            });
            await _productTypeDbRepository.SaveAsync();
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
                _productTypeDbRepository.Dispose();
            }
            // освобождаем неуправляемые объекты
            disposed = true;
        }

        ~ProductTypeRequest()
        {
            Dispose(false);
        }
    }
}
