using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vavatech.DotnetCore.IServices;
using Vavatech.DotnetCore.Models;
using Vavatech.DotnetCore.Models.SearchCriterias;

namespace Vavatech.DotnetCore.WebApi.Controllers
{
    // [Route("api/customers")]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService customerService;

        public CustomersController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        // GET api/customers        
        //[HttpGet]
        //public IEnumerable<Customer> Get()
        //{
        //    var customers = customerService.Get();

        //    return customers;
        //}

        // GET api/customers/female
        [HttpGet("female")]
        public IEnumerable<Customer> GetFemale()
        {
            var customers = customerService.GetByGender(Gender.Female);

            return customers;
        }

        // GET api/customers/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var customer = customerService.Get(id);

            if (customer == null)
                return NotFound();

            if (customer.IsRemoved)
            {
                // return BadRequest("Klient został usunięty");

                ErrorResponse response = new ErrorResponse { Description = "Klient został usunięty", ErrorCode = 2000 };
                return BadRequest(response);
            }

            return Ok(customer);
        }


        //// GET api/customers?CustomerType=Smiling&IsRemoved=0   // query string ? & & &
        //[HttpGet]
        //public IActionResult Get(CustomerType? customerType, bool? isRemoved)
        //{
        //    throw new NotImplementedException();
        //}


        // GET api/customers?CustomerType=Smiling&From=
        [HttpGet]
        public IActionResult Get(CustomerSearchCriteria searchCriteria)
        {
            var customers = customerService.Get(searchCriteria);

            return Ok(customers);
        }

        // GET api/customers/10/orders

    }

    public class ErrorResponse
    {
        public int ErrorCode { get; set; }
        public string Description { get; set; }
    }
}
