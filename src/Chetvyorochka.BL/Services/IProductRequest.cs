using Chetvyorochka.DAL.Entities;

namespace Chetvyorochka.BL.Services
{
    public interface IProductRequest: IDisposable
    {
        Task AddProductAsync(Product product);
        Task DeleteProductAsync(int id);
        Task<Product> GetProductAsync(int id);
        Task<IEnumerable<Product>> GetAllProductAsync();
        Task EditProductAsync(Product product);
    }
}
