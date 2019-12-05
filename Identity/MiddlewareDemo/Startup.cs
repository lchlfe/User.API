using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace MiddlewareDemo
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouter(builder => builder.MapGet("action", async context =>
                {
                    await context.Response.WriteAsync("这是一个Action");
                }));

            app.Map("/task", appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    await context.Response.WriteAsync("这是一个task");
                });
            });

            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("1: before start...");
                await next.Invoke();
            });

            app.Use(next =>
            {
                return (context) =>
                {
                    context.Response.WriteAsync("2: in the middleware of start");
                    return next(context);
                };
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("3: start!");
            });
        }
    }
}
