using System;
using System.Configuration;
using InventoryManagement;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace InventoryManagement
{
    internal class SubstanceContext : DbContext
    {
        public DbSet<Substance> ReferenceSubstances { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public SubstanceContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer($"Data Source ={ConfigurationManager.AppSettings["server"]}; Integrated Security = SSPI; TrustServerCertificate=True; User Instance = false; Database = Substances;");

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Substance>();
            modelBuilder.Entity<Order>().HasKey(x => x.Id);

            modelBuilder.Entity<Order>().HasMany(x => x.OrderDetails).WithOne(xc => xc.Order).HasForeignKey(xc => xc.OrderId);
            modelBuilder.Entity<Substance>().HasMany(x => x.OrderDetails).WithOne(xc => xc.Substance).HasForeignKey(xc => xc.SubstanceId);

            modelBuilder.Entity<OrderDetail>().ToTable("OrderDetails");
            modelBuilder.Entity<OrderDetail>().HasKey(x => x.DetailId);
            //modelBuilder.Entity<OrderDetail>().HasKey(ac => new { ac.OrderId, ac.SubstanceId });
            /*modelBuilder.Entity<OrderDetail>().HasOne(a => a.Order).WithMany(b => b.OrderDetails).HasForeignKey(ac => ac.DetailId);
            modelBuilder.Entity<OrderDetail>().HasOne(d => d.Substance).WithMany(b => b.OrderDetails).HasForeignKey(ac => ac.DetailId);*/
        }
    }
}