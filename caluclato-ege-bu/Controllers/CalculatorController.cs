using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Calculator_ege_bu.Models;
using Calculator_ege_bu.Models.Accaunt;
using Calculator_ege_bu.Models.Calculator;
using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Logging;

namespace Calculator_ege_bu.Controllers
{
    [AllowAnonymous]
    public class CalculatorController : BaseController<CalculatorController>
    {
        public CalculatorController(ApplicationDbContext dbContex, ILogger<CalculatorController> logger) : base(dbContex, logger)
        {}
        [ResponseCache(Location =ResponseCacheLocation.Any, Duration =300)]
        public async Task<IActionResult> Index(CalculatorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await model.InitAsync(_dbContext, ModelState.IsValid);
                return View(model);
            }
            await model.InitAsync(_dbContext);
            return View(model);
        }
        
        
        [HttpGet]
        public IActionResult Login(string returnUrl=null)
        {
            var model = new LoginViewModel();
            model.ReturnUrl = returnUrl;
            return View(model);
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, 
            [FromServices] UserManager<ApplicationUser> userManager,
            [FromServices] SignInManager<ApplicationUser> signInManager)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Name);
                if (user == null)
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                    return View(model);
                }
                var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "University");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout([FromServices] SignInManager<ApplicationUser> signInManager)
        {
            // удаляем аутентификационные куки
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Calculator");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode = null, string message = "")
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Message = message });
        }
    }
}
