using DAL.DataUtility;
using MDL.Common;
using MDL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class StudentReportDAL
    {
        #region
        static DataFunctions objDataFunctions = null;
        DataSet objDataSet = null;
        static string _commandText = string.Empty;
        #endregion

        public StudentReportDAL()
        {
            objDataFunctions = new DataFunctions();
        }

        public bool GetStudentReportData(out List<StudentReportMDL> objStudentReportList, out BasicPagingMDL objBasicPagingMDL, int id, int rowPerpage, int currentPage, int FK_CompanyId, string SearchBy, string SearchValue,string fromDate,string toDate)
        {
            objStudentReportList = new List<StudentReportMDL>();
            objBasicPagingMDL = new BasicPagingMDL();
            bool result = false;
            Messages objMessages = new Messages();
            _commandText = "[dbo].[usp_GetStudentReportData]";
            List<SqlParameter> parms = new List<SqlParameter>
               {
                    new SqlParameter("@iRowperPage",rowPerpage),
                    new SqlParameter("@iCurrentPage",currentPage),
                    new SqlParameter("@Fk_CompanyId",FK_CompanyId),
                    new SqlParameter("@SearchBy",SearchBy),
                    new SqlParameter("@SearchValue",SearchValue),
                    new SqlParameter("@PK_StudentId",id),
                    new SqlParameter("@cfromDate",fromDate),
                    new SqlParameter("@ctoDate",toDate)
              };
            try
            {
                CheckParameters.ConvertNullToDBNull(parms);
                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        objStudentReportList = objDataSet.Tables[1].AsEnumerable().Select(dr => new StudentReportMDL()
                        {
                            StudentName = dr.Field<string>("StudentName"),
                            ClassName = dr.Field<string>("ClassName"),
                            FatherName = dr.Field<string>("FatherName"),
                            MobileNo = dr.Field<string>("MobileNo"),
                            BillNo = dr.Field<string>("BillNo"),
                            Address = dr.Field<string>("Address"),
                            Date = dr.Field<string>("Date"),      // stored as string (e.g., "03-01-2018")
                            Amount = dr.Field<string>("Amount")     // stored as string (e.g., "1200.50")
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

    }
}
