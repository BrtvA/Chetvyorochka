using Chetvyorochka.DAL.Entities;
using Chetvyorochka.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chetvyorochka.DAL.Repositories
{
    public interface IProductTypeDbRepository<T>: IDisposable
        where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetAsync(int id);
        Task<T?> GetByNameAsync(string name);
        void Delete(T productType); // удаление категории
        Task CreateAsync(T productType);
        Task SaveAsync();
    }
}
