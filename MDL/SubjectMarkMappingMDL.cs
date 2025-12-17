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
        public int FK_ExamCategoryId { get; set; }
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
        public string ExamType { get; set; }
        public string ExamCategory { get; set; }
        public string Attendancecount { get; set; }
        public string Remarks { get; set; }
        public string PromotedToClass { get; set; }
    }

    public class SubjectMarkInsertMDL
    {
        public int CompanyId { get; set; }
        public int ClassId { get; set; }
        public int StudentId { get; set; }
        public int ExamCategoryId { get; set; }
        public int PKId { get; set; }

        public List<SubjectMarkMDL> Marks { get; set; }

        /* 🔹 NEW: Co-Scholastic Grades */
        public List<CoScholasticGradesMDL> CoScholasticGrades { get; set; }

        public int userId { get; set; }

        /* 🔹 JSON FOR DB INSERT */
        public string JsonData { get; set; }
        public string CoScholasticJson { get; set; }
        public string Attendancecount { get; set; }
        public string Remarks { get; set; }
        public string PromotedToClass { get; set; }
    }

    public class SubjectMarkMDL
    {
        public int SubjectId { get; set; }
        public int ExamTypeId { get; set; }
        public int TotalMark { get; set; }
        public int ObtainMark { get; set; }
    }
    public class CoScholasticGradesMDL
    {
        public Int64 ID { get; set; }
        public string Value { get; set; }
    }

}
