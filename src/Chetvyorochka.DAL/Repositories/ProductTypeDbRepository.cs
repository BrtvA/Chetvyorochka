using Chetvyorochka.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chetvyorochka.DAL.Repositories
{
    public class ProductTypeDbRepository : IProductTypeDbRepository<ProductType>
    {

        private ApplicationContext _db;

        public ProductTypeDbRepository(DbContextOptions<ApplicationContext> dbContextOptions)
        {
            _db = new ApplicationContext(dbContextOptions);
        }

        public async Task<IEnumerable<ProductType>> GetAllAsync()
        {
            return await _db.ProductTypes.OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<ProductType?> GetAsync(int id)
        {
            return await _db.ProductTypes.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ProductType?> GetByNameAsync(string name)
        {
            return await _db.ProductTypes.AsNoTrackingWithIdentityResolution().Where(x => x.Name == name).FirstOrDefaultAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public void Update(ProductType productType)
        {
            _db.Entry(productType).State = EntityState.Modified;
        }

        public void Delete(ProductType productType)
        {
            _db.ProductTypes.Remove(productType);
        }

        public async Task CreateAsync(ProductType productType)
        {
            await _db.ProductTypes.AddAsync(productType);
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

        ~ProductTypeDbRepository()
        {
            Dispose(false);
        }
    }
}
