using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chetvyorochka.DAL.Entities
{
    public class Basket
    {
        public string UserLogin { get; set; } = null!;
        public int ProductId { get; set; }
        public int ProductCount { get; set; }
        public Product? Product { get; set; }
        public User? User { get; set; }
    }
}
