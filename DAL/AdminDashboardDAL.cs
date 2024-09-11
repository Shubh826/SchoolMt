using DAL.DataUtility;
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
    public class AdminDashboardDAL
    {
        #region
        static DataFunctions objDataFunctions = null;
        DataSet objDataSet = null;
        static string _commandText = string.Empty;
        #endregion

        public AdminDashboardDAL()
        {
            objDataFunctions = new DataFunctions();
        }
        public bool GetAdminDashboardData(out AdminDashboardMDL objmdl, int fk_companyid, int userid, int clientId)
        {
            bool result = false;
            objmdl = new AdminDashboardMDL();
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>()
                {

                    new SqlParameter("@iCompanyId",fk_companyid),
                    new SqlParameter("@iUserId",userid),
                    new SqlParameter("@iClientId",clientId),
                };
                _commandText = "Dbo.Usp_GetAdminDashboardData";

                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    //if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    //{
                    //    objtotalSkillsProfile = objDataSet.Tables[1].AsEnumerable().Select(dr => new TotalSkillsWiseProfile()
                    //    {
                    //        CountSkills = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("CountSkills")),
                    //        Skills = dr.Field<string>("Skills"),
                    //    }).ToList();
                    //    objDataSet.Dispose();
                    //    result = true;
                    //}
                    //else
                    //{
                    //    result = false;
                    //}

                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        objmdl = new AdminDashboardMDL()
                        {
                            TotalClient = WrapDbNull.WrapDbNullValue<int>(objDataSet.Tables[1].Rows[0].Field<int?>("TotalClient")),
                            TotalAgencies = WrapDbNull.WrapDbNullValue<int>(objDataSet.Tables[2].Rows[0].Field<int?>("TotalAgencies")),
                            TotalActiveProfiles = WrapDbNull.WrapDbNullValue<int>(objDataSet.Tables[3].Rows[0].Field<int?>("TotalActiveProfiles")),
                            TotalClosedJobs = WrapDbNull.WrapDbNullValue<int>(objDataSet.Tables[4].Rows[0].Field<int?>("TotalClosedJobs")),
                            CurrentScheduledInterviews = WrapDbNull.WrapDbNullValue<int>(objDataSet.Tables[5].Rows[0].Field<int?>("CurrentScheduledInterviews")),
                            TotalShortlistedProfiles = WrapDbNull.WrapDbNullValue<int>(objDataSet.Tables[6].Rows[0].Field<int?>("TotalShortlistedProfiles")),
                            OpenJobs = WrapDbNull.WrapDbNullValue<int>(objDataSet.Tables[7].Rows[0].Field<int?>("OpenJobs")),
                            ActiveAgenciesorClients = WrapDbNull.WrapDbNullValue<int>(objDataSet.Tables[8].Rows[0].Field<int?>("ActiveAgenciesorClients"))
                        };
                    }

                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }


        public bool GetMonthWiseAdminDashboardData(out List<MonthwiseData> _MonthwiseData ,
            int Year, int fk_companyid, int userid, int clientId)
        {
            bool result = false;
            _MonthwiseData = new List<MonthwiseData>();
            //Monthlist = new List<Monthlist>();
            //Clientlist = new List<Clientlist>();
            //Agencylist = new List<Agencylist>();
            //Profilelist = new List<Profilelist>();
            //Joblist = new List<Joblist>();
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>()
                {

                    new SqlParameter("@iCompanyId",fk_companyid),
                    new SqlParameter("@iUserId",userid),
                    new SqlParameter("@iclientId",clientId),
                    new SqlParameter("@iYear",Year),
                };

                _commandText = "dbo.GetMonthwiseDataForAdminDashboard";
                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables.Count > 0)
                {
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_ID") == 1)
                    {
                        _MonthwiseData = objDataSet.Tables[1].AsEnumerable().Select(dr => new MonthwiseData()
                        {
                            Month = dr.Field<string>("MonthName"),
                            Agency = dr.Field<int>("Agency"),
                            ActiveProfile = dr.Field<int>("ActiveProfile"),
                            Job = dr.Field<int>("Jobs"),
                            Client = dr.Field<int>("Client"),

                        }).ToList();
                        //Monthlist = objDataSet.Tables[1].AsEnumerable().Select(dr => new Monthlist()
                        //{
                        //    Month = dr.Field<string>("MonthName"),

                        //}).ToList();
                        //Clientlist = objDataSet.Tables[1].AsEnumerable().Select(dr => new Clientlist()
                        //{
                        //    Client = dr.Field<int>("Client"),

                        //}).ToList();
                        //Agencylist = objDataSet.Tables[1].AsEnumerable().Select(dr => new Agencylist()
                        //{
                        //    Agency = dr.Field<int>("Agency"),

                        //}).ToList();
                        //Profilelist = objDataSet.Tables[1].AsEnumerable().Select(dr => new Profilelist()
                        //{
                        //    ActiveProfile = dr.Field<int>("ActiveProfile"),

                        //}).ToList();
                        //Joblist = objDataSet.Tables[1].AsEnumerable().Select(dr => new Joblist()
                        //{
                        //    Job = dr.Field<int>("Jobs"),

                        //}).ToList();


                        objDataSet.Dispose();
                        result = true;
                    }
                    else
                    {
                        result = false;
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
