using Bogus;
using FluentValidation;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
using Validators.Polish;
using Vavatech.DotnetCore.Fakers;
using Vavatech.DotnetCore.FakeServices;
using Vavatech.DotnetCore.IServices;
using Vavatech.DotnetCore.Models;
using Vavatech.DotnetCore.Models.Validators;
using Vavatech.DotnetCore.NBPExchangeRateService;
using Vavatech.DotnetCore.WebApi.AuthenticationHandlers;
using Vavatech.DotnetCore.WebApi.HealthChecks;
using Vavatech.DotnetCore.WebApi.Identity;
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
            services.AddSingleton<Faker<Customer>, CustomerFaker>();
            services.AddSingleton<ICustomerService, FakeCustomerService>();
            services.AddTransient<IMessageService, FakeSmsMessageService>();

            //services.AddTransient<IValidator<Customer>, CustomerValidator>();
            //services.AddTransient<IValidator<Order>, OrderValidator>();

            services.AddTransient<PeselValidator>();

            // Newtonsoft.Json
            // dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson -Version 3.1.2
            services.AddControllers()
                .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<OrderValidator>())
            //    .AddXmlSerializerFormatters()
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



            // dotnet add package NSwag.AspNetCore
            services.AddOpenApiDocument(options =>
            {
                options.Title = "NET Core API";
                options.DocumentName = "Api ze szkolenia";
                options.Version = "v1";
                options.Description = "Lorem ipsum";
            });

            // Nazwani klienci (Named Clients)
            services.AddHttpClient("nbpapi", config => 
            {
                config.BaseAddress = new Uri("http://api.nbp.pl/");
                config.DefaultRequestHeaders.Add("Accept", "application/json");
            });


            services.AddTransient<IExchangeRateService, NBPApiExchangeRateService>();

            services.AddHealthChecks()
                .AddCheck<RandomHealthCheck>("random")
                .AddCheck<NBPApiHealthCheck>("nbp-api");

            // dotnet add package AspNetCore.HealthChecks.UI
            // dotnet add package AspNetCore.HealthChecks.UI.Client
            // dotnet add package AspNetCore.HealthChecks.UI.InMemory.Storage

            services.AddHealthChecksUI(options =>
            {
                options.AddHealthCheckEndpoint("My API", "http://localhost:5000/health");
                options.SetEvaluationTimeInSeconds(60);
            })
                .AddInMemoryStorage();


            services.AddAuthentication("Basic")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);

            services.AddTransient<IServices.IAuthenticationService, Identity.AuthenticationService>();
            services.AddScoped<IClaimsTransformation, CustomerClaimsTransformation>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            string googleMapsUrl = Configuration["GoogleMapsUrl"];

            string googleMapSecretKey = Configuration["GoogleMapsSecretKey"];

            if (env.EnvironmentName == "TestowoProdukcyjne")
            {

            }

            if (env.IsEnvironment("TestowoProdukcyjne"))
            {
                string openStreetMapMode = Configuration["OpenStreetMap:Mode"];
            }


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseOpenApi();

                // /swagger
                app.UseSwaggerUi3();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();  // uwaga na kolejnoœæ!
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapGet("/", context =>
                //{
                //    context.Response.Redirect("/swagger");
                //    return Task.CompletedTask;
                //});

                endpoints.MapRedirect("/", "/swagger");

                // dotnet add package AspNetCore.HealthChecks.UI.Client
                

                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                { 
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                // https://localhost:5001/healthchecks-ui
                endpoints.MapHealthChecksUI();

                endpoints.MapControllers();
            });
        }
    }
}
