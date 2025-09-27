using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public int Percentagevalue { get; set; }


        public string CompanyName { get; set; }
        public string ClassName { get; set; }
        public string StudentName { get; set; }
        public string ExamType { get; set; }
        public string ExamCategory { get; set; }
        public string Subject { get; set; }
        public string SubjectGrade { get; set; }

    }





    // Example of your model (This is what I need from you to make the code perfect)
    
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

    /*START::New Model*/
    public class MarksTableViewModel
    {
        public ViewStudentDataMDL Student { get; set; }
        public List<ExamMarksDetailMDL> Marks { get; set; }
        public List<ViewExamCategoryMDL> ExamCategories { get; set; }
        public List<ViewExamTypeMDL> ExamTypes { get; set; }
        public List<ViewSubjectMDL> Subjects { get; set; }
        public List<ViewGrdaeMDL> Grades { get; set; }
        public List<ViewCoScholasticArea> CoScholasticArea { get; set; }

    }


    public class ViewStudentDataMDL
    {
        public int StudentId { get; set; }
        public string SchoolName { get; set; }
        public string SchoolAddress { get; set; }
        public string SchoolLogoUrl { get; set; }
        public string StudentImgUrl { get; set; }
        public string StudentName { get; set; }
        public string AdmissionNo { get; set; }
        public string Class { get; set; }
        public string Section { get; set; }
        public string RollNo { get; set; }
        public string DOB { get; set; }
        public string AcademicYear { get; set; }
        public string AffiliationNo { get; set; }
        public string SchoolCode { get; set; }
        public string ResultRemark { get; set; }
        public string ClassTeacherName { get; set; }
        public string PrincipalName { get; set; }

        public string FatherName { get; set; }
        public string MotherName { get; set; }

        public string SchoolEmailId { get; set; }
        public string SchoolMobileNo { get; set; }
        public string SchoolPin { get; set; }
        public string SchoolPhone { get; set; }
        public string SchoolWebSiteUrl { get; set; }
        public string Grade { get; set; }
        public string Percentages { get; set; }
        public string TotalMarks { get; set; }
        public string ObtainMarks { get; set; }
        public string AbbreviationText { get; set; }


    }

    public class ViewGrdaeMDL
    {
        public int Id { get; set; }
        public string GradeName { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }

    }
    public class ViewSubjectMDL
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }

    }

    public class ViewExamCategoryMDL
    {
        public int Id { get; set; }
        public string ExamCategoryType { get; set; }

    }

    public class ViewExamTypeMDL
    {
        public int Id { get; set; }
        public string ExamType { get; set; }
        public string TotalMark { get; set; }

    }

    public class ViewCoScholasticArea
    {
        public string AreaName { get; set; }
        public string Term1Grade { get; set; }
        public string Term2Grade { get; set; }
    }



    /*END:New Model*/
}
