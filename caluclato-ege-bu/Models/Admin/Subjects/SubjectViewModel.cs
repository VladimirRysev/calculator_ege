using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Calculator_ege_bu.Models.Admin.Subjects
{
    public class SubjectViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name="Название")]
        [MaxLength(300, ErrorMessage ="Максимальная длина 300 символов")]
        public string Name { get; set; }
    }
}
