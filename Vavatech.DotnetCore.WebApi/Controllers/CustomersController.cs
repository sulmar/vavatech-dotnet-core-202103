using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Vavatech.DotnetCore.IServices;
using Vavatech.DotnetCore.Models;
using Vavatech.DotnetCore.Models.SearchCriterias;

namespace Vavatech.DotnetCore.WebApi.Controllers
{
    
    [ApiController]
    // [Route("api/customers")]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService customerService;
        private readonly IConfiguration configuration;
        private readonly ILogger<CustomersController> logger;

        public CustomersController(
            ICustomerService customerService, 
            IConfiguration configuration, 
            ILogger<CustomersController> logger)
        {
            this.customerService = customerService;
            this.configuration = configuration;
            this.logger = logger;
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
        [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Customer> Get(int id)
        {
            var customer = customerService.Get(id);

            string openStreetMapMode = configuration["OpenStreetMap:Mode"];

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
        [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetByPesel(string number)
        {

            var customer = customerService.GetByPesel(number);

            if (customer == null)
                return NotFound();

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesDefaultResponseType]
        public ActionResult<IEnumerable<Customer>> Get([FromQuery] CustomerSearchCriteria searchCriteria)
        {

            if (!this.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            logger.LogInformation("Get Customers!");

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

        /*
         
        POST /api/customers HTTP/1.1
        Host: localhost:5000
        Content-Type: application/json
        Content-Length: 505

        {
                "firstName": "Marcin",
                "lastName": "Sulecki",
                "gender": 0,
                "email": "marcin.sulecki@domain.com",
                "birthday": "1951-09-04T11:50:49.3359767+02:00",
                "creditAmount": 875.945692483310692,
                "pesel": "51090470635",
                "type": 2,
                "description": "Laudantium cumque eum quis consequuntur similique temporibus animi.\nHarum culpa perspiciatis sint ratione blanditiis.\nNobis est qui vel reiciendis.",
                "username": "marcin"
            }

        */

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public IActionResult Post([FromBody] Customer customer, [FromServices] IMessageService messageService)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            customerService.Add(customer);

            messageService.Send($"Welcome {customer.FirstName} {customer.LastName}!");

            // return Created($"http://localhost:5000/api/customers/{customer.Id}", customer);

            return CreatedAtRoute("GetCustomerById", new { id = customer.Id }, customer);
        }

        // PUT api/customers/{id}
        // content-type: application/json
        // {json}
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
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

        /*
        PATCH /api/customers/10 HTTP/1.1
        Host: localhost:5000
        Content-Type: application/merge-patch+json
        Content-Length: 135

        [
          { "op": "replace", "path": "/firstname", "value": "Marcin" },
          { "op": "replace", "path": "/lastname", "value": "Sulecki" }
        ]

        */

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public IActionResult Patch(int id, [FromBody] JsonPatchDocument jsonPatch)
        {
            Customer customer = customerService.Get(id);

            jsonPatch.ApplyTo(customer);

            return NoContent();
        }

        // DELETE api/customers/{id}
        
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public IActionResult Delete(int id)
        {
            customerService.Remove(id);

            return NoContent();
        }

        // POST api/customers/{id}/attachments
        // Content-Type: multipart/form-data


        /*
    POST /api/customers/10/attachments HTTP/1.1
    Host: localhost:5000
    Content-Length: 211
    Content-Type: multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW

    ----WebKitFormBoundary7MA4YWxkTrZu0gW
    Content-Disposition: form-data; name="formFile"; filename="/C:/Users/marci/Downloads/vavatech_200.png"
    Content-Type: image/png

    (data)
    ----WebKitFormBoundary7MA4YWxkTrZu0gW
        */

        // Upload pojedycznego pliku

        //[HttpPost("{id}/attachments")]
        //public IActionResult Upload(int id, IFormFile formFile)
        //{
        //    string filename = Path.Combine("attachments", formFile.FileName);

        //    using(var stream = new FileStream(filename, FileMode.Create))
        //    {
        //        formFile.CopyTo(stream);
        //    }

        //    return Ok();
        //}


        // Upload wielu plików

        [HttpPost("{id}/attachments")]
        public IActionResult Upload(int id, IEnumerable<IFormFile> file)
        {
            foreach (var formFile in file)
            {
                string filename = Path.Combine("attachments", formFile.FileName);

                using (var stream = new FileStream(filename, FileMode.Create))
                {
                    formFile.CopyTo(stream);
                }
            }

            return Ok();
        }





    }

    public class ErrorResponse
    {
        public int ErrorCode { get; set; }
        public string Description { get; set; }
    }
}
