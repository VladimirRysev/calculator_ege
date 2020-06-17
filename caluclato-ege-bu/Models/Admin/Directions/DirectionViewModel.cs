using DataAccessLayer.Models;
using DataAccessLayer.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Calculator_ege_bu.Models.Admin.Directions
{
    public class DirectionViewModel
    {
        public int Id { get; set; }
        [Display(Name ="Название учебного направления")]
        [MaxLength(200)]
        [Required]
        public string Name { get; set; }
        [Display(Name ="Официальная страница")]
        [MaxLength(200)]
        public string PageUrl { get; set; }
        [Display(Name ="Код")]
        [MaxLength(50)]
        public string Code { get; set; }
        [Display(Name ="Стоимость обучения")]
        [UIHint("Double")]
        public double Price { get; set; }
        [Display(Name = "Форма обучения")]
        public EducationalFormEnum Form { get; set; }
        public int UniversityId { get; set; }
        public string UniversityName { get; set; }
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }
        [Display(Name ="Количество платных мест")]
        public int PaidPlacesCount { get; set; }
        [Display(Name ="Срок обучения")]
        [UIHint("Double")]
        public double PeriodOfStudy { get; set; }
        [Display(Name ="Уровень образования")]
        public EducationalLevelEnum Level { get; set; }
        [Display(Name ="Количество бюджетных мест")]
        public int BudgetPlacesCount { get; set; }
        public ICollection<SubjectScoreDto> Subjects { get; set; }

        public void Init(EducationalDirection direction)
        {
            Id = direction.Id;
            Name = direction.Name;
            PaidPlacesCount = direction.PaidPlacesCount;
            BudgetPlacesCount = direction.BudgetPlacesCount;
            PeriodOfStudy = direction.PeriodOfStudy;
            Code = direction.Code;
            Level = direction.Level;
            Form = direction.EducationalForm;
            PageUrl = direction.PageUrl;
            UniversityId = direction.EducationalDivision.UniversityId;
            UniversityName = direction.EducationalDivision.University.Name;
            Price = direction.Price;

            DivisionId = direction.EducationalDivisionId;
            DivisionName = direction.EducationalDivision.Name;

            Subjects = direction.SubjectScores?
                .Select(s=> new SubjectScoreDto
                { 
                    Id = s.Id,
                    MinScore = s.MinimumScore,
                    SubjectName = s.Subject.Name
                }).ToList();
        }
    }

    public class SubjectScoreDto
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }
        public int MinScore { get; set; }
    }
}
