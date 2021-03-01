using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vavatech.DotnetCore.WebApi.Controllers
{
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        // GET api/customers/{customerId}/orders

        [HttpGet("_api/customers/{customerId}/orders")]
        public IActionResult GetOrders(int customerId)
        {
            throw new NotImplementedException();
        }
    }
}
