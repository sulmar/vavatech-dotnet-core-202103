using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vavatech.DotnetCore.IServices;
using Vavatech.DotnetCore.Models;

namespace Vavatech.DotnetCore.WebApi.Identity
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ICustomerService customerService;

        public AuthenticationService(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        public bool TryAuthorize(string login, string password, out Customer customer)
        {
            customer = customerService.Get(login, password);

            return customer != null;
        }
    }
}
