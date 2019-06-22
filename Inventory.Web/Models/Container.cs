using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Web.Models
{
    public class Container
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}
