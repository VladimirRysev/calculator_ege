using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Calculator_ege_bu.Models;
using Calculator_ege_bu.Models.Admin.Divisions;
using Calculator_ege_bu.Models.Admin.Subjects;
using Calculator_ege_bu.Services;
using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Calculator_ege_bu.Controllers
{
    [Authorize]
    public class DivisionsController : BaseController<DivisionsController>
    {
        public DivisionsController(ApplicationDbContext dbContex, ILogger<DivisionsController> logger):base(dbContex, logger)
        {
        }
        
        [HttpGet]
        public async Task<IActionResult> Index(DivisionsListViewModel model)
        {
            await model.InitAsync(_dbContext);
            return View(model);
        }

        [HttpGet]
        public IActionResult Create(DivisionViewModel model)
        {
            ModelState.Clear();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrUpdate(DivisionViewModel model)
        {
            EducationalDivision division;
            if (!ModelState.IsValid)
            {
                return View("Create", model);
            }
            TrimStringProperties(ref model);
            if (model.Id > 0)
            {
                if (!ModelState.IsValid)
                {
                    return View("Edit", model);
                }
                division = await _dbContext.Divisions.FindAsync(model.Id);
                if (division == null)
                {
                    return NotFound("CreateOrUpdate", model.Id);
                }
                division.ModifiedDateTime = DateTime.Now;
            } else
            {
                division = new EducationalDivision();
                division.UniversityId = model.UniversityId;
                division.CreateDateTime = division.ModifiedDateTime = DateTime.Now;
                division.PageUrl = model.PageUrl;
                _dbContext.Add(division);
            }
            division.Name = model.Name;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Error("CreateOrUpdate", ex);
                
            }
            return RedirectToAction("Edit", "University", new { id = division.UniversityId, DivisionsPage = model.Page });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(DivisionViewModel model)
        {
            var division = await _dbContext.Divisions
                .Include(d=>d.University)
                .FirstOrDefaultAsync(d=>d.Id == model.Id);
            if (division == null)
            {
                return NotFound("Edit", model.Id);
            }
            try
            {
                await model.Init(division, _dbContext);
            }
            catch (Exception ex)
            {
                return Error("CreateOrUpdate", ex);
            }
            ModelState.Clear();
            return View("Create", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var division = await _dbContext.Divisions
                .Include(d=>d.EducationalDirections)
                .FirstOrDefaultAsync(d=>d.Id == id);
            if (division == null)
            {
                return NotFound("Delete", id);
            }
            var universityId = division.UniversityId;
            try
            {
                _dbContext.Divisions.Remove(division);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Error("Delete", ex);
            }
            return RedirectToAction("Edit", "University", new { id = universityId});
        }
    }
}
