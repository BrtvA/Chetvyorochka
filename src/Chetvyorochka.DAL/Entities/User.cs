using System.ComponentModel.DataAnnotations;

namespace Chetvyorochka.DAL.Entities
{
    public class User
    {
        //[Key]
        //[MaxLength(20)]
        public string Login { get; set; } = null!;
        [MaxLength(20)]
        public string Name { get; set; } = null!;
        [MaxLength(20)]
        public string? LastName { get; set; }
        public UserType UserType { get; set; }
        public decimal MoneyCount { get; set; }
        public string Password { get; set; } = null!;
        public List<Basket> Baskets { get; set; } = new();
    }
}
