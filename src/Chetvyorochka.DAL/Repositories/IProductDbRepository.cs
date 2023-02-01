namespace Chetvyorochka.DAL.Repositories
{
    public interface IProductDbRepository<T>: IDisposable
        where T: class
    {
        Task<IEnumerable<T>> GetAllAsync(); // получение всех объектов
        Task<T?> GetAsync(int id); // получение одного объекта по id
        Task<T?> GetByNameAsync(string name); // получение объекта по названию
        Task<int> GetCountAsync(int id);
        Task CreateAsync(T product); // создание объекта
        //void Update(T product); // обновление объекта
        void Delete(T product); // удаление объекта по id
        Task SaveAsync();  // сохранение изменений
    }
}
