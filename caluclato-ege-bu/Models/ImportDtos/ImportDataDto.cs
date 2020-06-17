using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calculator_ege_bu.Models.ImportDtos
{
    public class ImportDataDto
    {
        /// <summary>
        /// Университет
        /// </summary>
        public string UniversityName { get; set; }
        /// <summary>
        /// Факультет/Институт/школа
        /// </summary>
        public string DivisionName { get; set; }
        /// <summary>
        /// Уровень образования
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// Код направления
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Направление/специальность
        /// </summary>
        public string DirectionName { get; set; }
        /// <summary>
        /// Срок обучения (лет)
        /// </summary>
        public double PeriodOfStudy { get; set; }
        /// <summary>
        /// Форма обучения
        /// </summary>
        public string Form { get; set; }
        /// <summary>
        /// Количество бюджетных мест
        /// </summary>
        public double BudgetPlacesCount { get; set; }
        /// <summary>
        /// Количество платных мест
        /// </summary>
        public double PaidPlacesCount { get; set; }
        /// <summary>
        /// Стоимость обучения
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// Минимальный балл ЕГЭ по предмету
        /// </summary>
        public ICollection<SubjectImport> Subjects { get; set; }
    }

    public class SubjectImport
    {
        public string Name { get; set; }
        public double Score { get; set; }
    }

}
