using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL
{
    public class ReportCardViewMDL
    {
        public int StudentId { get; set; }
        public string SchoolName { get; set; }
        public string SchoolAddress { get; set; }
        public string SchoolLogoUrl { get; set; }

        public string StudentName { get; set; }
        public string AdmissionNo { get; set; }
        public string Class { get; set; }
        public string Section { get; set; }
        public string RollNo { get; set; }
        public string DOB { get; set; }
        public string AcademicYear { get; set; }

        public List<SubjectGrade> SubjectGrades { get; set; } = new List<SubjectGrade>();

        public int TotalMarks => SubjectGrades?.Sum(s => s.Total) ?? 0;
        public int MaxMarks => (SubjectGrades?.Count ?? 0) * 100; // if each subject max = 100
        public double Percentage => MaxMarks == 0 ? 0 : (TotalMarks * 100.0) / MaxMarks;

        public string ResultRemark { get; set; }
        public string ClassTeacherName { get; set; }
        public string PrincipalName { get; set; }
    }

    public class SubjectGrade
    {
        public string SubjectName { get; set; }
        public int? PeriodicTest { get; set; }
        public int? Notebook { get; set; }
        public int? SubjectEnrichment { get; set; }
        public int? AnnualExam { get; set; }
        public int Total => (PeriodicTest ?? 0) + (Notebook ?? 0) + (SubjectEnrichment ?? 0) + (AnnualExam ?? 0);
        public string Grade { get; set; }
    }
}
