using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL
{

    public class SubjectMarkMappingMDL
    {
        public int ? PKId { get; set; }
        public int FK_CompanyId { get; set; }
        
        public int FK_ClassId { get; set; }
        public int FK_StudentId { get; set; }
        public int LookUpDetailId { get; set; }
        public string FK_LookUpDetailIds { get; set; }
        public int FK_LookUpId { get; set; }
        public string TotalMark { get; set; }

        // Joined / Extra Fields
        public string ClassName { get; set; }
        public string CompanyName { get; set; }
        public string StudentName { get; set; }

        public int userId { get; set; }
        public bool IsActive { get; set; }
        // Placeholder fields (coming as empty string in query)
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }

        public string JsonData { get; set; }
    }
}
