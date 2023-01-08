using Chetvyorochka.BL.Services;
using Chetvyorochka.DAL;
using Chetvyorochka.DAL.Entities;
using Chetvyorochka.DAL.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace Chetvyorochka.PL
{
    public static class DataSeeder
    {
        public static void Seed(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            using var contextDB = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            using var contextUser = scope.ServiceProvider.GetRequiredService<IUserRequest>();
            //contextDB.Database.EnsureDeleted();
            contextDB.Database.EnsureCreated();
            AddUsers(contextDB, contextUser);
        }
        public static void AddUsers(ApplicationContext context, IUserRequest userRequest)
        {
            var user = context.Users.FirstOrDefault();
            if (user != null) return;

            context.Users.Add(new User
            {
                Login = "admin",
                Name = "Админ",
                LastName = "Админов",
                UserType = UserType.admin,
                MoneyCount = 0,
                Password = userRequest.HashPassword("1234")
            });

            context.SaveChanges();
        }

        /*
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                return hash;
            }
        }
        */
    }
}