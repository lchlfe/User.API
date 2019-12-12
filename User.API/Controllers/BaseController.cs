using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using User.API.Dtos;

namespace User.API.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// todo: 从token中获取当前请求用户的userid
        /// </summary>
        protected UserIdentity UserIdentity
        {
            get
            {
                var user = new UserIdentity();
                user.Avatar = "aa";
                user.Company = "bb";
                user.Name = "cc";
                user.Phone = "18874210960";
                user.Title = "123";
                user.UserId = 1;
                #region 先注释
                //user.Avatar = User.Claims.First(x => x.Type == "avatar").Value;
                //user.Company = User.Claims.First(x => x.Type == "company").Value;
                //user.Name = User.Claims.First(x => x.Type == "name").Value;
                //user.Phone = User.Claims.First(x => x.Type == "phone").Value;
                //user.Title = User.Claims.First(x => x.Type == "title").Value;
                //user.UserId = int.Parse(User.Claims.First(x => x.Type == "sub").Value); 
                #endregion

                return user;
            }
        }
    }
}
