using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vavatech.DotnetCore.Api.Middlewares
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                await next(context);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
        }
    }

}
