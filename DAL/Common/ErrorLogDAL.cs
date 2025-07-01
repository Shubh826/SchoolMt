using DAL.DataUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Common
{
   public class ErrorLogDAL
    {

        DataFunctions objDataFunctions = null;
        string commandText = string.Empty;
        public ErrorLogDAL()
        {
            objDataFunctions = new DataFunctions();
        }
        public void SetError(string Source, string AssemblyName, string ClassName, string MethodName, string ErrorMessage, string Remarks)
        {
            try
            {
                commandText = "[dbo].[usp_LogServiceError]";
                List<SqlParameter> paramList = new List<SqlParameter>();

                SqlParameter objSqlParameter = new SqlParameter("@cSource", SqlDbType.VarChar);
                objSqlParameter.Value = Source;
                paramList.Add(objSqlParameter);

                objSqlParameter = new SqlParameter("@cAssemblyName", SqlDbType.VarChar);
                objSqlParameter.Value = AssemblyName;
                paramList.Add(objSqlParameter);

                objSqlParameter = new SqlParameter("@cClassName", SqlDbType.VarChar);
                objSqlParameter.Value = ClassName;
                paramList.Add(objSqlParameter);

                objSqlParameter = new SqlParameter("@cMethodName", SqlDbType.VarChar);
                objSqlParameter.Value = MethodName;
                paramList.Add(objSqlParameter);

                objSqlParameter = new SqlParameter("@cErrorMessage", SqlDbType.VarChar);
                objSqlParameter.Value = ErrorMessage;
                paramList.Add(objSqlParameter);


                objSqlParameter = new SqlParameter("@cRemarks", SqlDbType.VarChar);
                objSqlParameter.Value = Remarks;
                paramList.Add(objSqlParameter);

                objDataFunctions.executeCommand(commandText, paramList);


            }
            catch { }

        }


        public void SetError(string name, string controller, string action, string Source, string message, string type, string Remarks)
        {
            try
            {
                commandText = "[dbo].[usp_LogApplicationError]";
                List<SqlParameter> paramList = new List<SqlParameter>();

                SqlParameter objSqlParameter = new SqlParameter("@cSource", SqlDbType.VarChar);
                objSqlParameter.Value = Source;
                paramList.Add(objSqlParameter);

                objSqlParameter = new SqlParameter("@cAssemblyName", SqlDbType.VarChar);
                objSqlParameter.Value = name;
                paramList.Add(objSqlParameter);

                objSqlParameter = new SqlParameter("@cClassName", SqlDbType.VarChar);
                objSqlParameter.Value = controller;
                paramList.Add(objSqlParameter);

                objSqlParameter = new SqlParameter("@cMethodName", SqlDbType.VarChar);
                objSqlParameter.Value = action;
                paramList.Add(objSqlParameter);

                objSqlParameter = new SqlParameter("@cErrorMessage", SqlDbType.VarChar);
                objSqlParameter.Value = message;
                paramList.Add(objSqlParameter);

                objSqlParameter = new SqlParameter("@cErrorType", SqlDbType.VarChar);
                objSqlParameter.Value = type;
                paramList.Add(objSqlParameter);


                objSqlParameter = new SqlParameter("@cRemarks", SqlDbType.VarChar);
                objSqlParameter.Value = Remarks;
                paramList.Add(objSqlParameter);

                objDataFunctions.executeCommand(commandText, paramList);


            }
            catch { }
            // throw new NotImplementedException();
        }
    }
}
