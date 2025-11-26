using DAL.DataUtility;
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
  public  class FeePendingStatusReportDAL
    {

        #region
        static DataFunctions objDataFunctions = null;
        DataSet objDataSet = null;
        static string _commandText = string.Empty;
        Messages objMessages = null;
        #endregion
        public FeePendingStatusReportDAL()
        {
            objDataFunctions = new DataFunctions();
            objMessages = new Messages();
        }
        public List<FeePendingStatusReportMDL> GetFeePendingStatusReport(int FK_CompanyId, string ClassName, int RowPerpage, int CurrentPage, string SearchBy, string SearchValue)
        {

            List<FeePendingStatusReportMDL> _List = new List<FeePendingStatusReportMDL>();

            List<SqlParameter> parms = new List<SqlParameter>()
                {
                     new SqlParameter("@iCompanyId",FK_CompanyId),
                     new SqlParameter("@cClassName",ClassName),
                  new SqlParameter      ("@iRowPerpage",RowPerpage),
                   new SqlParameter     ("@iCurrentPage",CurrentPage),
                     new SqlParameter     ("@cSearchBy",SearchBy),
                       new SqlParameter     ("@cSearchValue",SearchValue),

                };

            try
            {

                _commandText = "[USP_GetAllFeePendingStatusDetails]";

                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        _List = objDataSet.Tables[1].AsEnumerable().Select(dr => new FeePendingStatusReportMDL()
                        {
                            StudentId = dr.Field<int?>("FK_StudentId") ?? 0,
                            FK_CompanyId = dr.Field<int?>("FK_CompanyId") ?? 0,
                            StudentName = dr.Field<string>("StudentName") ??   string.Empty,
                            FatherName = dr.Field<string>("FatherName") ?? string.Empty,
                            MobileNo = dr.Field<string>("MobileNo") ?? string.Empty,
                            ClassName = dr.Field<string>("ClassName") ?? string.Empty,
                            ClassCode = dr.Field<string>("ClassCode") ?? string.Empty,
                            Gender = dr.Field<string>("Gender") ?? string.Empty,
                            Address = dr.Field<string>("Address") ?? string.Empty,
                            //DueAmount = dr.Field<int>("DueAmount"),
                            MotherName = dr.Field<string>("MotherName") ?? string.Empty,
                            AprilFee = dr.Field<int>("AprilFee"),
                            AprilTrnsFee = dr.Field<int>("AprilTrnsFee"),
                            MayFee = dr.Field<int>("MayFee"),
                            MayTrnsFee = dr.Field<int>("MayTrnsFee"),
                            JuneFee = dr.Field<int>("JuneFee"),
                            JuneTrnsFee = dr.Field<int>("JuneTrnsFee"),
                            JulyFee = dr.Field<int>("JulyFee"),
                            JulyTrnsFee = dr.Field<int>("JulyTrnsFee"),
                            AugustFee = dr.Field<int>("AugustFee"),
                            AugustTrnsFee = dr.Field<int>("AugustTrnsFee"),
                            SeptemberFee = dr.Field<int>("SeptemberFee"),
                            SeptemberTrnsFee = dr.Field<int>("SeptemberTrnsFee"),
                            OctoberFee = dr.Field<int>("OctoberFee"),
                            OctoberTrnsFee = dr.Field<int>("OctoberTrnsFee"),
                            NovemberFee = dr.Field<int>("NovemberFee"),
                            NovemberTrnsFee = dr.Field<int>("NovemberTrnsFee"),
                            DecemberFee = dr.Field<int>("DecemberFee"),
                            DecemberTrnsFee = dr.Field<int>("DecemberTrnsFee"),
                            JanuaryFee = dr.Field<int>("JanuaryFee"),
                            JanuaryTrnsFee = dr.Field<int>("JanuaryTrnsFee"),
                            FebruaryFee = dr.Field<int>("FebruaryFee"),
                            FebruaryTrnsFee = dr.Field<int>("FebruaryTrnsFee"),
                            MarchFee = dr.Field<int>("MarchFee"),
                            MarchTrnsFee = dr.Field<int>("MarchTrnsFee"),
                            ApplicableMonthFee = dr.Field<int>("ApplicableMonthFee"),
                            ApplicableTrnsFee = dr.Field<int>("ApplicableTrnsFee"),
                            //YearlyExamFee = dr.Field<int>("YearlyExamFee"),
                            //HalfYearlyExamFee = dr.Field<int>("HalfYearlyExamFee"),
                            //PreDueAmount = dr.Field<int>("DueAmount"),
                            PaymentDate = dr.Field<string>("PaymentDate"),
                            AdmissionFee = dr.Field<int>("AdmissionFee"),
                            ExaminationFee1= dr.Field<int>("ExaminationFee1"),
                            ExaminationFee2= dr.Field<int>("ExaminationFee2"),
                            ApplicableMonth = dr.Field<string>("ApplicableMonth")

                        }).ToList();

                        objDataSet.Dispose();
                      
                    }
                    else
                    {
                       
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            return _List;
        }
    }
}
