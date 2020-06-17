using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace Calculator_ege_bu.Models.Admin.Divisions
{
    public class DivisionsListViewModel
    {
        public const int _pageSize = 50;
        public int Page { get; set; } = 1;
        public int? UniversityId { get; set; }
        public IPagedList<DivisionDto> Divisions { get; set; }

        public async Task InitAsync(ApplicationDbContext dbContext)
        {
            var divisionsQuery = dbContext.Divisions.Where(d=>true);
            if (UniversityId.HasValue)
            {
                divisionsQuery = divisionsQuery.Where(d=>d.UniversityId == UniversityId.Value);
            }

            Divisions = await divisionsQuery.Select(d=>new DivisionDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    CreateDateTime = d.CreateDateTime,
                    PageUrl = d.PageUrl,
                    ModifiedDateTime = d.ModifiedDateTime
                }).ToPagedListAsync(Page, _pageSize);
        }
    }

    public class DivisionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PageUrl { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime ModifiedDateTime { get; set; }
    }
}
