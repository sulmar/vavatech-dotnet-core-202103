using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using Vavatech.DotnetCore.Models;

namespace Vavatech.DotnetCore.WebApi.Identity
{
    public class SameAuthorRequirement : IAuthorizationRequirement
    {

    }

    public class OrderAuthorizationHandler : AuthorizationHandler<SameAuthorRequirement, Order>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SameAuthorRequirement requirement, Order resource)
        {
            string email = context.User.FindFirst(c => c.Type == ClaimTypes.Email).Value;

            if (email == resource.Customer.Email)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
