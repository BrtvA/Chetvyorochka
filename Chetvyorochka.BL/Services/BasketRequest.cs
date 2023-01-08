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
    public class BasketRequest : IBasketRequest
    {
        private readonly IBasketDbRepository<Basket> _basketDbRepository;
        private readonly IProductDbRepository<Product> _productDbRepository;
        private readonly ILogger<BasketRequest> _logger;
        public BasketRequest(IBasketDbRepository<Basket> basketDbRepository,
            IProductDbRepository<Product> productDbRepository,
            ILogger<BasketRequest> logger)
        {
            _basketDbRepository = basketDbRepository;
            _productDbRepository = productDbRepository;
            _logger = logger;
        }

        public async Task BuyProductsAsync(string login)
        {
            string buyInfo = await _basketDbRepository.BuyInfoAsync(login);
            if (buyInfo != "Ok")
            {
                _logger.LogError(DateTime.UtcNow.ToLongTimeString() + ": " + buyInfo);
                throw new BadRequestException(buyInfo);
            }
        }

        public async Task<IEnumerable<BasketInfoModel>?> GetAllBasketAsync(string login)
        {
            return await _basketDbRepository.GetAllInfoAsync(login);
        }

        public async Task IncreaseCountProductAsync(string login, int productId)
        {
            int productCount = await _productDbRepository.GetCountAsync(productId);
            if (productCount < 1)
            {
                await _basketDbRepository.DeleteAsync(login, productId);
                await _basketDbRepository.SaveAsync();
                throw new NotFoundException("Товар отсутствует на складе");
            }

            var countProductInBasket = await _basketDbRepository.GetProductCountAsync(login, productId);
            if (countProductInBasket > 0)
            {
                if (productCount<(countProductInBasket+1))
                    throw new NotFoundException("Недостаточное количество товара на складе");

                _basketDbRepository.Update(new Basket
                {
                    UserLogin = login,
                    ProductId = productId,
                    ProductCount = countProductInBasket + 1
                });
            }
            else
            {
                await _basketDbRepository.CreateAsync(new Basket
                {
                    UserLogin = login,
                    ProductId = productId,
                    ProductCount = 1
                });
            }
            await _basketDbRepository.SaveAsync();
        }

        public async Task ReduceCountProductAsync(string login, int productId)
        {
            int productCount = await _productDbRepository.GetCountAsync(productId);
            if (productCount < 1)
            {
                await _basketDbRepository.DeleteAsync(login, productId);
                await _basketDbRepository.SaveAsync();
                throw new NotFoundException("Товар отсутствует на складе");
            }

            var countProduct = await _basketDbRepository.GetProductCountAsync(login, productId);
            if (countProduct > 1)
            {
                _basketDbRepository.Update(new Basket
                {
                    UserLogin = login,
                    ProductId = productId,
                    ProductCount = countProduct - 1
                });
            }
            else
            {
                await _basketDbRepository.DeleteAsync(login, productId);
            }
            await _basketDbRepository.SaveAsync();
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
                _basketDbRepository.Dispose();
                _productDbRepository.Dispose();
            }
            // освобождаем неуправляемые объекты
            disposed = true;
        }

        ~BasketRequest()
        {
            Dispose(false);
        }
    }
}
