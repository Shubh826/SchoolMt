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
    public class ClientDashboardDAL
    {
        #region
        static DataFunctions objDataFunctions = null;
        DataSet objDataSet = null;
        static string _commandText = string.Empty;
        #endregion

        public ClientDashboardDAL()
        {
            objDataFunctions = new DataFunctions();
        }
        public bool getClientDashboardData(out ClientDashboardMDL objmdl, out List<TotalSkillsWiseProfile> objtotalSkillsProfile, out List<JoiningTotalProfile> objtotalProfile, out List<LocationWiseProfile> objLocationWise, int fk_companyid, int userid, int clientId)
        {
            bool result = false;
            objtotalSkillsProfile = new List<TotalSkillsWiseProfile>();
            objtotalProfile = new List<JoiningTotalProfile>();
            objLocationWise = new List<LocationWiseProfile>();
            objmdl = new ClientDashboardMDL();
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>()
                {

                    new SqlParameter("@iCompanyId",fk_companyid),
                    new SqlParameter("@iUserId",userid),
                    new SqlParameter("@iClientId",clientId),
                };
                _commandText = "Dbo.Usp_GetClientDashboardData";

                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        objtotalSkillsProfile = objDataSet.Tables[1].AsEnumerable().Select(dr => new TotalSkillsWiseProfile()
                        {
                            CountSkills = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("CountJobType")),
                            //Skills = dr.Field<string>("JobType"),
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
                        objtotalProfile = objDataSet.Tables[2].AsEnumerable().Select(dr => new JoiningTotalProfile()
                        {
                            CountAvailability = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("CountAvailability")),
                            AvailabilityDuration = dr.Field<string>("AvailabilityDuration")
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
                        objLocationWise = objDataSet.Tables[3].AsEnumerable().Select(dr => new LocationWiseProfile()
                        {
                            CountLocation = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("CountLocations")),
                            Location = dr.Field<string>("Location"),
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
                        objmdl = new ClientDashboardMDL()
                        {
                            TotalAvbProfile = WrapDbNull.WrapDbNullValue<int>(objDataSet.Tables[4].Rows[0].Field<int?>("CountProfile")),
                            ShortlistedProfile = WrapDbNull.WrapDbNullValue<int>(objDataSet.Tables[5].Rows[0].Field<int?>("CountShortListed")),
                            TotalShedInterview = WrapDbNull.WrapDbNullValue<int>(objDataSet.Tables[6].Rows[0].Field<int?>("CountSchedule")),
                            TotalShortlisted = WrapDbNull.WrapDbNullValue<int>(objDataSet.Tables[7].Rows[0].Field<int?>("TotalShortlisted")),
                            selectedProfile = WrapDbNull.WrapDbNullValue<int>(objDataSet.Tables[8].Rows[0].Field<int?>("Totalselected"))
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

        public DashboardMDL GetDashBoardData(string fromDate, string toDate)
        {
            DashboardMDL objDashboardMDL = new DashboardMDL();
            DataSet objDataSet = null;

            try
            {
                List<SqlParameter> parms = new List<SqlParameter>()
            {
                new SqlParameter("@cFromDate", fromDate),
                new SqlParameter("@cToDate", toDate)
            };

                _commandText = "[usp_GetDashboardData]";
                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);

                if (objDataSet != null && objDataSet.Tables.Count > 1 && objDataSet.Tables[0].Rows.Count > 0)
                {
                    var messageId = Convert.ToInt32(objDataSet.Tables[0].Rows[0]["Message_Id"]);
                    if (messageId == 1)
                    {
                        DataRow row = objDataSet.Tables[1].Rows[0];

                        objDashboardMDL = new DashboardMDL
                        {
                            TotalFee = row["TotalFee"]?.ToString() ?? "0",
                            TotalCollectedFee = row["TotalCollectedFee"]?.ToString() ?? "0",
                            TotalPendingFee = row["TotalPendingFee"]?.ToString() ?? "0",
                            TotalExpense = row["TotalExpense"]?.ToString() ?? "0",
                            TotalBookCollection = row["TotalBookCollection"]?.ToString() ?? "0",
                            TotalNoteBookCollection = row["TotalNoteBookCollection"]?.ToString() ?? "0",
                            TotalStudent = row["TotalStudent"]?.ToString() ?? "0",
                            ChartJsonData = row["ChartJsonData"]?.ToString() ?? ""
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                // Optionally log the exception
            }
            finally
            {
                objDataSet?.Dispose();
            }

            return objDashboardMDL;
        }

    }
}
