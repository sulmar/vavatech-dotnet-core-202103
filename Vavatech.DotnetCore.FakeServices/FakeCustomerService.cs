using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using Vavatech.DotnetCore.IServices;
using Vavatech.DotnetCore.Models;

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
