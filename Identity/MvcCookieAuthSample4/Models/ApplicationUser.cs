using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MvcCookieAuthSample4.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string Avatar { get; set; }//头像
    }
}
