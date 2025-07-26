using DAL.DataUtility;
using MDL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace DAL
{
    public class GenrateDmdBillDAL
    {
        #region
        static DataFunctions objDataFunctions = null;
        DataSet objDataSet = null;
        static string _commandText = string.Empty;
        Messages objMessages = null;
        #endregion
        public GenrateDmdBillDAL()
        {
            objDataFunctions = new DataFunctions();
            objMessages = new Messages();
        }
        /// <summary>
        /// Get Fee List
        /// </summary>
        /// <param name="_feemdlMDL"></param>
        /// <param name="PK_HolidayId"></param>
        /// <param name="SearchBy"></param>
        /// <param name="SearchValue"></param>
        /// <returns></returns>
        public bool getFeedetails(out List<StudentFeeDetailsMDL> _StudentFeeDetailsMDL, GenrateDmdbillMdl obmdl)
        {
            bool result = false;
            _StudentFeeDetailsMDL = new List<StudentFeeDetailsMDL>();

            List<SqlParameter> parms = new List<SqlParameter>()
                {
                     new SqlParameter("@iCompanyId",obmdl.FK_CompanyId),
                     new SqlParameter("@cClassName",obmdl.ClassName),

                };

            try
            {

                _commandText = "[USP_GetAllfeedetails]";

                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        _StudentFeeDetailsMDL = objDataSet.Tables[1].AsEnumerable().Select(dr => new StudentFeeDetailsMDL()
                        {
                            StudentId = dr.Field<int?>("FK_StudentId") ?? 0,
                            CompanyId = dr.Field<int?>("FK_CompanyId") ?? 0,
                            StudentName = dr.Field<string>("StudentName") ?? string.Empty,
                            FatherName = dr.Field<string>("FatherName") ?? string.Empty,
                            MobileNo = dr.Field<string>("MobileNo") ?? string.Empty,
                            ClassName = dr.Field<string>("ClassName") ?? string.Empty,
                            Address = dr.Field<string>("Address") ?? string.Empty,
                            AdmissionFee = dr.Field<int?>("AdmissionFee") ?? 0,
                            AdmissionDate = dr["AdmissionDate"] != DBNull.Value ? Convert.ToDateTime(dr["AdmissionDate"]).ToString("yyyy-MM-dd") : string.Empty,

                            AprilFee = dr.Field<int?>("AprilFee") ?? 0,
                            AprilTrnsFee = dr.Field<int?>("AprilTrnsFee") ?? 0,
                            MayFee = dr.Field<int?>("MayFee") ?? 0,
                            MayTrnsFee = dr.Field<int?>("MayTrnsFee") ?? 0,
                            JuneFee = dr.Field<int?>("JuneFee") ?? 0,
                            JuneTrnsFee = dr.Field<int?>("JuneTrnsFee") ?? 0,
                            JulyFee = dr.Field<int?>("JulyFee") ?? 0,
                            JulyTrnsFee = dr.Field<int?>("JulyTrnsFee") ?? 0,
                            AugustFee = dr.Field<int?>("AugustFee") ?? 0,
                            AugustTrnsFee = dr.Field<int?>("AugustTrnsFee") ?? 0,
                            SeptemberFee = dr.Field<int?>("SeptemberFee") ?? 0,
                            SeptemberTrnsFee = dr.Field<int?>("SeptemberTrnsFee") ?? 0,
                            OctoberFee = dr.Field<int?>("OctoberFee") ?? 0,
                            OctoberTrnsFee = dr.Field<int?>("OctoberTrnsFee") ?? 0,
                            NovemberFee = dr.Field<int?>("NovemberFee") ?? 0,
                            NovemberTrnsFee = dr.Field<int?>("NovemberTrnsFee") ?? 0,
                            DecemberFee = dr.Field<int?>("DecemberFee") ?? 0,
                            DecemberTrnsFee = dr.Field<int?>("DecemberTrnsFee") ?? 0,
                            JanuaryFee = dr.Field<int?>("JanuaryFee") ?? 0,
                            JanuaryTrnsFee = dr.Field<int?>("JanuaryTrnsFee") ?? 0,
                            FebruaryFee = dr.Field<int?>("FebruaryFee") ?? 0,
                            FebruaryTrnsFee = dr.Field<int?>("FebruaryTrnsFee") ?? 0,
                            MarchFee = dr.Field<int?>("MarchFee") ?? 0,
                            MarchTrnsFee = dr.Field<int?>("MarchTrnsFee") ?? 0,

                            ExaminationFee1 = dr.Field<int?>("ExaminationFee1") ?? 0,
                            ExaminationFee2 = dr.Field<int?>("ExaminationFee2") ?? 0,
                            PreviousDueAmount = dr.Field<int?>("PreviousDueAmount") ?? 0,
                            IsActive = dr.Field<bool?>("IsActive") ?? false,
                            IsDeleted = dr.Field<bool?>("IsDeleted") ?? false,
                            CreatedBy = dr.Field<int?>("CreatedBy") ?? 0,

                            ApplicableMonthFee = dr.Field<int?>("ApplicableMonthFee") ?? 0,
                            ApplicableTrnsFee = dr.Field<int?>("ApplicableTrnsFee") ?? 0,
                            ApplicableMonth = dr.Field<string>("ApplicableMonth") ?? string.Empty,
                            AcademicSession = dr.Field<string>("AcademicSession") ?? string.Empty



                        }).ToList();

                        objDataSet.Dispose();
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
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
