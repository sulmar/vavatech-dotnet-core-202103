using Bogus;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Validators.Polish;
using Vavatech.DotnetCore.Fakers;
using Vavatech.DotnetCore.FakeServices;
using Vavatech.DotnetCore.IServices;
using Vavatech.DotnetCore.Models;
using Vavatech.DotnetCore.Models.Validators;
using Vavatech.DotnetCore.WebApi.RouteConstraints;

namespace Vavatech.DotnetCore.WebApi
{
    // dotnet add package FluentValidation.AspNetCore

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<Faker<Customer>, CustomerFaker>();
            services.AddTransient<ICustomerService, FakeCustomerService>();

            //services.AddTransient<IValidator<Customer>, CustomerValidator>();
            //services.AddTransient<IValidator<Order>, OrderValidator>();

            services.AddTransient<PeselValidator>();

            // Newtonsoft.Json
            // dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson -Version 3.1.2
            services.AddControllers()
                .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<OrderValidator>())
                .AddXmlSerializerFormatters()
                .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                options.SerializerSettings.Converters.Add(new StringEnumConverter(camelCaseText: true));
            });


            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("pesel", typeof(PeselRouteConstraint));
            });

            services.Configure<FakeCustomerServiceOptions>(Configuration.GetSection("CustomerOptions"));

            // services.Configure<FakeCustomerServiceOptions>(options => options.Count = 50);

            // System.Text.Json serialization
            //services.AddControllers()
            //    .AddJsonOptions(options =>
            //    {
            //        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            //    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            string googleMapsUrl = Configuration["GoogleMapsUrl"];

            string openStreetMapMode = Configuration["OpenStreetMap:Mode"];


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
