using Chetvyorochka.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chetvyorochka.BL.Services
{
    public interface IProductTypeRequest:IDisposable
    {
        Task<IEnumerable<ProductType>> GetAllProductTypeAsync();
        Task<ProductType> GetProductTypeAsync(int id);
        Task EditProductTypeAsync(ProductType productType);
        Task AddProductTypeAsync(string name);
        Task DeleteProductTypeAsync(int id);
    }
}
