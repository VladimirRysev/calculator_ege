using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Calculator_ege_bu.Models;
using DataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Calculator_ege_bu.Controllers
{
    public class BaseController<T> : Controller
    {
        protected readonly ApplicationDbContext _dbContext;
        protected readonly ILogger<T> _logger;
        protected const string ErrorMessage = "ErrorMessage";
        protected const string SuccesMessage = "SuccesMessage";
        public BaseController(ApplicationDbContext dbContex, ILogger<T> logger)
        {
            _dbContext = dbContex;
            _logger = logger;
        }

        protected IActionResult NotFound(string action, int modelId)
        {
            var errorMessage = $"{action}: not found! id = {modelId}";
            _logger.LogWarning(errorMessage);
            return NotFound();
        }

        protected IActionResult Error(string action, Exception ex)
        {
             var errorMessage = $"{action}.\n{ex}";
                _logger.LogWarning(errorMessage);
            return View("Error", new ErrorViewModel { Message = errorMessage });
        }

        protected void TrimStringProperties<Tmodel>(ref Tmodel model)
        {
            var modelType = model.GetType();
            var requiredProperties = modelType.GetProperties()
                .Where(p => p.PropertyType.Name == "String").ToList();
            foreach (var prop in requiredProperties)
            {
                var val = (prop.GetValue(model) as String)?.Trim();
                prop.SetValue(model, val);
                
            }
        }
    }
}
