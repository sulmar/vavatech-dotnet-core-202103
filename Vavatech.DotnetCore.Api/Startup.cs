using Bogus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Vavatech.DotnetCore.Api.Middlewares;
using Vavatech.DotnetCore.Fakers;
using Vavatech.DotnetCore.FakeServices;
using Vavatech.DotnetCore.IServices;
using Vavatech.DotnetCore.Models;

namespace Vavatech.DotnetCore.Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Faker<Customer>, CustomerFaker>();
            services.AddSingleton<ICustomerService, FakeCustomerService>();
            

            services.Configure<FakeCustomerServiceOptions>(options => options.Count = 100);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Logger
            //app.Use(async (context, next) =>
            //{
            //    Stopwatch stopwatch = new Stopwatch();

            //    Trace.WriteLine($"{context.Request.Method} {context.Request.Path}");

            //    stopwatch.Start();

            //    await next();

            //    stopwatch.Stop();

            //    Trace.WriteLine($"{context.Response.StatusCode} Elapsed {stopwatch.ElapsedMilliseconds} ms");
            //});

            // Autentykacja
            //app.Use(async (context, next) =>
            //{
            //    if (context.Request.Headers.ContainsKey("Authorization"))
            //    {
            //        await next();
            //    }
            //    else
            //    {
            //        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            //    }

            //});

            // GET api/customers
            //app.Use(async (context, next) =>
            //{
            //    if (context.Request.Path.ToString() == "/api/customers")
            //    {
            //        await context.Response.WriteAsync("Return customers");
            //    }
            //    else
            //        await next();
            //});

            // app.UseMiddleware<LoggerMiddleware>();
            // app.UseLogger();

            // app.UseMiddleware<AuthorizationMiddleware>();
            // app.UseMiddleware<CustomersMiddleware>();

            // app.UseCustom();

            // app.Run(context => context.Response.WriteAsync("Hello World!"));

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.Map("/", async context => await context.Response.WriteAsync("Hello World!"));

                endpoints.Map("/api/customers", 
                    endpoints.CreateApplicationBuilder()
                        .UseMiddleware<CustomersMiddleware>()
                        .Build());

                endpoints.MapGet("/api/customers/{id:int}", async context =>
                {
                    int id =  Convert.ToInt32(context.Request.RouteValues["id"]);

                    ICustomerService customerService = context.RequestServices.GetRequiredService<ICustomerService>();

                    var customer = customerService.Get(id);
                    string json = JsonConvert.SerializeObject(customer);

                    context.Response.StatusCode = StatusCodes.Status200OK;
                    context.Response.Headers.Add("Content-Type", "application/json");
                    await context.Response.WriteAsync(json);
                });

                
            });

        }
    }
}
