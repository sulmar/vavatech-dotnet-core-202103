using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Vavatech.DotnetCore.Api.Middlewares
{
    public static class LoggerMiddlewareExtensions
    {
        public static IApplicationBuilder UseLogger(this IApplicationBuilder app)
        {
            app.UseMiddleware<LoggerMiddleware>();

            return app;
        }
    }

    public class LoggerMiddleware
    {
        private readonly RequestDelegate next;

        public LoggerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Stopwatch stopwatch = new Stopwatch();
            
            Trace.WriteLine($"{context.Request.Method} {context.Request.Path}");

            stopwatch.Start();

            await next(context);

            stopwatch.Stop();

            Trace.WriteLine($"{context.Response.StatusCode} Elapsed {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
