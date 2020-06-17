using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    /// <summary>
    /// Образовательное подразделение
    /// </summary>
    public class EducationalDivision : EntityBase
    {
        /// <summary>
        /// Название подразделение
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Ссылка на официальную страницу
        /// </summary>
        public string PageUrl { get; set; }
        /// <summary>
        /// Идентификатор университета
        /// </summary>
        public int UniversityId { get; set; }
        /// <summary>
        /// Университет
        /// </summary>
        public University University { get; set; }
        /// <summary>
        /// Образовательные направления
        /// </summary>
        public virtual ICollection<EducationalDirection> EducationalDirections { get; set; }
    }
}
