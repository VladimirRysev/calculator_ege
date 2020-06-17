using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using System.ComponentModel.DataAnnotations;
using X.PagedList;
using Calculator_ege_bu.Models.Admin.Directions;

namespace Calculator_ege_bu.Models.Admin.Divisions
{
    public class DivisionViewModel
    {
        public int Id { get; set; }
        [Display(Name ="Название")]
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        public int UniversityId { get; set; }
        public int Page { get; set; }
        [Display(Name ="Официальная страница")]
        [MaxLength(200)]
        public string PageUrl { get; set; }
        public string UniversityName { get; set; }
        public int DirectionPage { get; set; }= 1;
        public DirectionsListViewModel Directions { get; set; }
        public async Task  Init(EducationalDivision division, ApplicationDbContext dbContex)
        {
            Id = division.Id;
            Name = division.Name;
            PageUrl = division.PageUrl;
            UniversityId = division.UniversityId;
            UniversityName = division.University.Name;
            
            Directions = new DirectionsListViewModel();
            Directions.Page = DirectionPage;
            await Directions.InitAsync(division.Id, dbContex);
        }
    }
}
