﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace MvcClient
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";//使用Cookies认证
                    options.DefaultChallengeScheme = "oidc";//使用oidc
                })
                .AddCookie("Cookies")//配置Cookies认证
                .AddOpenIdConnect("oidc", options =>//配置oidc
                {
                    options.SignInScheme = "Cookies";
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
                    options.ClientId = "mvc";
                    options.ClientSecret = "secret";
                    options.SaveTokens = true;
                    //options.GetClaimsFromUserInfoEndpoint = true;

                    //options.ClaimActions.MapJsonKey("sub", "sub");
                    //options.ClaimActions.MapJsonKey("preferred_username", "preferred_username");
                    //options.ClaimActions.MapJsonKey("sub", "sub");
                    //options.ClaimActions.MapJsonKey("avatar", "avatar");
                    //options.ClaimActions.MapCustomJson("role", jobj => jobj["role"].ToString());

                    options.Scope.Add("offline_access");
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
