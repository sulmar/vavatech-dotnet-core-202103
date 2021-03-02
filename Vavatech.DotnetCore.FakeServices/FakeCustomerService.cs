using Bogus;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using Vavatech.DotnetCore.IServices;
using Vavatech.DotnetCore.Models;
using Vavatech.DotnetCore.Models.SearchCriterias;

namespace Vavatech.DotnetCore.FakeServices
{
    public class FakeCustomerServiceOptions
    {
        public int Count { get; set; } = 99;
        public string Url { get; set; } 
    }

    public class FakeCustomerService : ICustomerService
    {
        private readonly ICollection<Customer> customers;

        // dotnet add package Microsoft.Extensions.Options
        public FakeCustomerService(Faker<Customer> faker, IOptionsSnapshot<FakeCustomerServiceOptions> options)
        {            
            customers = faker.Generate(options.Value.Count);
        }

        public void Add(Customer entity)
        {
            int lastId = customers.Max(c => c.Id);

            entity.Id = ++lastId;

            customers.Add(entity);
        }

        public IEnumerable<Customer> Get()
        {
            return customers;
        }

        public Customer Get(int id)
        {
            Customer customer =  customers.SingleOrDefault(c => c.Id == id);

            customer.Partner = customers.First();
            customer.Partner.Partner = customer;

            return customer;
        }


        public IEnumerable<Customer> Get(CustomerType? customerType, bool? isRemoved)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> Get(CustomerSearchCriteria searchCriteria)
        {
            IQueryable<Customer> query = UseFilter(searchCriteria);

            return query.ToList();
        }

        private IQueryable<Customer> UseFilter(CustomerSearchCriteria searchCriteria)
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

        public void Remove(int id)
        {
            customers.Remove(Get(id));
        }

        public void Update(Customer entity)
        {
            Remove(entity.Id);
            customers.Add(entity);
        }

        public Customer Get(string name)
        {
            return customers.FirstOrDefault(c => c.LastName == name);
        }

        public Customer GetByPesel(string pesel)
        {
            return customers.SingleOrDefault(c => c.Pesel == pesel);
        }
    }
}
