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



                            StudentId = dr.Field<int>("FK_StudentId"),
                            CompanyId = dr.Field<int>("FK_CompanyId"),
                            StudentName = dr.Field<string>("StudentName"),
                            FatherName = dr.Field<string>("FatherName"),
                            MobileNo = dr.Field<string>("MobileNo"),
                            ClassName = dr.Field<string>("ClassName"),
                            Address = dr.Field<string>("Address"),
                            AdmissionFee = dr.Field<int>("AdmissionFee"),
                            AdmissionDate = dr.Field<DateTime>("AdmissionDate").ToString(),
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
                            ExaminationFee1 = dr.Field<int>("ExaminationFee1"),
                            ExaminationFee2 = dr.Field<int>("ExaminationFee2"),
                            PreviousDueAmount = dr.Field<int>("PreviousDueAmount"),
                            IsActive = dr.Field<bool>("IsActive"),
                            IsDeleted = dr.Field<bool>("IsDeleted"),
                            CreatedBy = dr.Field<int>("CreatedBy"),  
                            ApplicableMonthFee =  dr.Field<int>("ApplicableMonthFee"),
                            ApplicableTrnsFee = dr.Field<int>("ApplicableTrnsFee"),
      
                            ApplicableMonth =dr.Field<string>("ApplicableMonth"),
      
                            AcademicSession =dr.Field<string>("AcademicSession"),


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
