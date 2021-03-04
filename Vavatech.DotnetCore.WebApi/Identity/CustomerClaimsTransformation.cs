using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Vavatech.DotnetCore.WebApi.Identity
{

    public class CustomerClaimsTransformation : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            // Kopia bieżącej tożsamości
            ClaimsPrincipal clone = principal.Clone();
            ClaimsIdentity identity = (ClaimsIdentity) clone.Identity;

            identity.AddClaim(new Claim("kat", "B"));
            identity.AddClaim(new Claim("kat", "C"));

            identity.AddClaim(new Claim(ClaimTypes.Role, "Developer"));
            identity.AddClaim(new Claim(ClaimTypes.Role, "Trainer"));


            return Task.FromResult(clone);
        }
    }
}
