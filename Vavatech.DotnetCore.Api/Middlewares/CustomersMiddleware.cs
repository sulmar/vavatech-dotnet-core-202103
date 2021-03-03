using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Vavatech.DotnetCore.IServices;

namespace Vavatech.DotnetCore.Api.Middlewares
{
    public static class CustomersMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustom(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomersMiddleware>();

            return app;
        }
    }

    public class CustomersMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ICustomerService customerService;

        public CustomersMiddleware(RequestDelegate next, ICustomerService customerService)
        {
            this.next = next;
            this.customerService = customerService;
        }

        public CustomersMiddleware()
        {

        }

        public async Task InvokeAsync(HttpContext context)
        {            
            if (context.Request.Path.HasValue && context.Request.Path == "/api/customers" && context.Request.Method == HttpMethods.Get)
            {
                var customers = customerService.Get();

                string json = JsonConvert.SerializeObject(customers);

                context.Response.StatusCode = StatusCodes.Status200OK;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
            }
            else
                await next(context);
        }
    }

}
