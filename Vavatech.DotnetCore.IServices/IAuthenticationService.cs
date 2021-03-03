using System;
using System.Collections.Generic;
using System.Text;
using Vavatech.DotnetCore.Models;

namespace Vavatech.DotnetCore.IServices
{
    public interface IAuthenticationService
    {
        bool TryAuthorize(string login, string password, out Customer customer);        
    }
}
