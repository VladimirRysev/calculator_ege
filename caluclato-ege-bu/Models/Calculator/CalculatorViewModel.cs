using DataAccessLayer;
using DataAccessLayer.Models;
using DataAccessLayer.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;

namespace Calculator_ege_bu.Models.Calculator
{
    public class CalculatorViewModel
    {
        public CalculatorViewModel()
        {
            Universities = new List<UniversityDto>();
        }

        public List<PropertyDto> Subjects { get; set; }
        public ICollection<PropertyDto> SubjectsForFilter { get; set; }
        public ICollection<UniversityDto> Results { get; set; }
        public ICollection<UniversityDto> Universities { get; set; }

        public async Task InitAsync(ApplicationDbContext dbContext, bool modelStateIsValid = true)
        {
            try
            {
                Subjects = Subjects?.Any() == true ? Subjects : 
                    await dbContext.Subjects
                    .Where(s=>s.Id < 14)
                    .OrderBy(s=>s.Id)
                    .Select(s=> new PropertyDto
                    { 
                        Id = s.Id,
                        Name = s.Name,
                        Score = s.Id == 6 ? (int?)100 : null
                    }).ToListAsync();

                var subjectNotInFilter =  await dbContext.Subjects
                    .Where(s=>s.Id > 13)
                    .OrderBy(s=>s.Id)
                    .Select(s=> new PropertyDto
                    { 
                        Id = s.Id,
                        Name = s.Name,
                        Score = 100
                    }).ToListAsync();

                var filterSubject = Subjects.Where(s=>s.Score > 0)
                    .ToList();
                var hasValues = filterSubject.Any();
                filterSubject.AddRange(subjectNotInFilter);
                var subjectIds = filterSubject
                    .Select(s=>s.Id)
                    .ToList();

                if (hasValues && modelStateIsValid)
                {
                    var query = dbContext.Directions
                        .Include(d=>d.EducationalDivision.University)
                        .Include(d=>d.SubjectScores)
                        .ThenInclude(ss=>ss.Subject)
                        .Where(d=> d.SubjectScores.All(ss=> subjectIds.Contains(ss.SubjectId)))
                        .Where(d=>d.SubjectScores.Any())
                        .Where(d => d.SubjectScores.All(ss =>
                                 ss.SubjectId == Subjects[0].Id && Subjects[0].Score >= ss.MinimumScore ||
                                 ss.SubjectId == Subjects[1].Id && Subjects[1].Score >= ss.MinimumScore ||
                                 ss.SubjectId == Subjects[2].Id && Subjects[2].Score >= ss.MinimumScore ||
                                 ss.SubjectId == Subjects[3].Id && Subjects[3].Score >= ss.MinimumScore ||
                                 ss.SubjectId == Subjects[4].Id && Subjects[4].Score >= ss.MinimumScore ||
                                 ss.SubjectId == Subjects[5].Id && Subjects[5].Score >= ss.MinimumScore ||
                                 ss.SubjectId == Subjects[6].Id && Subjects[6].Score >= ss.MinimumScore ||
                                 ss.SubjectId == Subjects[7].Id && Subjects[7].Score >= ss.MinimumScore ||
                                 ss.SubjectId == Subjects[8].Id && Subjects[8].Score >= ss.MinimumScore ||
                                 ss.SubjectId == Subjects[9].Id && Subjects[9].Score >= ss.MinimumScore ||
                                 ss.SubjectId == Subjects[10].Id && Subjects[10].Score >= ss.MinimumScore ||
                                 ss.SubjectId == Subjects[11].Id && Subjects[11].Score >= ss.MinimumScore ||
                                 ss.SubjectId == Subjects[12].Id && Subjects[12].Score >= ss.MinimumScore
                                 ));

                    var directions = await query
                        .ToListAsync();

                    Results = await directions.GroupBy(d=>d.EducationalDivision.University, (u, dir) => new UniversityDto
                        {
                            UniversityName = u.Name,
                            UniversityUrl = u.Page,
                            Logo = u.logo,
                            Color =u.Color,
                            Divisions = dir.GroupBy(x=>x.EducationalDivision, (d, dir) => new DivisionDto
                            {
                                DivisionName = d.Name,
                                DivisionUrl = d.PageUrl,
                                Directions = dir.Select(d => new DirectionDto
                                {
                                    DirectionName = d.Name,
                                    DirectionUrl = d.PageUrl,
                                    SubjectScores = d.SubjectScores.Select(x => $"{x.Subject.Name}({x.MinimumScore})").ToList(),
                                    BudgetPlacesCount = d.BudgetPlacesCount,
                                    Form = d.EducationalForm,
                                    Level = d.Level,
                                    logo = d.EducationalDivision.University.logo,
                                    PaidPlacesCount = d.PaidPlacesCount,
                                    PeriodOfStudy = d.PeriodOfStudy,
                                    Price = d.Price,
                                    Code = d.Code
                                }).ToList()
                            }).ToList()
                    }).ToListAsync();
                }
                if (Results?.Any() != true)
                {
                    Universities = await dbContext.Universities
                        .Include(u=>u.Divisions)
                        .ThenInclude(d=>d.EducationalDirections)
                        .Select(x=> new UniversityDto
                        {
                            Logo = x.logo,
                            Color = x.Color,
                            DirectionsCount = x.Divisions.SelectMany(d=>d.EducationalDirections).Count(),
                            DivisionsCount = x.Divisions.Count,
                            UniversityName = x.Name,
                            UniversityUrl = x.Page
                        }).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 

        
        public class UniversityDto
        {
            public string Logo { get; set; }
            public string Color { get; set; }
            public string UniversityUrl { get; set; }
            public string UniversityName { get; set; }
            public int DivisionsCount { get; set; }
            public int DirectionsCount { get; set; }
            public ICollection<DivisionDto> Divisions { get; set; }
        }

        public class DivisionDto
        {
            public string DivisionUrl { get; set; }
            public string DivisionName { get; set; }
            public ICollection<DirectionDto> Directions { get; set; }
        }

        public class DirectionDto
        {
            public ICollection<string> SubjectScores { get; set; }
            public string DirectionName { get; set; }
            public string DirectionUrl { get; set; }
            public string logo { get; set; }
            public double Price { get; set; }
            public EducationalFormEnum Form { get; set; }
            public double PeriodOfStudy { get; set; }
            public int BudgetPlacesCount { get; set; }
            public int PaidPlacesCount { get; set; }
            public EducationalLevelEnum Level { get; set; }
            public string Code { get; set; }
        }
    }

    public class PropertyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Range(minimum: 0, maximum: 100, ErrorMessage = "Значение должно быть от 0 до 100")]
        public int? Score { get; set; }
    }
}
