using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Vavatech.DotnetCore.DbServices.Configurations;
using Vavatech.DotnetCore.Models;

namespace Vavatech.DotnetCore.DbServices
{
    public class StoreContext : DbContext
    {
        public StoreContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fluent API
            //modelBuilder.Entity<Customer>().Property(p => p.FirstName).HasMaxLength(50);
            //modelBuilder.Entity<Customer>().Property(p => p.LastName).HasMaxLength(50).IsRequired();
            //modelBuilder.Entity<Customer>().Property(p => p.Pesel).HasMaxLength(11).IsRequired().IsUnicode(false);

            // modelBuilder.Entity<Product>().Property(p => p.Name).HasMaxLength(100).IsRequired();

            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());

            base.OnModelCreating(modelBuilder);
        }

    }
}
