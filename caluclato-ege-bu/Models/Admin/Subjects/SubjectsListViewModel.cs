using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calculator_ege_bu.Models.Admin.Subjects
{
    public class SubjectsListViewModel
    {
        public ICollection<SubjectItemDto> Subjects { get; set; }
        public async Task InitAsync(ApplicationDbContext dbContext)
        {
            Subjects = await dbContext.Subjects
                .Select(s=>new SubjectItemDto
                { 
                    Id = s.Id,
                    Name = s.Name,
                    CanDelete = s.Id > 13
                }).ToListAsync();
        }
    }

    public class SubjectItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool CanDelete { get; set; }
    }
}
