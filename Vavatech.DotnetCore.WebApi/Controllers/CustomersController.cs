using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Vavatech.DotnetCore.IServices;
using Vavatech.DotnetCore.Models;
using Vavatech.DotnetCore.Models.SearchCriterias;

namespace Vavatech.DotnetCore.WebApi.Controllers
{
   // [ApiController]
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

        // https://docs.microsoft.com/pl-pl/aspnet/core/fundamentals/routing?view=aspnetcore-5.0#route-constraint-reference
        // GET api/customers/{id}
        [HttpGet("{id:int:min(1)}", Name = "GetCustomerById")]
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

        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {
            var customer = customerService.Get(name);

            return Ok(customer);
        }

        // GET api/customers/{pesel}
        [HttpGet("{number:length(11):pesel}")]
        public IActionResult GetByPesel(string number)
        {
            var customer = customerService.GetByPesel(number);

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


        // GET api/customers?lat=53.04&lng=21.04

        //[HttpGet]
        //public IActionResult Get([Required, Range(-180, 180)] float lat, [Required] float lng)
        //{
        //    throw new NotImplementedException();
        //}


        // POST api/customers
        // content-type: application/json
        // {json}

        [HttpPost]
        public IActionResult Post([FromBody] Customer customer)
        {
            customerService.Add(customer);

            // return Created($"http://localhost:5000/api/customers/{customer.Id}", customer);

            return CreatedAtRoute("GetCustomerById", new { id = customer.Id }, customer);
        }

        // PUT api/customers/{id}
        // content-type: application/json
        // {json}
        [HttpPut("{id}")]
        public IActionResult Put(int id, Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            customerService.Update(customer);

            return NoContent();
        }


        // dotnet add package Microsoft.AspNetCore.JsonPatch

        // PATCH api/customers/{id}
        // content-type: application/merge-patch+json
        // {json}
        [HttpPatch("{id}")]
        public IActionResult Patch(int id, [FromBody] JsonPatchDocument jsonPatch)
        {
            Customer customer = customerService.Get(id);

            jsonPatch.ApplyTo(customer);

            return NoContent();
        }

        // DELETE api/customers/{id}
        
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            customerService.Remove(id);

            return NoContent();
        }


       

        
    }

    public class ErrorResponse
    {
        public int ErrorCode { get; set; }
        public string Description { get; set; }
    }
}
