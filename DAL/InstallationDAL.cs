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
    public class InstallationDAL
    {
        #region
        static DataFunctions objDataFunctions = null;
        DataSet objDataSet = null;
        static string _commandText = string.Empty;
        #endregion

        public InstallationDAL()
        {
            objDataFunctions = new DataFunctions();
        }
        public Messages AddEdit_MapMachineDeviceSyncStatus(int MachineId, bool IsSync)
        {
            Messages objMessages = new Messages();
            _commandText = "AddEdit_MapMachineDeviceSyncStatus";
            List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter("@iMachineId",MachineId),
                    new SqlParameter("@bIsSync", IsSync),

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
            }
            catch (Exception ex)
            {
                objMessages.Message_Id = 0;
                objMessages.Message = "Failed";
            }
            return objMessages;
        }

    }
}
