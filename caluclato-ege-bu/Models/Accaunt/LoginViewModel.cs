using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Calculator_ege_bu.Models.Accaunt
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name ="Логин")]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="Пароль")]
        public string Password { get; set; }
        [Display(Name ="Запомнить меня")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
