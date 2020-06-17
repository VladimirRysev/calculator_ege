using DataAccessLayer;
using DataAccessLayer.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace Calculator_ege_bu.Models.Admin.Directions
{
    public class DirectionsListViewModel
    {
        private const int _pageSize = 50;
        public IPagedList<DirectionDto> Directions { get; set; }
        public int Page { get; set; } = 1;
        public int Divisionid { get; set; }
        public async Task InitAsync(int divisionId, ApplicationDbContext dbContex)
        {
            Divisionid = divisionId;
            Directions = await dbContex.Directions
                .Where(d=>d.EducationalDivisionId == divisionId)
                .Select(d=> new DirectionDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    PageUrl = d.PageUrl,
                    CreateDateTime = d.CreateDateTime,
                    ModifiedDateTime = d.ModifiedDateTime,
                    BudgetPlacesCount = d.BudgetPlacesCount,
                    Form = d.EducationalForm,
                    Code = d.Code,
                    Level = d.Level,
                    PaidPlacesCount = d.PaidPlacesCount,
                    PeriodOfStudy = d.PeriodOfStudy,
                    Price = d.Price
                }).ToPagedListAsync(Page, _pageSize);
        }
    }

    public class DirectionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PageUrl { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime ModifiedDateTime { get; set; }
        public string Code { get; set; }
        public double Price { get; set; }
        public EducationalFormEnum Form { get; set; }
        public int PaidPlacesCount { get; set; }
        public double PeriodOfStudy { get; set; }
        public EducationalLevelEnum Level { get; set; }
        public int BudgetPlacesCount { get; set; }
    }
}
