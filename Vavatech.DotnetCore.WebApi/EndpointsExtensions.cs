using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace Vavatech.DotnetCore.WebApi
{
    public static class EndpointsExtensions
    {
        public static IEndpointRouteBuilder MapRedirect(this IEndpointRouteBuilder endpoints, string pattern, string location)
        {
            endpoints.MapGet(pattern, context =>
            {
                context.Response.Redirect(location);
                return Task.CompletedTask;
            });

            return endpoints;

        }
    }
}
