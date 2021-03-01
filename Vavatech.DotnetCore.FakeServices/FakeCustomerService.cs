using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using Vavatech.DotnetCore.IServices;
using Vavatech.DotnetCore.Models;
using Vavatech.DotnetCore.Models.SearchCriterias;

namespace Vavatech.DotnetCore.FakeServices
{
    public class FakeCustomerService : ICustomerService
    {
        private readonly ICollection<Customer> customers;

        public FakeCustomerService(Faker<Customer> faker)
        {
            customers = faker.Generate(100);
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
            return customers.SingleOrDefault(c => c.Id == id);
        }

        public IEnumerable<Customer> Get(CustomerType? customerType, bool? isRemoved)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> Get(CustomerSearchCriteria searchCriteria)
        {
            throw new NotImplementedException();
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
    }
}
