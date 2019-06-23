using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Web.Models
{
    public class ItemFullView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ContainerId { get; set; }
        public string ContainerName { get; set; }
        public string ContainerDescription { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public string RoomDescription { get; set; }
    }
}
