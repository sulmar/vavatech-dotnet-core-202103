using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vavatech.DotnetCore.IServices;
using Vavatech.DotnetCore.Models;

namespace Vavatech.DotnetCore.BlazorServerClient.Pages
{
    public partial class Customers : ComponentBase
    {
        [Inject]
        protected ICustomerService CustomerService { get; set; }

        protected IEnumerable<Customer> customers;

        protected override void OnInitialized()
        {
            customers = CustomerService.Get();
        }
    }
}
