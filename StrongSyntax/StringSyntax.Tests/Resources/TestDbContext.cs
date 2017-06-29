using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StrongSyntax.Tests.Resources.Models;
using Microsoft.EntityFrameworkCore;

namespace StrongSyntax.Tests.Resources
{
    class TestDbContext : DbContext
    {
        private void SetSequentialID<T>(ModelBuilder modelBuilder)
            where T : ModelBase
        {
            modelBuilder
                .Entity<T>()
                .Property(e => e.ID)
                .HasDefaultValueSql("NEWSEQUENTIALID()");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetSequentialID<InvItem>(modelBuilder);
            SetSequentialID<UnitOfMeasure>(modelBuilder);
            SetSequentialID<Warehouse>(modelBuilder);

            modelBuilder
                .Entity<WarehouseItem>()
                .HasKey(e => new { e.InvItemID, e.WarehouseID });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=.\SQLEXPRESS01;Initial Catalog=StrongSyntaxTests.Resources.TestDbContext;Integrated Security=True");
        }

        public DbSet<InvItem> InvItems { get; set; }
        public DbSet<UnitOfMeasure> UnitOfMeasures { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseItem> WarehouseItems { get; set; }
    }
}
