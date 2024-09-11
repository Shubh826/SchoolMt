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
    }
}
