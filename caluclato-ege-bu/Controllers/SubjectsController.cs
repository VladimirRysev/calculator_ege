using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Calculator_ege_bu.Models.Admin.Subjects;
using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;

namespace Calculator_ege_bu.Controllers
{
    [Authorize]
    public class SubjectsController : BaseController<SubjectsController>
    {
        public SubjectsController(ApplicationDbContext dbContex, ILogger<SubjectsController> logger) : base(dbContex, logger)
        {}
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new SubjectsListViewModel();
            await model.InitAsync(_dbContext);
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new SubjectViewModel();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrUpdate(SubjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", model);
            }
            TrimStringProperties(ref model);
            if (_dbContext.Subjects.Any(x=>x.Id != model.Id && model.Name == x.Name))
            {
                TempData[ErrorMessage] = "Такой экзамен уже существует!";
                return View("Create", model);
            }
            Subject subject = null;
            if (model.Id > 0)
            {
                subject = await _dbContext.Subjects.FindAsync(model.Id);
                if (subject == null)
                {
                    return NotFound("CreateOrUbdate", model.Id);
                }
            }else
            {
                subject = new Subject();
                subject.CreateDateTime = subject.ModifiedDateTime = DateTime.Now;
                _dbContext.Subjects.Add(subject);
            }
            subject.Name = model.Name;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Error("CreateOrUbdate", ex);
            }
            TempData[SuccesMessage] = "Экзамен успешно добавлен!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var subject = await _dbContext.Subjects.FindAsync(id);
            if (subject==null)
            {
                return NotFound("Edit", id);
            }
            var model = new SubjectViewModel();
            model.Id = subject.Id;
            model.Name = subject.Name;
            return View("Create", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var subject = await _dbContext.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound("Delete", id);
            }
            var remuved = await _dbContext.SubjectScores
                .Where(x=>x.SubjectId == subject.Id)
                .ToListAsync();
            _dbContext.SubjectScores.RemoveRange(remuved);
            await _dbContext.SaveChangesAsync();
            _dbContext.Subjects.Remove(subject);
            await _dbContext.SaveChangesAsync();
            
            TempData[SuccesMessage] = "Экзамен успешно удален!";
            return RedirectToAction("Index");
        }
    }
}
