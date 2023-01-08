using Chetvyorochka.DAL.Entities;

namespace Chetvyorochka.DAL.Models
{
    public class UserAuthentificationModel
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public UserType UserType { get; set; }
    }
}
