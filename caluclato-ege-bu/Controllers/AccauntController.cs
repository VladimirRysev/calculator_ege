using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Calculator_ege_bu.Extentions;
using Calculator_ege_bu.Models.Accaunt;
using DataAccessLayer;
using DataAccessLayer.Models;
using DataAccessLayer.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Calculator_ege_bu.Controllers
{
    [Authorize(Roles = "SuperAdministrator")]
    public class AccauntController : BaseController<AccauntController>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccauntController(ApplicationDbContext dbContex, 
            ILogger<AccauntController> logger,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
            : base(dbContex, logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        ApplicationUser Current 
        { 
            get
            {
                var id = int.Parse(_userManager.GetUserId(User));
                return _dbContext.Users.Find(id);
            }
        }

        public async Task<IActionResult> Index(UsersListViewModel model)
        {
            await model.Init(_dbContext, Current);

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateUserViewModel();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", model);
            }
            var userInDb = await _userManager.FindByNameAsync(model.Name);
            if (userInDb != null)
            {
                TempData[ErrorMessage] = "Пользователь с таким именем уже существует!";
                return View("Create", model);
            }
            var user = new ApplicationUser{UserName = model.Name};
            var result = await _userManager.CreateAsync(user, model.Password);
            await _userManager.AddToRoleAsync(user,  model.Role.ToString());

            if (result.Succeeded)
            {
                TempData[SuccesMessage] = "Пользователь успешно создан!";
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View("Create", model);

            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound("Edit", id);
            }
            var model = new EditUserViewModel();
            model.Id = user.Id;
            model.Name = user.UserName.Trim();

            model.Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault().GetEnumFromString<RoleEnum>();
            return View("Edit", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }

            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user == null)
            {
                return NotFound("Edit", model.Id);
            }
            var user1 = await _userManager.FindByNameAsync(model.Name);
            if ( user1 != null && user.Id != user1.Id)
            {
                TempData[ErrorMessage] = "Пользователь с таким именем уже существует!";
                return View("Edit", model);
            }
            user.UserName = model.Name;

            var result = await _userManager.UpdateAsync(user);
            await _userManager.RemoveFromRolesAsync(user, new List<string>(){ RoleEnum.Administrator.ToString(), RoleEnum.SuperAdministrator.ToString()});
            await _userManager.AddToRoleAsync(user,  model.Role.ToString());
            if (!string.IsNullOrEmpty(model.Password) && result.Succeeded)
            {
                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Пароли не совпадают");
                    return View("Edit", model);
                }
                
                result =  await _userManager.RemovePasswordAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddPasswordAsync(user, model.Password);
                }
            }
            if (result.Succeeded)
            {
                TempData[SuccesMessage] = "Пользователь успешно изменен!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData[ErrorMessage] = "Произошла ошибка!";
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View("Edit", model);
            }
        }

        [HttpPost, AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
             UsersListViewModel listModel = new UsersListViewModel();
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound("Delete", id);
            }
            if (id==1)
            {
               
                await listModel.Init(_dbContext, Current);
                TempData[ErrorMessage] = "Этого пользователя нельзя удалить";
                return View("Index", listModel);
            }
            await _userManager.RemoveFromRolesAsync(user, new List<string>(){ RoleEnum.Administrator.ToString(), RoleEnum.SuperAdministrator.ToString()});
            
            await _userManager.DeleteAsync(user);

            TempData[SuccesMessage] = "Пользователь успешно удален!";
            return RedirectToAction("Index");
        }


    }
}
