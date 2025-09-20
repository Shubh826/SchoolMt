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
    public class GeneratedReportcardDAL
    {
        static DataFunctions objDataFunctions = null;
        System.Data.DataSet objDataSet = null;
        static string _commandText = string.Empty;

        public GeneratedReportcardDAL()
        {
            objDataFunctions = new DataFunctions();
        }

        public bool GetStudentData(out List<StudentMasterMDL> objStudentList, out BasicPagingMDL objBasicPagingMDL, int id, int rowPerpage, int currentPage, int FK_CompanyId, string SearchBy, string SearchValue)
        {
            objStudentList = new List<StudentMasterMDL>();
            objBasicPagingMDL = new BasicPagingMDL();
            bool result = false;
            Messages objMessages = new Messages();
            _commandText = "[usp_GetStudentData]";
            List<SqlParameter> parms = new List<SqlParameter>
               {
                    new SqlParameter("@iRowperPage",rowPerpage),
                    new SqlParameter("@iCurrentPage",currentPage),
                    new SqlParameter("@Fk_CompanyId",FK_CompanyId),
                    new SqlParameter("@SearchBy",SearchBy),
                    new SqlParameter("@SearchValue",SearchValue),
                    new SqlParameter("@PK_StudentId",id)
              };
            try
            {
                CheckParameters.ConvertNullToDBNull(parms);
                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        objStudentList = objDataSet.Tables[1].AsEnumerable().Select(dr => new StudentMasterMDL()
                        {
                            PK_SudentId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_SudentId")),
                            FK_CompanyId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_CompanyId")),
                            StudentName = dr.Field<string>("StudentName"),
                            ClassName = dr.Field<string>("ClassName"),
                            ClassCode = dr.Field<string>("ClassCode"),
                            FatherName = dr.Field<string>("FatherName"),
                            MotherName = dr.Field<string>("MotherName"),
                            Address = dr.Field<string>("Address"),
                            GuardianContactNo = dr.Field<string>("GuardianContactNo"),
                            Emergency_Contact_No = dr.Field<string>("Emergency_Contact_No"),
                            StudentImageUrl = dr.Field<string>("StudentImageUrl"),
                            Gender = dr.Field<string>("Gender"),
                            Category = dr.Field<string>("Category"),
                            IsActive = dr.Field<bool>("IsActive"),
                            ImageName = dr.Field<string>("ImageName"),
                            StudentDOB = dr.Field<string>("StudentDOB"),
                            FK_AreaId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_AreaId")),
                            MonthlyFee = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("MonthlyFee")),
                            TransportFee = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("TransportFee")),
                            Discount = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("Discount")),

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


        public ReportCardViewMDL GetStudentDataForReportCard(int id)
        {
            ReportCardViewMDL obj = new ReportCardViewMDL();
            _commandText = "[dbo].[usp_GetStudentDataForReportCard]";
            List<SqlParameter> parms = new List<SqlParameter>
    {
        new SqlParameter("@PK_StudentId", id)
    };

            try
            {
                CheckParameters.ConvertNullToDBNull(parms);
                objDataSet = (DataSet)objDataFunctions.getQueryResult(
                    _commandText,
                    DataReturnType.DataSet,
                    parms
                );

                if (objDataSet.Tables[0].Rows.Count > 0 &&
                    objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                {
                    // ✅ Student Info (Table[1])
                    if (objDataSet.Tables.Count > 1 && objDataSet.Tables[1].Rows.Count > 0)
                    {
                        var dr = objDataSet.Tables[1].Rows[0];

                        
                        obj.StudentId =Convert.ToInt32( dr["StudentId"]);
                        obj.StudentName = dr["StudentName"]?.ToString();
                        obj.AdmissionNo = dr["AdmissionNo"]?.ToString();
                        obj.Class = dr["Class"]?.ToString();
                        obj.Section = dr["Section"]?.ToString();
                        obj.RollNo = dr["RollNo"]?.ToString();
                        obj.DOB = dr["DOB"]?.ToString();
                        obj.AcademicYear = dr["AcademicYear"]?.ToString();

                        // ✅ School Info (static for now, you can pull from DB/config)
                        obj.SchoolName = dr["SchoolName"]?.ToString();
                        obj.SchoolAddress = dr["SchoolAddress"]?.ToString();
                        obj.SchoolLogoUrl = dr["SchoolLogoUrl"]?.ToString();

                        // ✅ Teachers (static for now)
                        obj.ClassTeacherName = dr["ClassTeacherName"]?.ToString();
                        obj.PrincipalName = dr["PrincipalName"]?.ToString();
                    }

                    // ✅ Marks Info (Table[2])
                    if (objDataSet.Tables.Count > 2 && objDataSet.Tables[2].Rows.Count > 0)
                    {
                        obj.SubjectGrades = objDataSet.Tables[2].AsEnumerable()
                            .Select(dr => new SubjectGrade
                            {
                                SubjectName = dr.Field<string>("SubjectName"),
                                PeriodicTest = dr.Field<int?>("PeriodicTest"),
                                Notebook = dr.Field<int?>("Notebook"),
                                SubjectEnrichment = dr.Field<int?>("SubjectEnrichment"),
                                AnnualExam = dr.Field<int?>("AnnualExam"),
                                Grade = dr.Field<string>("Grade")
                            }).ToList();
                    }

                    // ✅ Calculate Result Remark
                    if (obj.Percentage >= 75)
                        obj.ResultRemark = "Excellent Performance";
                    else if (obj.Percentage >= 60)
                        obj.ResultRemark = "Good Performance";
                    else if (obj.Percentage >= 40)
                        obj.ResultRemark = "Satisfactory";
                    else
                        obj.ResultRemark = "Needs Improvement";
                }
            }
            catch (Exception ex)
            {
                // log exception
                throw;
            }
            finally
            {
                objDataSet?.Dispose();
            }

            return obj;
        }

    }
}
