using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    /// <summary>
    /// Минимальный балл ЕГЭ по предмету для конкретного учебного подразделения
    /// </summary>
    public class SubjectScore : EntityBase
    {
        /// <summary>
        /// Идентификатор предмета
        /// </summary>
        public int SubjectId { get; set; }
        /// <summary>
        /// Предмет
        /// </summary>
        public virtual Subject Subject { get; set; }
        /// <summary>
        /// Идентификатор ссылки на образовательное направление и подразделение
        /// </summary>
        public int EducationalDirectionId { get; set; }
        /// <summary>
        /// Ссылка на образовательное направление и подразделение
        /// </summary>
        public virtual EducationalDirection EducationalDirection { get; set; }
        /// <summary>
        /// минимальный проходной бал
        /// </summary>
        public int MinimumScore { get; set; }
    }
}
