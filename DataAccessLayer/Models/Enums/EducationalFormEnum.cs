using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Models.Enums
{
    public enum EducationalFormEnum
    {
        [Display(Name ="Очная")]
        Intramural = 1,
        [Display(Name = "Заочная")]
        Correspondence = 2,
        [Display(Name = "Очно-заочная")]
        Intramural_Correspondence = 3,

    }
}
