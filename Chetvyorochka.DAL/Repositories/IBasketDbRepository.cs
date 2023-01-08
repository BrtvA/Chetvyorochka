using Chetvyorochka.DAL.Models;

namespace Chetvyorochka.DAL.Repositories
{
    public interface IBasketDbRepository<T>: IDisposable
        where T : class
    {
        Task<int> GetProductCountAsync(string login, int id);//?????
        Task<string> BuyInfoAsync(string login);
        Task<IEnumerable<BasketInfoModel>> GetAllInfoAsync(string login); // получение информации о корзине пользователя
        Task CreateAsync(T basket); // создание объекта в корзине
        Task DeleteAsync(string login, int productId); // удаление заказа в корзине
        void DeleteAll(string login);
        void Update(T basket);
        Task SaveAsync();
    }
}
