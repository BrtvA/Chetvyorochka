using Chetvyorochka.DAL.Entities;
using Chetvyorochka.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Chetvyorochka.DAL.Repositories
{
    public class BasketDbRepository : IBasketDbRepository<Basket>
    {
        private ApplicationContext _db;

        public BasketDbRepository(DbContextOptions<ApplicationContext> dbContextOptions)
        {
            _db = new ApplicationContext(dbContextOptions);
        }

        public async Task<IEnumerable<BasketInfoModel>> GetAllInfoAsync(string login)
        {
            return await Task.Run(() => _db.Baskets.Where(x => x.UserLogin == login)
                .OrderBy(x => x.Product.ProductTypeId)
                .ThenBy(x=>x.ProductId)
                .Join(_db.Products,
                    u => u.ProductId,
                    c => c.Id,
                    (u, c) => new BasketInfoModel
                    {
                        Id = u.ProductId,
                        Name = c.Name,
                        Price = c.Price,
                        Count = u.ProductCount
                    }
                ));
        }

        public async Task<string> BuyInfoAsync(string login)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                User? user = await _db.Users.Where(x => x.Login == login).FirstOrDefaultAsync();
                decimal totalPrice = await _db.Baskets.AsNoTracking().Where(x => x.UserLogin == login)
                .Join(_db.Products,
                    u => u.ProductId,
                    c => c.Id,
                    (u, c) => c.Price * u.ProductCount)
                .SumAsync();
                if (user.MoneyCount >= totalPrice)
                {
                    var buyInfo = await Task.Run(() => _db.Baskets.AsNoTracking().Where(x => x.UserLogin == login)
                        .Join(_db.Products,
                            u => u.ProductId,
                            c => c.Id,
                            (u, c) => new BasketBuyModel
                            {
                                Id = u.ProductId,
                                Name = c.Name,
                                ProductCount = c.Count,
                                BasketCount = u.ProductCount,
                                Description = c.Description,
                                ProductTypeId = c.ProductTypeId,
                                Price = c.Price
                            }));

                    foreach (var item in buyInfo)
                    {
                        if (item.BasketCount <= item.ProductCount)
                        {
                            _db.Entry(new Product
                            {
                                Id = item.Id,
                                Name = item.Name,
                                Description = item.Description,
                                ProductTypeId = item.ProductTypeId,
                                Price = item.Price,
                                Count = item.ProductCount - item.BasketCount
                            }).State = EntityState.Modified;
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException($"Недостаточное количество \"{item.Name}\" на складе");
                        }
                    }
                    user.MoneyCount -= totalPrice;
                    DeleteAll(login);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Недостаточное количество средств на счёте");
                }
                transaction.Commit();
                return "Ok";
            }
            catch (ArgumentOutOfRangeException ex)
            {
                transaction.Rollback();
                return ex.ParamName;
            }
        }

        public async Task<int> GetProductCountAsync(string login, int id)
        {
            return await _db.Baskets.AsNoTrackingWithIdentityResolution().Where(x => x.UserLogin == login && x.ProductId == id).Select(x => x.ProductCount).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Basket basket)
        {
            await _db.Baskets.AddAsync(basket);
        }

        public void DeleteAll(string login)
        {
            var basketItem = _db.Baskets.Where(x => x.UserLogin == login);
            if (basketItem != null)
                _db.Baskets.RemoveRange(basketItem);
        }

        public async Task DeleteAsync(string login, int productId)
        {
            var basketItem = await _db.Baskets.FindAsync(login, productId);
            if (basketItem != null)
                _db.Baskets.Remove(basketItem);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public void Update(Basket basket)
        {
            _db.Entry(basket).State = EntityState.Modified;
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
                _db.Dispose();
            }
            // освобождаем неуправляемые объекты
            disposed = true;
        }

        ~BasketDbRepository()
        {
            Dispose(false);
        }
    }
}
