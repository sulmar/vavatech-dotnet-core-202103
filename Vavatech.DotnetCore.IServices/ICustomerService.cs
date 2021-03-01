using System.Collections.Generic;
using Vavatech.DotnetCore.Models;
using Vavatech.DotnetCore.Models.SearchCriterias;

namespace Vavatech.DotnetCore.IServices
{


    public interface ICustomerService : IEntityService<Customer, CustomerSearchCriteria>
    {
        IEnumerable<Customer> GetByGender(Gender gender);

        // IEnumerable<Customer> Get(CustomerType? customerType, bool? isRemoved);

        // IEnumerable<Customer> Get(CustomerSearchCriteria searchCriteria);
       
    }
}
