using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    /// <summary>
    /// Университет
    /// </summary>
    public class University : EntityBase
    {
        /// <summary>
        /// Название университета
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Сокращенное название университета
        /// </summary>
        public string ShortName { get; set; }
        /// <summary>
        /// Основной цвет
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// Путь к файлу в хранилище
        /// </summary>
        public string FilePathInDirectory { get; set; }
        /// <summary>
        /// Логотип
        /// </summary>
        public string logo { get; set; }
        /// <summary>
        /// Ссылка на официальный сайт
        /// </summary>
        public string Page { get; set; }
        /// <summary>
        /// Подразделения
        /// </summary>
        public virtual ICollection<EducationalDivision> Divisions { get; set; }
    }
}
