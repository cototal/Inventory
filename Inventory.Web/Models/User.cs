using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Web.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public ICollection<Room> Rooms { get; set; }
    }
}
