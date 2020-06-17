using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Calculator_ege_bu.Constants;
using Calculator_ege_bu.Models;
using Calculator_ege_bu.Models.Admin.Universities;
using Calculator_ege_bu.Services;
using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NLog;

namespace Calculator_ege_bu.Controllers
{
    [Authorize]
    public class UniversityController : BaseController<UniversityController>
    {
        private readonly IFileService _fileService;

        public UniversityController(ApplicationDbContext dbContex, IFileService fileService, ILogger<UniversityController> logger)
            : base(dbContex, logger)
        {
            _fileService = fileService;
        }

        public async Task<IActionResult> Index(UniversitiesListViewModel model)
        {
            await model.InitAsync(_dbContext);

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateUniversityViewModel();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUniversityViewModel model)
        {
            University university;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            TrimStringProperties(ref model);
            if (model.Id != 0)
            {
                university = await _dbContext.Universities.FindAsync(model.Id);
                if (university == null)
                {
                    return NotFound("Create", model.Id);
                }
                university.ModifiedDateTime = DateTime.Now;
            } else
            {
                university = new University();
                university.CreateDateTime = university.ModifiedDateTime = DateTime.Now;
                _dbContext.Universities.Add(university);
            }

            university.Name = model.Name;
            university.ShortName = model.ShortName;
            university.Page = model.Page;
            university.Color = model.Color;

            try
            {
                if (model.Logo != null)
                {
                    if (university.logo != null)
                    {
                        
                       await _fileService.RemoveFile(university.logo);
                    }
                    university.FilePathInDirectory = await _fileService
                        .SaveFileAsync(model.Logo, StringConstants.UniversityLogosDeroctory);
                    university.logo = GetPath(university.FilePathInDirectory);
                }

                await _dbContext.SaveChangesAsync();
                model.Id = university.Id;
            }
            catch (Exception ex)
            {
                return Error("Create", ex);
            }
            
            return RedirectToActionPermanent("Edit", model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(CreateUniversityViewModel model)
        {
            ModelState.Clear();
            var university = await _dbContext.Universities.FindAsync(model.Id);
            if (university == null)
            {
                return NotFound("Edit", model.Id);
            }
            await model.InitAsync(university, _dbContext);
            return View("Create", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var university = await _dbContext.Universities
                .Include(u=>u.Divisions)
                .ThenInclude(d=>d.EducationalDirections)
                .FirstOrDefaultAsync(u=>u.Id == id);

            if (university == null)
            {
                return NotFound("Delete", id);
            }
            await _fileService.RemoveFile(university.logo);
            _dbContext.Universities.Remove(university);
            await _dbContext.SaveChangesAsync();
            return RedirectToActionPermanent("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportData(IFormFile file, [FromServices] ExcelService service)
        {
            if (file != null)
            {
                var responce = await service.ImportFromExcel(file);
                if (responce.IsOk)
                {
                    TempData[SuccesMessage] = $"Добавлено/изменено {responce.AddItemCount} записей!";
                }else
                {
                    TempData[ErrorMessage] = $"При импорте произошла ошибка. {responce.ErrorMesage}";
                }

            }else
            {
                TempData[ErrorMessage] = "Файл не указан!";
                _logger.LogError("ImportData: file = null!");
            }
            return RedirectToAction("Index");
        }

        private string GetPath(string filename)
        {
            return $"/{StringConstants.UniversityLogosDeroctory}/{filename}";
        }
    }
}
