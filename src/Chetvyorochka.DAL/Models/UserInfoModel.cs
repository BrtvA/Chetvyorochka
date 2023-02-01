using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chetvyorochka.DAL.Models
{
    public class UserInfoModel
    {
        public string Name { get; set; } = null!;
        public string? LastName { get; set; }
        public decimal MoneyCount { get; set; }
    }
}
