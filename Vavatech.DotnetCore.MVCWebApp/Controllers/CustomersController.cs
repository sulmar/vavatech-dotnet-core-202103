using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vavatech.DotnetCore.IServices;
using Vavatech.DotnetCore.Models;

namespace Vavatech.DotnetCore.MVCWebApp.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ICustomerService customerService;

        public CustomersController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        public IActionResult Index()
        {
            var customers = customerService.Get();

            return View(customers);
        }

        public IActionResult Edit(int ide)
        {
            var customer = customerService.Get(ide);

            return View(customer);
        }

        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
            if (!ModelState.IsValid)
            {                
                return View(customer);
            }
            else
            {
                customerService.Update(customer);
                return RedirectToAction("Index");
            }

            
        }

    }
}
