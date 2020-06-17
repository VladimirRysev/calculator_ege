using DataAccessLayer.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Calculator_ege_bu.Models.Accaunt
{
    public class EditUserViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="Логин")]
        public string Name { get; set; }
        [DataType(DataType.Password)]
        [Display(Name ="Текущий пароль")]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name ="Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [Display(Name ="Подтвердите пароль")]
        public string ConfirmPassword { get; set; }
        [Required]
        [Display(Name ="Роль")]
        public RoleEnum Role { get; set; }
    }
}
