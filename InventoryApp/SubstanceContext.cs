using System;
using System.Configuration;
using InventoryManagement;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace InventoryManagement
{
    public class SubstanceContext : DbContext
    {
        public DbSet<Substance> ReferenceSubstances { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public SubstanceContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer($"Data Source ={ConfigurationManager.AppSettings["server"]}; Integrated Security = SSPI; TrustServerCertificate=True; User Instance = false; Database = Substances;");

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Substance>().HasKey(e => e.BatchNumber);
            modelBuilder.Entity<Order>().HasKey(e => e.Id);

            modelBuilder.Entity<Order>().HasMany(a => a.OrderDetails).WithOne(ac => ac.Order).HasForeignKey(ac => ac.OrderId);
            modelBuilder.Entity<Substance>().HasMany(c => c.OrderDetails).WithOne(ac => ac.Substance).HasForeignKey(ac => ac.SubstanceId);

            modelBuilder.Entity<OrderDetail>().ToTable("OrderDetails");
            modelBuilder.Entity<OrderDetail>().HasKey(ac => new { ac.OrderId, ac.SubstanceId });
        }
    }
}