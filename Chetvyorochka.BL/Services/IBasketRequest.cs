using Chetvyorochka.DAL.Models;

namespace Chetvyorochka.BL.Services
{
    public interface IBasketRequest: IDisposable
    {
        Task IncreaseCountProductAsync(string login, int productId);
        Task ReduceCountProductAsync(string login, int productId);
        Task BuyProductsAsync(string login);
        Task<IEnumerable<BasketInfoModel>?> GetAllBasketAsync(string login);
    }
}
