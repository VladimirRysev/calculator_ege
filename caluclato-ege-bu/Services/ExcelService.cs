using Calculator_ege_bu.Extentions;
using Calculator_ege_bu.Models.ImportDtos;
using DataAccessLayer;
using DataAccessLayer.Models;
using DataAccessLayer.Models.Enums;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Logging;
using NLog.LayoutRenderers.Wrappers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Calculator_ege_bu.Services
{
    public class ExcelService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ExcelService> _logger;
        private const int _subjectIndexStart = 10;
        public const int  _subjectCount = 13;
        public ExcelService(ILogger<ExcelService> logger, ApplicationDbContext dbContex)
        {
            _dbContext = dbContex;
            _logger = logger;
        }

        public async Task<ResponceExcelService> ImportFromExcel(IFormFile file)
        {
            var responce = new ResponceExcelService();
            //try
            //{
                responce.IsOk = true;
                ICollection<ImportDataDto> result = new List<ImportDataDto>();
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    using var reader = ExcelReaderFactory.CreateReader(stream);
                    var subjectNames = new List<string>();
                    reader.Read();
                    reader.Read();
                    for (int i = _subjectIndexStart; i < reader.FieldCount; i++)
                    {
                        var str = reader.GetString(i);
                        if (!string.IsNullOrEmpty(str))
                        {
                            subjectNames.Add(str);
                        }
                    }

                    if ( subjectNames.Count > 13)
                    {
                        responce.IsOk = false;
                        responce.ErrorMesage = "В файле присутствуют предметы, которых нет в списке. Скачайте шаблон и заполните его.";
                        return responce;
                    }
                    while (reader.Read())
                    {
                        if (
                            string.IsNullOrEmpty(reader.GetString(0)?.Trim()) ||
                            string.IsNullOrEmpty(reader.GetString(1)?.Trim()) ||
                            string.IsNullOrEmpty(reader.GetString(4)?.Trim()) )
                        {
                            continue;
                        }
                        var data = new ImportDataDto();
                        data.UniversityName = reader.GetString(0)?.Trim();
                        data.DivisionName = reader.GetString(1)?.Trim();
                        data.Level = reader.GetString(2)?.Trim();
                        data.Code = reader.GetString(3)?.Trim();
                        data.DirectionName = reader.GetString(4)?.Trim();
                        data.PeriodOfStudy = reader.IsDBNull(5)? 0: reader.GetDouble(5);
                        data.Form = reader.GetString(6)?.Trim();
                        data.BudgetPlacesCount = reader.IsDBNull(7)? 0: reader.GetDouble(7);
                        data.PaidPlacesCount = reader.IsDBNull(8)? 0: reader.GetDouble(8);
                        data.Price = reader.IsDBNull(9)? 0: reader.GetDouble(9);

                        data.Subjects = new List<SubjectImport>();

                        for (int i = 0; i < reader.FieldCount - _subjectIndexStart; i++)
                        {
                            if (!reader.IsDBNull(i + _subjectIndexStart))
                            {
                                var sub = new SubjectImport
                                {
                                    Name = subjectNames[i]?.Trim(),
                                    Score = reader.IsDBNull(i + _subjectIndexStart)? 0 : reader.GetDouble(i + _subjectIndexStart)
                                };
                                data.Subjects.Add(sub);
                            }
                        }

                        result.Add(data);
                    }
                }

                responce.AddItemCount = result.Count;
                if (responce.IsOk)
                {
                    var responceSave = await SaveToDatabase(result);
                    if (!responceSave.IsOk)
                    {
                        responce.IsOk = false;
                        responce.ErrorMesage = responceSave.ErrorMesage;
                    }
                }
                
                return responce;
            //}
            //catch (Exception ex)
            //{
            //    var errorMessage = $"SaveToDatabase.\n{ex}";
            //    _logger.LogWarning(errorMessage);
            //    responce.IsOk = false;
            //    responce.ErrorMesage = ex.Message;
            //    return responce;
            //}
        }

        private async Task<ResponceExcelService> SaveToDatabase(ICollection<ImportDataDto> dataDtos)
        {
            var responce = new ResponceExcelService();
            try
            {

                var subjects = dataDtos
                    .SelectMany(x=>x.Subjects.Select(s=>s.Name))
                    .Distinct().ToList();
                var subjectsInDb = _dbContext.Subjects.Select(s=>s.Name).ToList();
                
                if (!subjects.All(s=>subjectsInDb.Any(x=>x==s)))
                {
                    responce.IsOk = false;
                    responce.ErrorMesage = "В файле присутствуют предметы, которых нет в списке. Скачайте шаблон и заполните его.";
                    return responce;
                }

                ICollection<University> universities = new List<University>();
                var data = dataDtos.GroupBy(x=>x.UniversityName, (u, us) =>new 
                { 
                    UniversityName = u,
                    Divisions = us.GroupBy(d => d.DivisionName, (d, ds) => new
                    {
                        DivisionName = d,
                        Directions = ds.Select(dir => new
                          {
                              DirectionName = dir.DirectionName,
                              Budjet = dir.BudgetPlacesCount,
                              Payment = dir.PaidPlacesCount,
                              Price = dir.Price,
                              Level = dir.Level,
                              Form = dir.Form,
                              Code = dir.Code,
                              Period = dir.PeriodOfStudy,
                              Subjects = dir.Subjects
                          }).ToList()
                    }).ToList()
                }).ToList();
                var now = DateTime.Now;
                foreach (var item in data)
                {
                    University university = await _dbContext.Universities
                        .Include(u=>u.Divisions)
                        .FirstOrDefaultAsync(u=>u.Name == item.UniversityName);
                    if (university == null)
                    {
                        university = new University();
                        university.Name = item.UniversityName;
                        university.CreateDateTime = university.ModifiedDateTime = now;
                        university.Divisions = new List<EducationalDivision>();
                        _dbContext.Universities.Add(university);
                       await _dbContext.SaveChangesAsync();
                    }

                    foreach (var division in item.Divisions)
                    {
                        EducationalDivision educationalDivision = await _dbContext.Divisions
                            .Include(x=>x.EducationalDirections)
                            .FirstOrDefaultAsync(d=>d.Name == division.DivisionName && d.UniversityId == university.Id);
                        if (educationalDivision==null)
                        {
                            educationalDivision = new EducationalDivision();
                            educationalDivision.Name = division.DivisionName;
                            educationalDivision.CreateDateTime = educationalDivision.ModifiedDateTime = now;
                            educationalDivision.EducationalDirections = new List<EducationalDirection>();
                            university.Divisions.Add(educationalDivision);
                            await _dbContext.SaveChangesAsync();
                        }

                        foreach (var direction in division.Directions)
                        {
                            var form = direction.Form.GetEnumFromString<EducationalFormEnum>();
                            var level = direction.Level.GetEnumFromString<EducationalLevelEnum>();
                            EducationalDirection educationalDirection = await _dbContext.Directions
                                .Include(x=>x.SubjectScores)
                                .FirstOrDefaultAsync(x=>x.Name == direction.DirectionName &&
                                x.EducationalDivisionId == educationalDivision.Id &&
                                x.EducationalForm == form &&
                                x.Level == level);
                            if (educationalDirection == null)
                            {
                                educationalDirection = new EducationalDirection();
                                educationalDirection.Name = direction.DirectionName;
                                educationalDirection.CreateDateTime = educationalDirection.ModifiedDateTime = now;

                                educationalDivision.EducationalDirections.Add(educationalDirection);
                            }
                            else
                            {
                                educationalDirection.ModifiedDateTime = now;
                            }
                            
                            educationalDirection.Level = level;
                            educationalDirection.EducationalForm = form;
                            educationalDirection.BudgetPlacesCount = (int)direction.Budjet;
                            educationalDirection.PaidPlacesCount = (int)direction.Payment;
                            educationalDirection.PeriodOfStudy = direction.Period;
                            educationalDirection.Price = (int)direction.Price;
                            educationalDirection.Code = direction.Code;

                            if (educationalDirection.SubjectScores?.Any() == true)
                            {
                                educationalDirection.SubjectScores.Clear();
                            }
                            else if (educationalDirection.SubjectScores == null)
                            {
                                educationalDirection.SubjectScores = new List<SubjectScore>();
                            }
                            await _dbContext.SaveChangesAsync();
                            foreach (var subjectDto in direction.Subjects)
                            {
                                var subject = await _dbContext.Subjects.FirstOrDefaultAsync(x=>x.Name == subjectDto.Name);
                                if (subject != null)
                                {
                                    var subjectScore = new SubjectScore();
                                    subjectScore.MinimumScore = (int)subjectDto.Score;
                                    subjectScore.CreateDateTime = subjectScore.ModifiedDateTime = now;
                                    subjectScore.Subject = subject;
                                    educationalDirection.SubjectScores.Add(subjectScore);
                                }
                            }
                        }
                    }
                }
                await _dbContext.SaveChangesAsync();
                responce.IsOk = true;
                return responce;
            }
            catch (Exception ex)
            {
                 var errorMessage = $"SaveToDatabase.\n{ex}";
                _logger.LogWarning(errorMessage);
                responce.IsOk = false;
                responce.ErrorMesage = ex.Message;
                return responce;
            }
        }
    }

    public class ResponceExcelService
    {
        public bool IsOk { get; set; }
        public string ErrorMesage { get; set; }
        public int AddItemCount { get; set; }
    }

}
