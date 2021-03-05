using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Vavatech.DotnetCore.IServices;
using Vavatech.DotnetCore.Models;
using Vavatech.DotnetCore.Models.SearchCriterias;

namespace Vavatech.DotnetCore.DbServices
{
    public class DbCustomerService : ICustomerService
    {
        private readonly StoreContext context;

        private DbSet<Customer> customers => context.Customers;

        public DbCustomerService(StoreContext context)
        {
            this.context = context;
        }

        public void Add(Customer entity)
        {
            context.Customers.Add(entity);
            context.SaveChanges();
        }

        public Customer Get(string name)
        {
            return customers.SingleOrDefault(c => c.LastName == name);
        }

        public Customer Get(string username, string password)
        {
            return customers.SingleOrDefault(c => c.Username == username && c.HashedPassword == password);
        }

        public IEnumerable<Customer> Get()
        {
            return customers.ToList();
        }

        public Customer Get(int id)
        {
            return customers.Find(id);
        }

        public IEnumerable<Customer> Get(CustomerSearchCriteria searchCriteria)
        {
            IQueryable<Customer> query = customers.AsQueryable();

            if (searchCriteria.CustomerType.HasValue)
            {
                query = query.Where(c => c.Type == searchCriteria.CustomerType);
            }

            if (searchCriteria.IsRemoved.HasValue)
            {
                query = query.Where(c => c.IsRemoved == searchCriteria.IsRemoved);
            }

            if (searchCriteria.From.HasValue)
            {
                query = query.Where(c => c.Birthday >= searchCriteria.From);
            }

            if (searchCriteria.To.HasValue)
            {
                query = query.Where(c => c.Birthday <= searchCriteria.To);
            }

            return query;
        }

        public IEnumerable<Customer> GetByGender(Gender gender)
        {
            return customers.Where(c => c.Gender == gender);
        }

        public Customer GetByPesel(string pesel)
        {
            return customers.SingleOrDefault(c => c.Pesel == pesel);
        }

        public void Remove(int id)
        {
            customers.Remove(Get(id));
            context.SaveChanges();
        }

        public void Update(Customer entity)
        {
            customers.Update(entity);
            context.SaveChanges();
        }
    }
}
