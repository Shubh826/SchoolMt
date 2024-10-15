using DAL.DataUtility;
using MDL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDL.Common;

namespace DAL
{
    public class StudentFeeDetailsDAL
    {
        #region
        static DataFunctions objDataFunctions = null;
        System.Data.DataSet objDataSet = null;
        static string _commandText = string.Empty;
        #endregion

        public StudentFeeDetailsDAL()
        {
            objDataFunctions = new DataFunctions();
        }
        public Messages UploadValidStudentDataToDB(string jsondata, int UserId,int companyId)
        {
            Messages objMessages = new Messages();
            _commandText = "[SMS].[usp_BulkUploadStudentFeeData]";
            List<SqlParameter> parms = new List<SqlParameter>
               {
                 new SqlParameter("@jsondata",jsondata),
                 new SqlParameter("@iuserId",UserId),
                 new SqlParameter("@icompanyId",companyId)
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

        public bool GetStudentFeeData(out List<StudentFeeDetailsMDL> objStudentList, out BasicPagingMDL objBasicPagingMDL, int id, int rowPerpage, int currentPage, int FK_CompanyId, string SearchBy, string SearchValue)
        {
            objStudentList = new List<StudentFeeDetailsMDL>();
            objBasicPagingMDL = new BasicPagingMDL();
            bool result = false;
            Messages objMessages = new Messages();
            _commandText = "USP_GetStudentFeesDtails";
            List<SqlParameter> parms = new List<SqlParameter>
               {
                    new SqlParameter("@iRowperPage",rowPerpage),
                    new SqlParameter("@iCurrentPage",currentPage),
                    new SqlParameter("@Fk_CompanyId",FK_CompanyId),
                    new SqlParameter("@SearchBy",SearchBy),
                    new SqlParameter("@SearchValue",SearchValue),
                    new SqlParameter("@PK_StudentFeeDtlId",id)
              };
            try
            {
                CheckParameters.ConvertNullToDBNull(parms);
                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        objStudentList = objDataSet.Tables[1].AsEnumerable().Select(dr => new StudentFeeDetailsMDL()
                        {
                            CompanyId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_CompanyId")),
                            StudentName = dr.Field<string>("StudentName"),
                            FatherName = dr.Field<string>("FatherName"),
                            MobileNo = dr.Field<string>("MobileNo"), 
                            ClassName = dr.Field<string>("ClassName"),
                            Address = dr.Field<string>("Address"),
                            AdmissionFee = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("AdmissionFee")),
                            AdmissionDate = dr.Field<string>("AdmissionDate"), 
                            AprilFee = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("AprilFee")),
                            AprilTrnsFee = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("AprilTrnsFee")), 
                            MayFee = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("MayFee")), 
                            MayTrnsFee = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("MayTrnsFee")),
                            JuneFee = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("JuneFee")), 
                            JuneTrnsFee = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("JuneTrnsFee")), 
                            JulyFee = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("JulyFee")), 
                            JulyTrnsFee = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("JulyTrnsFee")), 
                            AugustFee = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("AugustFee")),
                            AugustTrnsFee = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("AugustTrnsFee")), 
                            SeptemberFee = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("SeptemberFee")), 
                            SeptemberTrnsFee = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("SeptemberTrnsFee")), 
                            OctoberFee = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("OctoberFee")), 
                            OctoberTrnsFee = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("OctoberTrnsFee")),
                            NovemberFee = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("NovemberFee")), 
                            NovemberTrnsFee = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("NovemberTrnsFee")), 
                            DecemberFee = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("DecemberFee")), 
                            DecemberTrnsFee = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("DecemberTrnsFee")),
                            JanuaryFee = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("JanuaryFee")),
                            JanuaryTrnsFee = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("JanuaryTrnsFee")),
                            FebruaryFee = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FebruaryFee")),
                            FebruaryTrnsFee = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FebruaryTrnsFee")), 
                            MarchFee = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("MarchFee")), 
                            MarchTrnsFee = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("MarchTrnsFee")),
                            ExaminationFee1 = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("ExaminationFee1")), 
                            ExaminationFee2 = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("ExaminationFee2")), 
                            PreviousDueAmount = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PreviousDueAmount")), 
                            IsActive = dr.Field<bool>("IsActive"),
                            CreatedBy = dr.Field<int>("CreatedBy"),
                            ApplicableMonthFee = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("ApplicableMonthFee")),
                            ApplicableTrnsFee = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("ApplicableTrnsFee")), 
                            ApplicableMonth = dr.Field<string>("ApplicableMonth"),
                            AcademicSession = dr.Field<string>("AcademicSession") 
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
