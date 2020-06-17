using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Models.Enums
{
    public enum EducationalLevelEnum
    {
        [Display(Name = "Бакалавриат")]
        Bachelor = 1,
        [Display(Name = "Специалитет")]
        SpecialistDegree = 2,
        [Display(Name = "Магистратура")]
        Magistracy = 3,
        [Display(Name = "Аспирантура")]
        Postgraduate = 4,
        [Display(Name = "Ординатура")]
        Residency = 5,
        [Display(Name = "Ассистентура-стажировка")]
        Assistant_internship = 6
    }
}
