using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MvcCookieAuthSample.Models;
using MvcCookieAuthSample.Models.MvcCookieAuthSample.Models;

namespace MvcCookieAuthSample.Data
{
    public class ApplicationDbContextSeed
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<ApplicationUserRole> _roleManager;

        public async Task SeedAsync(ApplicationDbContext context, IServiceProvider services)
        {
            if (!context.Roles.Any())
            {
                var role = new ApplicationUserRole()
                {
                    Name = "Administrators",
                    NormalizedName = "Administrators"
                };
                _roleManager = services.GetRequiredService<RoleManager<ApplicationUserRole>>();
                var result = await _roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    throw new Exception("初始默认角色失败:"+result.Errors.SelectMany(e=>e.Description));
                }
            }

            if (!context.Users.Any())
            {
                _userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

                var defaultUser = new ApplicationUser
                {
                    UserName = "Administrator",
                    Email = "786744873@qq.com",
                    NormalizedUserName = "admin",
                    SecurityStamp = "admin",
                    Avatar = "https://img-prod-cms-rt-microsoft-com.akamaized.net/cms/api/am/imageFileData/RE1Mu3b?ver=5c31"
                };


                var result = await _userManager.CreateAsync(defaultUser, "123456");
                await _userManager.AddToRoleAsync(defaultUser, "Administrators");



                if (!result.Succeeded)
                {
                    throw new Exception("初始默认用户失败:" + result.Errors.SelectMany(e => e.Description));
                }
            }
        }
    }
}
