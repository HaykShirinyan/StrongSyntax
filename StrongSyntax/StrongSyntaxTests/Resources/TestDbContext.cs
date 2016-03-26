using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using StrongSyntaxTests.Resources.Models;

namespace StrongSyntaxTests.Resources
{
    class TestDbContext : DbContext
    {
        public DbSet<InvItem> InvItems { get; set; }
        public DbSet<UnitOfMeasure> UnitOfMeasures { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseItem> WarehouseItems { get; set; }
    }
}
