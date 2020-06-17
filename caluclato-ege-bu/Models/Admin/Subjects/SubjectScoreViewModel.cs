using DataAccessLayer;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace Calculator_ege_bu.Models.Admin.Subjects
{
    public class SubjectScoreViewModel
    {
        public int Id { get; set; }
        public int DirectionId { get; set; }
        public string DirectionName { get; set; }
        public SubjectDto Subject { get; set; }
        public ICollection<SubjectDto> Subjects  { get; set; }
        [Required]
        [Range(minimum: 0, maximum: 100, ErrorMessage = "Значение должно быть от 0 до 100")]
        [Display(Name ="Минимальный бал")]
        public int MinScore { get; set; }

        public async Task InitAsync(ApplicationDbContext dbContex)
        {
            Subjects = await dbContex.Subjects.ToList()
                .Select(s=> new SubjectDto
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToListAsync();
        }

        public async Task InitAsync(SubjectScore subjectScore, ApplicationDbContext dbContex)
        {
            await InitAsync(dbContex);
            Id = subjectScore.Id;
            Subject = new SubjectDto{ Id = subjectScore.Subject.Id, Name = subjectScore.Subject.Name, Selected = true};
            foreach (var item in Subjects)
            {
                item.Selected = Subject.Id == item.Id;
            }
            MinScore = subjectScore.MinimumScore;
            DirectionId = subjectScore.EducationalDirectionId;
            DirectionName = subjectScore.EducationalDirection.Name;
        }
    }

    public class SubjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }
}
