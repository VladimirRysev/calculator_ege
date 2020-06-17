using DataAccessLayer;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace Calculator_ege_bu.Models.Admin.Universities
{
    public class UniversitiesListViewModel
    {
        public const int _pageSize = 50;
        public IPagedList<UniversityDto> Universities { get; set; }
        public int Page { get; set; } = 1;

        public async Task InitAsync(ApplicationDbContext db)
        {
            Universities = await db.Universities
                .Where(u=>true)
                .OrderByDescending(u=>u.CreateDateTime)
                .Select(u=> new UniversityDto(u))
                .ToPagedListAsync(Page, _pageSize);
        }

    }

    public class UniversityDto
    {
        public int Id { get; set; }

        [DisplayName("Название университета")]
        public string Name { get; set; }

        [DisplayName("Краткое название университета")]
        public string ShortName { get; set; }
        
        [DisplayName("Основной цвет")]
        public string Color { get; set; }

        [DisplayName("Логотип университета")]
        public string Logo { get; set; }

        [DisplayName("Официальная страница университета")]
        public string Page { get; set; }

        [DisplayName("Дата создания")]
        public DateTime CreateDateTime { get; set; }

        [DisplayName("Дата последнего изменения")]
        public DateTime ModifiedDateTime { get; set; }

        public UniversityDto(University university)
        {
            Id = university.Id;
            Name = university.Name;
            ShortName = university.ShortName;
            Color = university.Color;
            Logo = university.logo;
            Page = university.Page;
            CreateDateTime = university.CreateDateTime;
            ModifiedDateTime = university.ModifiedDateTime;
        }

    }
}
