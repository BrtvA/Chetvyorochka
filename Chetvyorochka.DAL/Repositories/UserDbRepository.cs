using Chetvyorochka.DAL.Entities;
using Chetvyorochka.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Chetvyorochka.DAL.Repositories
{
    public class UserDbRepository : IUserDbRepository<User>
    {
        private ApplicationContext _db;

        public UserDbRepository(DbContextOptions<ApplicationContext> dbContextOptions)
        {
            _db = new ApplicationContext(dbContextOptions);
        }

        public async Task<UserAuthentificationModel?> GetAuthInfoAsync(string login)
        {
            return await _db.Users.AsNoTracking().Where(x => x.Login == login).Select(x => new UserAuthentificationModel { Login = x.Login, Password = x.Password, UserType = x.UserType }).FirstOrDefaultAsync();
        }

        public async Task<User?> GetByNameAsync(string login)
        {
            return await _db.Users.Where(x => x.Login == login).FirstOrDefaultAsync();///
        }

        public async Task<UserInfoModel?> GetUserInfoAsync(string login)
        {
            return await _db.Users.AsNoTrackingWithIdentityResolution().Where(x => x.Login == login).Select(x => new UserInfoModel { Name = x.Name, LastName = x.LastName, MoneyCount = x.MoneyCount }).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(User user)
        {
            await _db.Users.AddAsync(user);
        }

        public async Task DeleteAsync(string login)
        {
            User? user = await _db.Users.FindAsync(login);
            if (user != null)
                _db.Users.Remove(user);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public void Update(User user)
        {
            _db.Entry(user).State = EntityState.Modified;
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
        ~UserDbRepository()
        {
            Dispose(false);
        }
    }
}
