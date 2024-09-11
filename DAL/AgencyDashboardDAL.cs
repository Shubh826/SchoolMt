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
   public class AgencyDashboardDAL
    {
        #region
        static DataFunctions objDataFunctions = null;
        DataSet objDataSet = null;
        static string _commandText = string.Empty;
        #endregion

        public AgencyDashboardDAL()
        {
            objDataFunctions = new DataFunctions();
        }
        public bool getDashboardData(out AgencyDashboardMDL objmdl, out List<JoiningTotalJobs> objtable, out List<TotalOpenJobs> objopenjobs, out List<LocationWiseOpen> objLocationWise, int fk_companyid, int userid, int clientId)
        {
            bool result = false;
            //objBasicPagingMDL = new BasicPagingMDL();
            objtable = new List<JoiningTotalJobs>();
            objopenjobs = new List<TotalOpenJobs>();
            objLocationWise = new List<LocationWiseOpen>();
            objmdl = new AgencyDashboardMDL();
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>()
                {

                    new SqlParameter("@iCompanyId",fk_companyid),
                    new SqlParameter("@iUserId",userid),
                    new SqlParameter("@iClientId",clientId),
                };
                _commandText = "Dbo.Usp_GetAgencyDashboardData";

                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        objtable = objDataSet.Tables[1].AsEnumerable().Select(dr => new JoiningTotalJobs()
                        {
                            CountOnbtime = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("Countonbtime")),
                            Onbtime = dr.Field<string>("onbTime"),
                        }).ToList();
                        objDataSet.Dispose();
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        objopenjobs = objDataSet.Tables[2].AsEnumerable().Select(dr => new TotalOpenJobs()
                        {
                            CountOpenJob = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("CountJobType"))
                        }).ToList();
                        objDataSet.Dispose();
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        objLocationWise = objDataSet.Tables[3].AsEnumerable().Select(dr => new LocationWiseOpen()
                        {
                            CountLocation = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("CountLocations")),
                            Location = dr.Field<string>("Locations"),
                        }).ToList();
                        objDataSet.Dispose();
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        objmdl = new AgencyDashboardMDL()
                        {
                            TotalJobs = WrapDbNull.WrapDbNullValue<int>(objDataSet.Tables[4].Rows[0].Field<int?>("CountJob")),
                            TotalProfile = WrapDbNull.WrapDbNullValue<int>(objDataSet.Tables[5].Rows[0].Field<int?>("CountProfile")),
                            TotalShedInterview = WrapDbNull.WrapDbNullValue<int>(objDataSet.Tables[6].Rows[0].Field<int?>("CountSchedule")),
                            TotalOnboard = WrapDbNull.WrapDbNullValue<int>(objDataSet.Tables[7].Rows[0].Field<int?>("TotalShortlisted"))
                        };
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
