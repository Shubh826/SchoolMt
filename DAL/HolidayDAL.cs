using DAL.DataUtility;
using MDL;
using MDL.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class HolidayDAL
    {
        #region
        static DataFunctions objDataFunctions = null;
        DataSet objDataSet = null;
        static string _commandText = string.Empty;
        Messages objMessages = null;
        #endregion
        public HolidayDAL()
        {
            objDataFunctions = new DataFunctions();
            objMessages = new Messages();
        }
        /// <summary>
        /// Get Holiday List
        /// </summary>
        /// <param name="_HolidayDetailsMDL"></param>
        /// <param name="PK_HolidayId"></param>
        /// <param name="SearchBy"></param>
        /// <param name="SearchValue"></param>
        /// <returns></returns>
        public bool GetHolidayDetails(out List<HolidayMDL> _HolidayDetailsMDL, int PK_HolidayId, string SearchBy, string SearchValue)
        {
            bool result = false;
            _HolidayDetailsMDL = new List<HolidayMDL>();

            List<SqlParameter> parms = new List<SqlParameter>()
                {
                     new SqlParameter("@iPK_holidayID",PK_HolidayId),
                     new SqlParameter("@cSearchBy",SearchBy),
                     new SqlParameter("@cSearchValue",SearchValue)

                };

            try
            {

                _commandText = "[USP_Get_HolidayList]";

                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        _HolidayDetailsMDL = objDataSet.Tables[1].AsEnumerable().Select(dr => new HolidayMDL()
                        {
                            PK_HolidayId = dr.Field<int>("PKHolidayId"),
                            FK_City_Id = dr.Field<int>("PK_CityId"),
                            Month = dr.Field<string>("Hmonth"),
                            HolidayDate = dr.Field<string>("Hdate"),
                            Day = dr.Field<string>("Hday"),
                            HolidayName = dr.Field<string>("Hname"),
                            CityName = dr.Field<string>("CityName"),
                            Statename = dr.Field<string>("StateName")

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
        /// <summary>
        /// Insert or update Holidays
        /// </summary>
        /// <param name="CityIds"></param>
        /// <param name="PK_HolidayId"></param>
        /// <param name="Holidaydate"></param>
        /// <param name="HolidayName"></param>
        /// <param name="CompanyId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public Messages AddEditHolidayDetails(string[] CityIds, string[] WeekDay, int PK_HolidayId = 0, string Holidaydate = "", string HolidayName = "", string HolidayType = "", int CompanyId = 0, int UserId = 0)
        {
            string WeekOffDay = "";
            string CityIDs = "";
            try
            {

                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[7] {
                new DataColumn("HolidayDate",typeof(string)),
                new DataColumn("HolidayName",typeof(string)),
                new DataColumn("FK_CityId", typeof(int)),
                new DataColumn("FK_CompanyId",typeof(int)),
                new DataColumn("CreatedBy",typeof(int)),
                new DataColumn("IsActive",typeof(Boolean)),
                new DataColumn("IsDeleted",typeof(Boolean)),

                });

                if (CityIds != null)
                {
                    foreach (var cityid in CityIds)
                    {
                        dt.Rows.Add(Holidaydate, HolidayName, Convert.ToInt32(cityid), CompanyId, UserId, true, false);
                    }
                }

                if (HolidayType != "Other")
                {
                    if (WeekDay != null)
                    {

                        WeekOffDay = string.Join(",", WeekDay);

                    }
                    if (CityIds != null)
                    {
                        CityIDs = string.Join(",", CityIds);
                    }

                }


                List<SqlParameter> parms = new List<SqlParameter>()
                {

                new SqlParameter("@iHolidayId",PK_HolidayId),
                new SqlParameter("@tblHolidayList",dt),
                new SqlParameter("@cCityIds",CityIDs),
                new SqlParameter("@HolidayType",HolidayType),
                new SqlParameter("@HolidayName",HolidayName),
                new SqlParameter("@iFKCompanyId",CompanyId),
                new SqlParameter("@Weekday",WeekOffDay),
                new SqlParameter("@iUserId",UserId)


                };

                _commandText = "[USP_AddEdit_HolidayList]";

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

        }/// <summary>
         /// Delete Holidays
         /// </summary>
         /// <param name="HolidayId"></param>
         /// <returns></returns>
        public Messages DeleteHoliday(int HolidayId)
        {

            List<SqlParameter> parms = new List<SqlParameter>()
                {

                     new SqlParameter("@iHolidayId",HolidayId)

                };

            _commandText = "[USP_DeleteHoliday]";

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
            return objMessages;

        }


    }
}
