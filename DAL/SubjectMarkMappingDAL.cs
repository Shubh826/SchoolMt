using DAL.DataUtility;
using MDL.Common;
using MDL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class SubjectMarkMappingDAL
    {
        static DataFunctions objDataFunctions = null;
        System.Data.DataSet objDataSet = null;
        static string _commandText = string.Empty;

        public SubjectMarkMappingDAL()
        {
            objDataFunctions = new DataFunctions();
        }
        public bool GetSubjectMarkMappingData(out List<SubjectMarkMappingMDL> List, out BasicPagingMDL objBasicPagingMDL, int id, int rowPerpage, int currentPage, int FK_CompanyId, string SearchBy, string SearchValue)
        {
            List = new List<SubjectMarkMappingMDL>();
            objBasicPagingMDL = new BasicPagingMDL();
            bool result = false;
            Messages objMessages = new Messages();
            _commandText = "[dbo].[usp_GetSubjectMarkMappingData]";
            List<SqlParameter> parms = new List<SqlParameter>
               {
                    new SqlParameter("@iRowperPage",rowPerpage),
                    new SqlParameter("@iCurrentPage",currentPage),
                    new SqlParameter("@FK_CompanyId",FK_CompanyId),
                    new SqlParameter("@SearchBy",SearchBy),
                    new SqlParameter("@SearchValue",SearchValue),
                    new SqlParameter("@icompId",id)
              };
            try
            {
                CheckParameters.ConvertNullToDBNull(parms);
                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        List = objDataSet.Tables[1].AsEnumerable().Select(dr => new SubjectMarkMappingMDL()
                        {
                            PKId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_ExamMarksMasterId")),
                            FK_CompanyId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_SchoolId")),
                            FK_ClassId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_ClassId")),
                            FK_StudentId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_StudentId")),
                            FK_LookUpId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_ExamTypeId")),
                            LookUpDetailId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_ExamTypeId")),
                            FK_ExamCategoryId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_ExamCategoryId")),
                            ClassName = dr.Field<string>("ClassName"),
                            CompanyName = dr.Field<string>("CompanyName"),
                            StudentName = dr.Field<string>("StudentName"),
                            CreatedBy = dr.Field<string>("CreatedBy"),
                            CreatedDate = dr.Field<string>("CreatedDate"),
                            ExamType = dr.Field<string>("ExamType"),
                            ExamCategory = dr.Field<string>("ExamCategory")

                        }).ToList();


                        objBasicPagingMDL = new BasicPagingMDL()
                        {
                            TotalItem = WrapDbNull.WrapDbNullValue<int>(objDataSet.Tables[2].Rows[0].Field<int?>("TotalItem")),
                            RowParPage = rowPerpage,
                            CurrentPage = currentPage
                        };
                        if (objBasicPagingMDL.TotalItem % objBasicPagingMDL.RowParPage == 0)
                        {
                            objBasicPagingMDL.TotalPage = objBasicPagingMDL.TotalItem / objBasicPagingMDL.RowParPage;
                        }
                        else
                            objBasicPagingMDL.TotalPage = objBasicPagingMDL.TotalItem / objBasicPagingMDL.RowParPage + 1;

                        objDataSet.Dispose();
                        result = true;
                    }
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public Messages InsertSubjectMarkMappingData(SubjectMarkMappingMDL obj)
        {
            Messages objMessages = new Messages();
            _commandText = "[dbo].[USP_InsertOrUpdateSubjectmark]";
            List<SqlParameter> parms = new List<SqlParameter>
               {

                    new SqlParameter("@PKId", obj.PKId == null? 0:obj.PKId),
                    new SqlParameter("@CompanyId",obj.FK_CompanyId),
                    new SqlParameter("@ClassId" ,obj.FK_ClassId),
                    new SqlParameter("@StudentId" ,obj.FK_StudentId),
                    new SqlParameter("@ExamTypeId" ,obj.LookUpDetailId),
                    new SqlParameter("@ExamcategoryId" ,obj.FK_ExamCategoryId),
                    new SqlParameter("@MarksData", obj.JsonData),
                    new SqlParameter("@CreatedBy" ,obj.userId)
              };
            try
            {
                CheckParameters.ConvertNullToDBNull(parms);
                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    objMessages.Message_Id = objDataSet.Tables[0].Rows[0].Field<int>("Message_Id");
                    objMessages.Message = objDataSet.Tables[0].Rows[0].Field<string>("Message");
                }
                else
                {
                    objMessages.Message_Id = 0;
                    objMessages.Message = "Failed";
                }
            }
            catch (Exception ex)
            {
                objMessages.Message_Id = 0;
                objMessages.Message = "Failed";
            }
            return objMessages;
        }

        public Messages DeleteSchoolConfigurationData(int pkId)
        {
            Messages objMessages = new Messages();
            _commandText = "[dbo].[usp_DeleteSubjectMarkMappingData]";
            List<SqlParameter> parms = new List<SqlParameter>
               {
                    new SqlParameter("@iPkId",pkId)
              };
            try
            {
                CheckParameters.ConvertNullToDBNull(parms);
                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    objMessages.Message_Id = objDataSet.Tables[0].Rows[0].Field<int>("Message_Id");
                    objMessages.Message = objDataSet.Tables[0].Rows[0].Field<string>("Message");
                }
                else
                {
                    objMessages.Message_Id = 0;
                    objMessages.Message = "Failed";
                }
            }
            catch (Exception ex)
            {
                objMessages.Message_Id = 0;
                objMessages.Message = "Failed";
            }
            return objMessages;
        }

        public ViewStudentResultMDL StudentReportCardDetails(int id)
        {
            ViewStudentResultMDL studentResult = new ViewStudentResultMDL();
            Messages objMessages = new Messages();
            _commandText = "[dbo].[usp_GetStudentResultData]";
            List<SqlParameter> parms = new List<SqlParameter>
    {
        new SqlParameter("@iStudentId", id)
    };

            try
            {
                CheckParameters.ConvertNullToDBNull(parms);
                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);

                if (objDataSet.Tables.Count > 0 && objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        // --- Table[1]: Subjects ---
                        studentResult.Subjects = objDataSet.Tables[1].AsEnumerable().Select(dr => new ViewSubjectMDL
                        {
                            Id = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("Id")),
                            SubjectName = dr.Field<string>("Value")
                        }).ToList();

                        // --- Table[2]: Exam Types ---
                        studentResult.ExamTypes = objDataSet.Tables[2].AsEnumerable().Select(dr => new ViewExamTypeMDL
                        {
                            Id = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("Id")),
                            ExamType = dr.Field<string>("Value"),
                            TotalMark = dr.Field<string>("TotalMark")
                        }).ToList();

                        // --- Table[3]: Exam Marks Detail ---
                        studentResult.ExamMarksDetails = objDataSet.Tables[3].AsEnumerable().Select(dr => new ExamMarksDetailMDL
                        {
                            SchoolId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_SchoolId")),
                            ClassId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_ClassId")),
                            StudentId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_StudentId")),
                            ExamCategoryId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_ExamCategoryId")),
                            ExamTypeId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_ExamTypeId")),
                            SubjectId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_SubjectId")),
                            TotalMarks = dr.Field<int>("TotalMarks"),
                            ObtainMarks = dr.Field<int>("ObtainMarks"),
                            CompanyName = dr.Field<string>("CompanyName"),
                            StudentName = dr.Field<string>("StudentName"),
                            ClassName = dr.Field<string>("ClassName"),
                            ExamType = dr.Field<string>("ExamType"),
                            ExamCategory = dr.Field<string>("ExamCategory"),
                            Subject = dr.Field<string>("Subject")
                        }).ToList();
                    }
                }

                objDataSet.Dispose();
            }
            catch (Exception ex)
            {
                // Handle exception or log it
            }

            return studentResult;
        }




        public MarksTableViewModel StudentReportCardDetails_New(int id)
        {
            MarksTableViewModel studentResult = new MarksTableViewModel();
            _commandText = "[dbo].[usp_GetStudentResultData]";
            List<SqlParameter> parms = new List<SqlParameter>
    {
        new SqlParameter("@iStudentId", id)
    };

            try
            {
                CheckParameters.ConvertNullToDBNull(parms);
                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);

                if (objDataSet.Tables.Count > 0 && objDataSet.Tables[0].Rows.Count > 0 &&
                    objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                {
                    // --- Table[1]: Subjects ---
                    studentResult.Subjects = objDataSet.Tables[1].AsEnumerable().Select(dr => new ViewSubjectMDL
                    {
                        Id = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("Id")),
                        SubjectName = dr.Field<string>("Value")
                    }).ToList();

                    // --- Table[2]: Exam Categories ---
                    studentResult.ExamCategories = objDataSet.Tables[2].AsEnumerable().Select(dr => new ViewExamCategoryMDL
                    {
                        Id = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("Id")),
                        ExamCategoryType = dr.Field<string>("Value")
                    }).ToList();

                    // --- Table[3]: Exam Types ---
                    studentResult.ExamTypes = objDataSet.Tables[3].AsEnumerable().Select(dr => new ViewExamTypeMDL
                    {
                        Id = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("Id")),
                        ExamType = dr.Field<string>("Value"),
                        TotalMark = dr.Field<string>("TotalMark")
                    }).ToList();

                    // --- Table[4]: Grades ---
                    studentResult.Grades = objDataSet.Tables[4].AsEnumerable().Select(dr => new ViewGrdaeMDL
                    {
                        Id = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("Id")),
                        GradeName = dr.Field<string>("Value"),
                        Min = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("MinValue")),
                        Max = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("MaxValue"))
                    }).ToList();

                    // --- Table[5]: Exam Marks ---
                    studentResult.Marks = objDataSet.Tables[5].AsEnumerable().Select(dr => new ExamMarksDetailMDL
                    {
                        SchoolId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_SchoolId")),
                        ClassId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_ClassId")),
                        StudentId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_StudentId")),
                        ExamCategoryId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_ExamCategoryId")),
                        ExamTypeId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_ExamTypeId")),
                        SubjectId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_SubjectId")),
                        TotalMarks = dr.Field<int>("TotalMarks"),
                        ObtainMarks = dr.Field<int>("ObtainMarks"),
                        CompanyName = dr.Field<string>("CompanyName"),
                        StudentName = dr.Field<string>("StudentName"),
                        ClassName = dr.Field<string>("ClassName"),
                        ExamType = dr.Field<string>("ExamType"),
                        ExamCategory = dr.Field<string>("ExamCategory"),
                        Subject = dr.Field<string>("Subject")
                    }).ToList();

                    // --- Table[6]: Student Detail ---
                    if (objDataSet.Tables.Count > 6 && objDataSet.Tables[6].Rows.Count > 0)
                    {
                        var dr = objDataSet.Tables[6].Rows[0];
                        studentResult.Student = new ViewStudentDataMDL
                        {
                            StudentId = Convert.ToInt32(dr["StudentId"]),
                            StudentName = dr["StudentName"]?.ToString(),
                            AdmissionNo = dr["AdmissionNo"]?.ToString(),
                            Class = dr["Class"]?.ToString(),
                            Section = dr["Section"]?.ToString(),
                            RollNo = dr["RollNo"]?.ToString(),
                            DOB = dr["DOB"]?.ToString(),
                            AcademicYear = dr["AcademicYear"]?.ToString(),
                            AffiliationNo = dr["AffiliationNo"]?.ToString(),
                            SchoolName = dr["SchoolName"]?.ToString(),
                            SchoolAddress = dr["SchoolAddress"]?.ToString(),
                            SchoolLogoUrl = dr["SchoolLogoUrl"]?.ToString(),
                            StudentImgUrl = dr["StudentImgUrl"]?.ToString(),
                            SchoolCode = dr["SchoolCode"]?.ToString(),
                            ClassTeacherName = dr["ClassTeacherName"]?.ToString(),
                            PrincipalName = dr["PrincipalName"]?.ToString(),
                            FatherName = dr["FatherName"]?.ToString(),
                            MotherName = dr["MotherName"]?.ToString(),
                            SchoolEmailId = dr["SchoolEmailId"]?.ToString(),
                            SchoolMobileNo = dr["SchoolMobileNo"]?.ToString(),
                            SchoolPin = dr["SchoolPin"]?.ToString(),
                            SchoolPhone = dr["SchoolPhone"]?.ToString(),
                            AbbreviationText = dr["AbbreviationText"]?.ToString(),
                            ResultRemark = dr["ResultRemark"]?.ToString()
                        };

                        // --- Calculate Total Marks, Percentage, and Grade ---
                        var marks = studentResult.Marks;
                        var grades = studentResult.Grades;

                        decimal totalObtainMarks = marks.Sum(m => m.ObtainMarks);
                        decimal totalMaxMarks = marks.Sum(m =>
                        {
                            if (int.TryParse(m.TotalMarks.ToString(), out int t)) return t;
                            return 0;
                        });

                        decimal percentage = totalMaxMarks > 0 ? (totalObtainMarks * 100) / totalMaxMarks : 0;
                        string overallGrade = grades.FirstOrDefault(g => percentage >= g.Min && percentage <= g.Max)?.GradeName ?? "-";

                        studentResult.Student.ObtainMarks = totalObtainMarks.ToString();
                        studentResult.Student.TotalMarks = totalMaxMarks.ToString();
                        studentResult.Student.Percentages = Math.Round(percentage, 2).ToString();
                        studentResult.Student.Grade = overallGrade;

                        studentResult.CoScholasticArea = new List<ViewCoScholasticArea>
                        {
                             new ViewCoScholasticArea { AreaName = "Work Education (or Pre-vocational Education)", Term1Grade = "A", Term2Grade = "A" },
                             new ViewCoScholasticArea { AreaName = "Art Education", Term1Grade = "A", Term2Grade = "A" },
                             new ViewCoScholasticArea { AreaName = "Health & Physical Education", Term1Grade = "A", Term2Grade = "A" },
                             new ViewCoScholasticArea { AreaName = "Discipline: Term-1/Term-2", Term1Grade = "Grade", Term2Grade = "Grade" },
                             new ViewCoScholasticArea { AreaName = "[on a 3-point (A-C) grading scale]", Term1Grade = "A", Term2Grade = "A" }
                        };
                    }
                }

                objDataSet.Dispose();
            }
            catch (Exception ex)
            {
                // Handle exception
            }

            return studentResult;
        }

    }
}
