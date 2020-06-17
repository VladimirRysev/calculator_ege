using Calculator_ege_bu.Models.Admin.Divisions;
using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace Calculator_ege_bu.Models.Admin.Universities
{
    public class CreateUniversityViewModel
    {
        public int Id { get; set; }

        [DisplayName("Название университета")]
        [Required]
        [MaxLength(300)]
        public string Name { get; set; }

        [DisplayName("Краткое название университета")]
        [Required]
        [MaxLength(100)]
        public string ShortName { get; set; }
        
        [DisplayName("Основной цвет")]
        [Required]
        public string Color { get; set; }

        [DisplayName("Логотип университета")]
        public string LogoPath { get; set; }

        [DisplayName("Официальная страница университета")]
        public string Page { get; set; }

        [DisplayName("Дата создания")]
        public DateTime CreateDateTime { get; set; }

        [DisplayName("Дата последнего изменения")]
        public DateTime ModifiedDateTime { get; set; }

        [DisplayName("Логотип университета")]
        public IFormFile Logo { get; set; }
        public DivisionsListViewModel Divisions { get; set; }
        public int DivisionsPage { get; set; } = 1;
        public string LogoName { get; set; }

        public async Task InitAsync(University university, ApplicationDbContext dbContex)
        {
            Id = university.Id;
            Name = university.Name;
            ShortName = university.ShortName;
            Color = university.Color;
            LogoPath = university.logo;
            Page = university.Page;
            CreateDateTime = university.CreateDateTime;
            ModifiedDateTime = university.ModifiedDateTime;

            Divisions = new DivisionsListViewModel();
            Divisions.UniversityId = Id;
            Divisions.Page = DivisionsPage == 0 ? 1: DivisionsPage;
            await Divisions.InitAsync(dbContex);

        }
    }
}
