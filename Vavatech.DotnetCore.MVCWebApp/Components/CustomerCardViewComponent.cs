using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vavatech.DotnetCore.Models;

namespace Vavatech.DotnetCore.MVCWebApp.Components
{
    public class CustomerCardViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Customer customer)
        {
            return View(customer);
        }
    }
}
