using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Subject : EntityBase
    {
        public string Name { get; set; }
        public virtual ICollection<SubjectScore> SubjectScores { get; set; }
    }
}
