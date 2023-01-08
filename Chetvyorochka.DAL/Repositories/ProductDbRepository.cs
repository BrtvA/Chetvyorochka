using Chetvyorochka.DAL.Entities;
using Chetvyorochka.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Chetvyorochka.DAL.Repositories
{
    public class ProductDbRepository : IProductDbRepository<Product>
    {
        private ApplicationContext _db;

        public ProductDbRepository(DbContextOptions<ApplicationContext> dbContextOptions)
        {
            _db = new ApplicationContext(dbContextOptions);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _db.Products.AsNoTrackingWithIdentityResolution()
                .OrderBy(x => x.ProductTypeId)
                .ThenBy(x=>x.Id)
                .ToListAsync();
        }

        public async Task<Product?> GetAsync(int id)
        {
            return await _db.Products.Where(x=>x.Id==id).FirstOrDefaultAsync();
        }

        public async Task<Product?> GetByNameAsync(string name)
        {
            return await _db.Products.AsNoTrackingWithIdentityResolution().Where(x => x.Name == name).FirstOrDefaultAsync();
        }

        public async Task<int> GetCountAsync(int id) 
        {
            return await _db.Products.AsNoTracking().Where(x => x.Id == id).Select(x => x.Count).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Product product)
        {
            await _db.Products.AddAsync(product);
        }

        public void Delete(Product product)
        {
            _db.Products.Remove(product);
            /*
            Product? product = await _db.Products.FindAsync(id);
            if (product!=null)
                _db.Products.Remove(product);
            */
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        /*
        public void Update(Product product)
        {
            _db.Entry(product).State = EntityState.Modified;
        }
        */

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

        ~ProductDbRepository()
        {
            Dispose(false);
        }
    }
}
