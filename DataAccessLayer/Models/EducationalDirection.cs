using DataAccessLayer.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    /// <summary>
    /// Образовательное направление/специальность
    /// </summary>
    public class EducationalDirection : EntityBase
    {
        /// <summary>
        /// Название направления/специальности
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Официальная страница
        /// </summary>
        public string PageUrl { get; set; }
        /// <summary>
        /// Уровень обучения
        /// </summary>
        public EducationalLevelEnum Level { get; set; }
        /// <summary>
        /// Код специальности/направления
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Форма обучения
        /// </summary>
        public EducationalFormEnum EducationalForm { get; set; }
        /// <summary>
        /// Период обучения
        /// </summary>
        public double PeriodOfStudy { get; set; }
        /// <summary>
        /// Количество бюджетных мест
        /// </summary>
        public int BudgetPlacesCount { get; set; }
        /// <summary>
        /// Количество платных мест
        /// </summary>
        public int PaidPlacesCount { get; set; }
        /// <summary>
        /// Стоимость обучения
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// Минимальный балл ЕГЭ по предмету
        /// </summary>
        public ICollection<SubjectScore> SubjectScores { get; set; }
        /// <summary>
        /// Идентификатор образовательного подразделения
        /// </summary>
        public int EducationalDivisionId { get; set; }
        /// <summary>
        /// Образовательное подразделение
        /// </summary>
        public virtual EducationalDivision EducationalDivision { get; set; }
    }
}
