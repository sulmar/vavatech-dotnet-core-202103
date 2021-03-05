using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Vavatech.DotnetCore.DbServices.Configurations;
using Vavatech.DotnetCore.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Vavatech.DotnetCore.DbServices
{
    public interface IUserResolverService
    {
        string GetUser();
    }

    public class UserResolverService : IUserResolverService
    {
        private readonly IHttpContextAccessor _context;
        public UserResolverService(IHttpContextAccessor context)
        {
            _context = context;
        }

        public string GetUser()
        {
            return _context.HttpContext.User?.Identity?.Name;
        }
    }

    public class StoreContext : DbContext
    {
        private readonly IUserResolverService userResolverService;

        public StoreContext([NotNullAttribute] DbContextOptions options, IUserResolverService userResolverService) : base(options)
        {
            this.userResolverService = userResolverService;
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

        public override int SaveChanges()
        {
            var entities = ChangeTracker.Entries<BaseEntity>();

            var addedEntries = entities.Where(e => e.State == EntityState.Added);
            var modifiedEntries = entities.Where(e => e.State == EntityState.Modified);            

            foreach (var entry in addedEntries)
            {
                entry.Entity.CreatedDate = DateTime.Now;
            }

            foreach (var entry in modifiedEntries)
            {
                entry.Entity.ModifiedDate = DateTime.Now;
                entry.Entity.ModifiedBy = userResolverService.GetUser();
            }

            var orderEntities = ChangeTracker.Entries<Order>();

            //foreach (var order in orderEntities)
            //{
            //    order.Entity.Customer
            //}


            return base.SaveChanges();
        }

    }
}
