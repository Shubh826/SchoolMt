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
    public class FeeBillDAL
    {
        static DataFunctions objDataFunctions = null;
        System.Data.DataSet objDataSet = null;
        static string _commandText = string.Empty;

        public FeeBillDAL()
        {
            objDataFunctions = new DataFunctions();
        }
        public bool GetFeeBillData(out List<FeeBillMDL> objFeeBillList, out BasicPagingMDL objBasicPagingMDL, int id, int rowPerpage, int currentPage, int FK_CompanyId, string SearchBy, string SearchValue, string ClassName = "", string Section = "")
        {
            objFeeBillList = new List<FeeBillMDL>();
            objBasicPagingMDL = new BasicPagingMDL();
            bool result = false;
            Messages objMessages = new Messages();
            _commandText = "[SMS].[usp_GetFeeBillData]";
            List<SqlParameter> parms = new List<SqlParameter>
               {
                    new SqlParameter("@iRowperPage",rowPerpage),
                    new SqlParameter("@iCurrentPage",currentPage),
                    new SqlParameter("@Fk_CompanyId",FK_CompanyId),
                    new SqlParameter("@SearchBy",SearchBy),
                    new SqlParameter("@SearchValue",SearchValue),
                    new SqlParameter("@iPK_BillId",id),
                    new SqlParameter("@ClassName",ClassName),
                    new SqlParameter("@Section",Section)
              };
            try
            {
                CheckParameters.ConvertNullToDBNull(parms);
                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        objFeeBillList = objDataSet.Tables[1].AsEnumerable().Select(dr => new FeeBillMDL()
                        {
                            PK_BillId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_StudentId")),
                            PK_StudentFeeDetId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_StudentFeeDtlId")),
                            StudentName = dr.Field<string>("StudentName"),
                            FatherName = dr.Field<string>("FatherName"),
                            Address = dr.Field<string>("StudentAddress"),
                            ClassName = dr.Field<string>("ClassName"),
                            MobileNumber = dr.Field<string>("MobileNumber"),
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
                            YearlyExamFee = dr.Field<int>("YearlyExamFee"),
                            HalfYearlyExamFee = dr.Field<int>("HalfYearlyExamFee"),
                            PreDueAmount = dr.Field<int>("DueAmount"),
                            PaymentDate = dr.Field<string>("PaymentDate"),
                            AdmissionFee = dr.Field<int>("AdmissionFee")
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
        public Messages AddEditFeeBill(FeeBillMDL objFeeBillMDL,out PaymentDetails _PaymentDetails)
        {
            _PaymentDetails = new PaymentDetails();
            Messages objMessages = new Messages();
            _commandText = "[usp_InsertUpdateFeeBill]";
            List<SqlParameter> parms = new List<SqlParameter>
            {
                new SqlParameter("@PK_StudentFeeDetId", objFeeBillMDL.PK_StudentFeeDetId), // Assuming this is the unique identifier
                new SqlParameter("@PK_BillId", objFeeBillMDL.PK_BillId), // Assuming this is the unique identifier
                new SqlParameter("@Fk_CompanyId", objFeeBillMDL.FK_CompanyId),
                new SqlParameter("@StudentName", objFeeBillMDL.StudentName),
                new SqlParameter("@ClassName", objFeeBillMDL.ClassName),
                new SqlParameter("@ClassCode", objFeeBillMDL.ClassCode),
                new SqlParameter("@FatherName", objFeeBillMDL.FatherName),
                new SqlParameter("@Discount", objFeeBillMDL.Discount),
                new SqlParameter("@Address", objFeeBillMDL.Address),
                new SqlParameter("@CreatedBy", objFeeBillMDL.CreatedBy),
                new SqlParameter("@AprilFee", objFeeBillMDL.AprilFee),
                new SqlParameter("@AprilTrnsFee", objFeeBillMDL.AprilTrnsFee),
                new SqlParameter("@MayFee", objFeeBillMDL.MayFee),
                new SqlParameter("@MayTrnsFee", objFeeBillMDL.MayTrnsFee),
                new SqlParameter("@JuneFee", objFeeBillMDL.JuneFee),
                new SqlParameter("@JuneTrnsFee", objFeeBillMDL.JuneTrnsFee),
                new SqlParameter("@JulyFee", objFeeBillMDL.JulyFee),
                new SqlParameter("@JulyTrnsFee", objFeeBillMDL.JulyTrnsFee),
                new SqlParameter("@AugustFee", objFeeBillMDL.AugustFee),
                new SqlParameter("@AugustTrnsFee", objFeeBillMDL.AugustTrnsFee),
                new SqlParameter("@SeptemberFee", objFeeBillMDL.SeptemberFee),
                new SqlParameter("@SeptemberTrnsFee", objFeeBillMDL.SeptemberTrnsFee),
                new SqlParameter("@OctoberFee", objFeeBillMDL.OctoberFee),
                new SqlParameter("@OctoberTrnsFee", objFeeBillMDL.OctoberTrnsFee),
                new SqlParameter("@NovemberFee", objFeeBillMDL.NovemberFee),
                new SqlParameter("@NovemberTrnsFee", objFeeBillMDL.NovemberTrnsFee),
                new SqlParameter("@DecemberFee", objFeeBillMDL.DecemberFee),
                new SqlParameter("@DecemberTrnsFee", objFeeBillMDL.DecemberTrnsFee),
                new SqlParameter("@JanuaryFee", objFeeBillMDL.JanuaryFee),
                new SqlParameter("@JanuaryTrnsFee", objFeeBillMDL.JanuaryTrnsFee),
                new SqlParameter("@FebruaryFee", objFeeBillMDL.FebruaryFee),
                new SqlParameter("@FebruaryTrnsFee", objFeeBillMDL.FebruaryTrnsFee),
                new SqlParameter("@MarchFee", objFeeBillMDL.MarchFee),
                new SqlParameter("@MarchTrnsFee", objFeeBillMDL.MarchTrnsFee),
                new SqlParameter("@HalfYearlyExamFee", objFeeBillMDL.HalfYearlyExamFee),
                new SqlParameter("@YearlyExamFee", objFeeBillMDL.YearlyExamFee),
                new SqlParameter("@Totalfee", objFeeBillMDL.Totalfee),
                new SqlParameter("@payablefee", objFeeBillMDL.payablefee),
                new SqlParameter("@Cash", objFeeBillMDL.Cash),
                new SqlParameter("@Online", objFeeBillMDL.Online),
                new SqlParameter("@ApplicableMonthFee", objFeeBillMDL.ApplicableMonthFee),
                new SqlParameter("@ApplicableTrnsFee", objFeeBillMDL.ApplicableTrnsFee),
                new SqlParameter("@Months", objFeeBillMDL.Months),
                new SqlParameter("@DueAmount", objFeeBillMDL.DueAmount),
                new SqlParameter("@PreDueAmount", objFeeBillMDL.PreDueAmount),
                new SqlParameter("@predueamounchecked",objFeeBillMDL.predueamounchecked),
                new SqlParameter("@PaymentDate",objFeeBillMDL.PaymentDate),
                new SqlParameter("@AdmissionFee",objFeeBillMDL.AdmissionFee)

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
                if (objDataSet.Tables[1].Rows.Count > 0)
                {
                    _PaymentDetails.PK_BillId = WrapDbNull.WrapDbNullValue<int>(objDataSet.Tables[1].Rows[0].Field<int?>("PK_BillId"));
                    _PaymentDetails.StudentName = objDataSet.Tables[1].Rows[0].Field<string>("StudentName");
                    _PaymentDetails.PaymentDate = objDataSet.Tables[1].Rows[0].Field<string>("PaymentDate");
                    _PaymentDetails.FatherName = objDataSet.Tables[1].Rows[0].Field<string>("FatherName");
                    _PaymentDetails.ClassName = objDataSet.Tables[1].Rows[0].Field<string>("ClassName");
                    _PaymentDetails.BillNo = objDataSet.Tables[1].Rows[0].Field<string>("BillNo");
                    //_PaymentDetails.PreDue = WrapDbNull.WrapDbNullValue<int>(objDataSet.Tables[1].Rows[0].Field<int?>("PreDue"));
                    _PaymentDetails.Months = objDataSet.Tables[1].Rows[0].Field<string>("Months");
                    _PaymentDetails.MonthFee = WrapDbNull.WrapDbNullValue<int>(objDataSet.Tables[1].Rows[0].Field<int?>("MonthFee"));
                    _PaymentDetails.TransFee = WrapDbNull.WrapDbNullValue<int>(objDataSet.Tables[1].Rows[0].Field<int?>("TransFee"));
                    _PaymentDetails.ExamFee = WrapDbNull.WrapDbNullValue<int>(objDataSet.Tables[1].Rows[0].Field<int?>("ExamFee"));
                    _PaymentDetails.DueAmount = WrapDbNull.WrapDbNullValue<int>(objDataSet.Tables[1].Rows[0].Field<int?>("DueAmount"));
                    _PaymentDetails.Discount = WrapDbNull.WrapDbNullValue<int>(objDataSet.Tables[1].Rows[0].Field<int?>("Discount"));
                    _PaymentDetails.TotalFee = WrapDbNull.WrapDbNullValue<int>(objDataSet.Tables[1].Rows[0].Field<int?>("TotalFee"));
                    _PaymentDetails.PaymentDate = objDataSet.Tables[1].Rows[0].Field<string>("PaymentDate");
                    _PaymentDetails.Addfee = WrapDbNull.WrapDbNullValue<int>(objDataSet.Tables[1].Rows[0].Field<int?>("AdmissionFee"));


                }
            }
            catch (Exception ex)
            {
                objMessages.Message_Id = 0;
                objMessages.Message = "Failed: " + ex.Message; // Optional: Log the exception message for debugging
            }

            return objMessages;
        }

    }
}
