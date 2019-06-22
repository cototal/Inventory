using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Web.Models
{
    public class Borrower
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateLent { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}
