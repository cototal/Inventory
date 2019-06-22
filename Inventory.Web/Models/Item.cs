using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Web.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int ContainerId { get; set; }
        public Container Container { get; set; }

        public ICollection<Borrower> Borrowers { get; set; }
    }
}
