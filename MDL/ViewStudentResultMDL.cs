using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL
{
    public class ViewStudentResultMDL
    {
        public List<ViewSubjectMDL> Subjects { get; set; }
        public List<ViewExamTypeMDL> ExamTypes { get; set; }
        public List<ExamMarksDetailMDL> ExamMarksDetails { get; set; }
    }


    public class ViewSubjectMDL
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }

    }

    public class ViewExamTypeMDL
    {
        public int Id { get; set; }
        public string ExamType { get; set; }
        public string TotalMark { get; set; }

    }

    public class ExamMarksDetailMDL
    {
        public int SchoolId { get; set; }
        public int ClassId { get; set; }
        public int StudentId { get; set; }
        public int ExamTypeId { get; set; }
        public int ExamCategoryId { get; set; }
        public int SubjectId { get; set; }

        public int TotalMarks { get; set; }
        public int ObtainMarks { get; set; }

        public string CompanyName { get; set; }
        public string ClassName { get; set; }
        public string StudentName { get; set; }
        public string ExamType { get; set; }
        public string ExamCategory { get; set; }
        public string Subject { get; set; }
    }



    // Example of your model (This is what I need from you to make the code perfect)
    public class ViewStudentResultNewMDL
    {
        public string StudentName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Class { get; set; }
        public string Section { get; set; }
        public string RollNumber { get; set; }
        public string StudentPhotoUrl { get; set; }

        public List<SubjectResult> Subjects { get; set; }
        public List<CoScholasticArea> CoScholasticAreas { get; set; }

        public int OverallMarks { get; set; }
        public int OverallMaxMarks { get; set; }
        public double OverallPercentage { get; set; }
        public string OverallGrade { get; set; }
        public string OverallRemarks { get; set; }
        public int Attendance { get; set; }
        public string Remarks { get; set; }
        public string PromotedToClass { get; set; }
        public DateTime Date { get; set; }
    }

    public class SubjectResult
    {
        public string SubjectName { get; set; }
        public TermMarks Term1 { get; set; }
        public TermMarks Term2 { get; set; }
        public double GrandTotal { get; set; }
        public string OverallGrade { get; set; }
    }

    public class TermMarks
    {
        public int PT { get; set; }
        public int NB { get; set; }
        public int SEA { get; set; }
        public int HY { get; set; }
        public int YE { get; set; }
        public int Total { get; set; }
        public string Grade { get; set; }
    }

    public class CoScholasticArea
    {
        public string AreaName { get; set; }
        public string Term1Grade { get; set; }
        public string Term2Grade { get; set; }
    }
}
