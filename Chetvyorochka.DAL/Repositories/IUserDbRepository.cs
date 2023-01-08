using Chetvyorochka.DAL.Models;

namespace Chetvyorochka.DAL.Repositories
{
    public interface IUserDbRepository<T>: IDisposable
        where T : class
    {
        Task<T?> GetByNameAsync(string login); // получение объекта по названию
        Task<UserInfoModel?> GetUserInfoAsync(string login);
        Task<UserAuthentificationModel?> GetAuthInfoAsync(string login); // данные для аутентификации
        Task CreateAsync(T user); // создание объекта
        void Update(T user); // обновление объекта
        Task DeleteAsync(string login); // удаление объекта по id
        Task SaveAsync();  // сохранение изменений
    }
}
