﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MvcCookieAuthSample.Data;

namespace MvcCookieAuthSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build()
                //自动初始化数据库开始
                .MigrateDbContext<ApplicationDbContext>((context, services) =>
                    {
                        new ApplicationDbContextSeed().SeedAsync(context, services).Wait();
                    })
                //自动初始化数据库结束
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://*:5000")
                .UseStartup<Startup>();
    }
}
