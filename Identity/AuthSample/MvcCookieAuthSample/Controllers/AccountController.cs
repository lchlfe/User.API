using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Services;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcCookieAuthSample.Models;
using MvcCookieAuthSample.ViewModels;

namespace MvcCookieAuthSample.Controllers
{
    public class AccountController : Controller
    {
        //private TestUserStore _users;

        //public AccountController(TestUserStore users)
        //{
        //    _users = users;
        //}

        private readonly UserManager<ApplicationUser> _userManager;//创建用户的
        private readonly SignInManager<ApplicationUser> _signInManager;//用来登录的
        private readonly IIdentityServerInteractionService _interaction;
        //依赖注入
        public AccountController(UserManager<ApplicationUser> userManager
            , SignInManager<ApplicationUser> signInManager
            , IIdentityServerInteractionService interaction)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
        }

        public IActionResult Register(string returnUrl = null)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel, string returnUrl = null)
        {
            var identityUser = new ApplicationUser
            {
                Email = registerViewModel.Email,
                UserName = registerViewModel.Email,
                NormalizedUserName = registerViewModel.Email
            };
            var identityResult = await _userManager.CreateAsync(identityUser, registerViewModel.Password);
            if (identityResult.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult Login(string returnUrl = null)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                ViewData["returnUrl"] = returnUrl;
                var user =await _userManager.FindByEmailAsync(loginViewModel.Email);
                if (user==null)
                {
                    ModelState.AddModelError(nameof(loginViewModel.Email),"UserName not exist");
                }
                else
                {
                    if (await _userManager.CheckPasswordAsync(user,loginViewModel.Password))
                    {
                        AuthenticationProperties prop = null;
                        if (loginViewModel.RememberMe)
                        {
                            prop = new AuthenticationProperties()
                            {
                                IsPersistent = true,
                                ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromMinutes(30))
                            };
                        }

                        //await Microsoft.AspNetCore.Http.AuthenticationManagerExtensions.SignInAsync(HttpContext,
                        //    user.SubjectId, user.Username,prop);
                        //return RedirectToLocal(returnUrl);

                        await _signInManager.SignInAsync(user, prop);
                        if (_interaction.IsValidReturnUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }

                        return Redirect("~/");
                    }
                    ModelState.AddModelError(nameof(loginViewModel.Password),"Wrong Password");
                }

            }
            return View(loginViewModel);
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            //await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        //内部跳转
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        //添加验证错误
        private void AddError(IdentityResult result)
        {
            //遍历所有的验证错误
            foreach (var error in result.Errors)
            {
                //返回error到model
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}