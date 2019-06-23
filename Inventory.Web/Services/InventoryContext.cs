using Inventory.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Web.Services
{
    public class InventoryContext : IdentityDbContext<ApplicationUser>
    {
        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options)
        { }

        public DbSet<Item> Items { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Container> Containers { get; set; }
        public DbSet<Borrower> Borrowers { get; set; }

        // Access just like a regular DbSet:
        //    var items = _context.ItemFullViews.ToList();
        public DbQuery<ItemFullView> ItemFullViews { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .Query<ItemFullView>().ToView("View_ItemFullView");
        }
    }
}
