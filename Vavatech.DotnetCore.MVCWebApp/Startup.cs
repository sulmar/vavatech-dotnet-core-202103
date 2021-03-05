using Bogus;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Validators.Polish;
using Vavatech.DotnetCore.DbServices;
using Vavatech.DotnetCore.Fakers;
using Vavatech.DotnetCore.FakeServices;
using Vavatech.DotnetCore.IServices;
using Vavatech.DotnetCore.Models;
using Vavatech.DotnetCore.Models.Validators;

namespace Vavatech.DotnetCore.MVCWebApp
{
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
            //services.AddSingleton<Faker<Customer>, CustomerFaker>();
            //services.AddSingleton<ICustomerService, FakeCustomerService>();

            string connectionString = Configuration.GetConnectionString("StoreConnectionString");

            // dotnet add package Microsoft.EntityFrameworkCore.SqlServer
            services.AddDbContext<StoreContext>(options => options.UseSqlServer(connectionString));
            // services.AddDbContextPool<StoreContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<ICustomerService, DbCustomerService>();

            services.AddTransient<PeselValidator>();

            services.AddControllersWithViews()
                    .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<OrderValidator>());

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IUserResolverService, UserResolverService>();


            services.AddAuthentication(IISDefaults.AuthenticationScheme);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, StoreContext context)
        {
            // context.Database.EnsureCreated();

            context.Database.Migrate();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{ide?}");
            });
        }
    }
}
