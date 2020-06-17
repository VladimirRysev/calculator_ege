using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Calculator_ege_bu.Models.Admin.Directions;
using Calculator_ege_bu.Models.Admin.Divisions;
using Calculator_ege_bu.Models.Admin.Subjects;
using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;

namespace Calculator_ege_bu.Controllers
{
    [Authorize]
    public class DirectionsController : BaseController<DirectionsController>
    {
        public DirectionsController(ApplicationDbContext dbContex, ILogger<DirectionsController> logger) 
            : base(dbContex, logger)
        {

        }

        [HttpGet]
        public IActionResult Create(DirectionViewModel model)
        {
            ModelState.Clear();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async  Task<IActionResult> CreateOrUpdate(DirectionViewModel model)
        {
            EducationalDirection direction;
            if (!ModelState.IsValid)
            {
                
                return View("Create", model);
            }
            TrimStringProperties(ref model);
            if (model.Id > 0)
            {
                direction = await _dbContext.Directions
                    .Include(d=>d.EducationalDivision.University)
                    .Include(d=>d.SubjectScores)
                    .ThenInclude(s=>s.Subject)
                    .FirstOrDefaultAsync(d=>d.Id == model.Id);
                if (direction == null)
                {
                    return NotFound("CreateOrUpdate", model.Id);
                }
                direction.ModifiedDateTime = DateTime.Now;
            }else
            {
                direction = new EducationalDirection();
                direction.CreateDateTime = direction.ModifiedDateTime = DateTime.Now;
                direction.EducationalDivisionId = model.DivisionId;
                _dbContext.Directions.Add(direction);
            }
            direction.Name = model.Name;
            direction.PaidPlacesCount = model.PaidPlacesCount;
            direction.PeriodOfStudy = model.PeriodOfStudy;
            direction.Level =model.Level;
            direction.BudgetPlacesCount = model.BudgetPlacesCount;
            direction.Code = model.Code;
            direction.EducationalForm = model.Form;
            direction.Price = model.Price;
            direction.PageUrl = model.PageUrl;
            try
            {
               await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Error("CreateOrUpdate", ex);
            }

            return RedirectToAction("Edit", new { id = direction.Id});
        }

        [HttpGet]
        public async Task<IActionResult> Edit(DirectionViewModel model)
        {
            try
            {
                ModelState.Clear();
                var direction = await _dbContext.Directions
                    .Include(d=>d.EducationalDivision.University)
                    .Include(d=>d.SubjectScores)
                    .ThenInclude(s=>s.Subject)
                    .FirstOrDefaultAsync(d=>d.Id == model.Id);
                if (direction == null)
                {
                    return NotFound("Edit", model.Id);
                }
                model.Init(direction);
            }
            catch (Exception ex)
            {
                return Error("Edit", ex);
            }

            return View("Create", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var direction = await _dbContext.Directions.FindAsync(id);
            if (direction == null)
            {
                return NotFound("Delete", id);
            }
            var divisionId = direction.EducationalDivisionId;
            try
            {
                _dbContext.Directions.Remove(direction);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Error("Delete", ex);
            }
            return RedirectToAction("Edit", "Divisions", new { id = divisionId});
        }

        [HttpGet]
        public async Task<IActionResult> AddSubject(SubjectScoreViewModel model)
        {
            ModelState.Clear();
            await model.InitAsync(_dbContext);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrUpdateSubject(SubjectScoreViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("AddSubject", model);
            }
            SubjectScore subject;
            if (model.Id > 0)
            {
                subject = await _dbContext.SubjectScores.FindAsync(model.Id);
                if (subject == null)
                {
                    return NotFound("AddOrUpdateSubject");
                }
                subject.ModifiedDateTime = DateTime.Now;
            } else
            {
                subject = new SubjectScore();
                subject.ModifiedDateTime = subject.CreateDateTime = DateTime.Now;
                subject.EducationalDirectionId = model.DirectionId;
                _dbContext.Add(subject);
            }
            subject.SubjectId = model.Subject.Id;
            subject.MinimumScore = model.MinScore;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Error("AddOrUpdateSubject", ex);
            }
            return RedirectToAction("Edit", new { id = subject.EducationalDirectionId});
        }

        [HttpGet]
        public async Task<IActionResult> EditSubject(SubjectScoreViewModel model)
        {
            var subject = _dbContext.SubjectScores
                .Include(x=>x.Subject)
                .Include(x=>x.EducationalDirection)
                .FirstOrDefault(x=>x.Id == model.Id);
            if (subject == null)
            {
                return NotFound("EditSubject", model.Id);
            }
            await model.InitAsync(subject, _dbContext);
            return View("AddSubject", model);
        }
        
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            var subject = await _dbContext.SubjectScores.FindAsync(id);
            if (subject == null)
            {
                return NotFound("DeleteSubject", id);
            }
            var directionId = subject.EducationalDirectionId;
            try
            {
                _dbContext.SubjectScores.Remove(subject);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Error("DeleteSubject", ex);
            }
            return RedirectToAction("Edit", new { id = directionId});
        }
    }
}
