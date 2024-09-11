using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL.DataUtility
{
    /// <summary>
    /// Represents a set of methods use for DML operations in database.
    /// </summary>
    public class DataFunctions
    {
        Command objClsCommand;
        SqlCommand objSqlCommand;
        SqlDataAdapter objSqlDataAdapter;
        DataSet objdataSet;
        object returnValue;

        /// <summary>
        /// Return sql query result.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="returnType"></param>
        /// <returns></returns>
        public object getQueryResult(string commandText, DataReturnType returnType, int ConnectionTimeout = 50)
        {
            try
            {
                objClsCommand = new Command();
                objdataSet = new DataSet();
                objSqlCommand = objClsCommand.getCommand;
                objSqlCommand.CommandText = commandText;
                objSqlCommand.CommandTimeout = ConnectionTimeout;
                if (returnType == DataReturnType.none)
                {
                    objSqlCommand.Connection.Open();
                    objSqlCommand.ExecuteNonQuery();
                }
                else
                {
                    objSqlDataAdapter = new SqlDataAdapter(objSqlCommand);
                    objSqlDataAdapter.Fill(objdataSet);

                    if (returnType == DataReturnType.DataSet)
                    {
                        returnValue = objdataSet;
                    }
                    else if (returnType == DataReturnType.Table)
                    {
                        returnValue = objdataSet.Tables[0];
                    }
                    else if (returnType == DataReturnType.DataRow)
                    {
                        returnValue = objdataSet.Tables[0].Rows[0];

                    }
                    else if (returnType == DataReturnType.Value)
                    {
                        returnValue = objdataSet.Tables[0].Rows[0][0];
                    }

                }
                return returnValue;
            }
            finally
            {
                if (objSqlCommand.Connection.State != ConnectionState.Closed)
                {
                    objSqlCommand.Connection.Close();
                }
            }
        }
        /// <summary>
        /// Execute sql Procedure with sql parameter.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="returnType"></param>
        /// <returns></returns>
        public object getQueryResult(string commandText, DataReturnType returnType, List<SqlParameter> sqlParameters, int ConnectionTimeout = 50)
        {
            DataSet objdataSetQuery = new DataSet();
            try
            {
                objClsCommand = new Command();
              
                objSqlCommand = objClsCommand.getCommand;
                objSqlCommand.CommandText = commandText;
                objSqlCommand.CommandTimeout = ConnectionTimeout;
                objSqlCommand.CommandType = CommandType.StoredProcedure;
                if (sqlParameters != null)
                {
                    if (sqlParameters.Count != 0)
                    {
                        objSqlCommand.Parameters.AddRange(sqlParameters.ToArray());
                    }
                }
                if (returnType == DataReturnType.none)
                {
                    if (objSqlCommand.Connection.State == ConnectionState.Closed)
                    {
                        objSqlCommand.Connection.Open();
                    }
                    //objSqlCommand.Connection.Open();
                    objSqlCommand.ExecuteNonQuery();
                }
                else
                {
                    if (objSqlCommand.Connection.State == ConnectionState.Closed)
                    {
                        objSqlCommand.Connection.Open();
                    }

                    objSqlDataAdapter = new SqlDataAdapter(objSqlCommand);
                    objSqlDataAdapter.Fill(objdataSetQuery);

                    if (returnType == DataReturnType.DataSet)
                    {
                        returnValue = objdataSetQuery;
                    }
                    else if (returnType == DataReturnType.Table)
                    {
                        returnValue = objdataSetQuery.Tables[0];
                    }
                    else if (returnType == DataReturnType.DataRow)
                    {
                        returnValue = objdataSetQuery.Tables[0].Rows[0];

                    }
                    else if (returnType == DataReturnType.Value)
                    {
                        returnValue = objdataSetQuery.Tables[0].Rows[0][0];
                    }

                }
                return returnValue;
            }           
            finally
            {
                if (objSqlCommand.Connection.State != ConnectionState.Closed)
                {
                    objSqlCommand.Connection.Close();
                }
            }
        }
        /// <summary>
        /// Execute sql Procedure with single sql parameter.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="returnType"></param>
        /// <returns></returns>
        public object getQueryResult(string commandText, DataReturnType returnType, SqlParameter sqlParameter, int ConnectionTimeout = 50)
        {
            try
            {
                objClsCommand = new Command();
                objdataSet = new DataSet();
                objSqlCommand = objClsCommand.getCommand;
                objSqlCommand.CommandText = commandText;
                objSqlCommand.CommandTimeout = ConnectionTimeout;
                objSqlCommand.CommandType = CommandType.StoredProcedure;
                if (sqlParameter != null)
                {
                    objSqlCommand.Parameters.Add(sqlParameter);
                }
                if (returnType == DataReturnType.none)
                {
                    objSqlCommand.Connection.Open();
                    objSqlCommand.ExecuteNonQuery();
                }
                else
                {
                    objSqlDataAdapter = new SqlDataAdapter(objSqlCommand);
                    objSqlDataAdapter.Fill(objdataSet);

                    if (returnType == DataReturnType.DataSet)
                    {
                        returnValue = objdataSet;
                    }
                    else if (returnType == DataReturnType.Table)
                    {
                        returnValue = objdataSet.Tables[0];
                    }
                    else if (returnType == DataReturnType.DataRow)
                    {
                        returnValue = objdataSet.Tables[0].Rows[0];

                    }
                    else if (returnType == DataReturnType.Value)
                    {
                        returnValue = objdataSet.Tables[0].Rows[0][0];
                    }

                }
                return returnValue;
            }
            finally
            {
                if (objSqlCommand.Connection.State != ConnectionState.Closed)
                {
                    objSqlCommand.Connection.Close();
                }
            }
        }


        /// <summary>
        /// execute sql command.
        /// </summary>
        /// <param name="commandText"></param>
        public void executeCommand(string commandText, int ConnectionTimeout = 50)
        {
            try
            {
                objClsCommand = new Command();
                objSqlCommand = objClsCommand.getCommand;
                objSqlCommand.CommandText = commandText;
                objSqlCommand.CommandTimeout = ConnectionTimeout;
                objSqlCommand.Connection.Open();
                objSqlCommand.ExecuteNonQuery();
            }
            finally
            {

                if (objSqlCommand.Connection.State != ConnectionState.Closed)
                {
                    objSqlCommand.Connection.Close();
                }
            }

        }

        /// <summary>
        /// execute sql Procedure with collaction of sql parameters.
        /// </summary>
        /// <param name="commandText"></param>
        public void executeCommand(string commandText, List<SqlParameter> sqlParameters, int ConnectionTimeout = 50)
        {
            try
            {
                objClsCommand = new Command();
                objSqlCommand = objClsCommand.getCommand;
                objSqlCommand.CommandText = commandText;
                objSqlCommand.CommandTimeout = ConnectionTimeout;
                objSqlCommand.CommandType = CommandType.StoredProcedure;
                if (sqlParameters != null)
                {
                    if (sqlParameters.Count != 0)
                    {
                        //  Check on NullValues in the SqlParameter list
                        CheckParameters(sqlParameters);

                        //after the check change the list to an array and 
                        //add to the SqlCommand
                        objSqlCommand.Parameters.AddRange(sqlParameters.ToArray());
                    }
                }
                objSqlCommand.Connection.Open();
                objSqlCommand.ExecuteNonQuery();
            }
            finally
            {

                if (objSqlCommand.Connection.State != ConnectionState.Closed)
                {
                    objSqlCommand.Connection.Close();
                }
            }

        }
        /// <summary>
        /// execute sql Procedure with single sql parameter.
        /// </summary>
        /// <param name="commandText"></param>
        public void executeCommand(string commandText, SqlParameter sqlParameter, int ConnectionTimeout = 50)
        {
            try
            {
                objClsCommand = new Command();
                objSqlCommand = objClsCommand.getCommand;
                objSqlCommand.CommandText = commandText;
                objSqlCommand.CommandTimeout = ConnectionTimeout;
                objSqlCommand.CommandType = CommandType.StoredProcedure;
                if (sqlParameter != null)
                {
                    objSqlCommand.Parameters.Add(sqlParameter);
                }
                objSqlCommand.Connection.Open();
                objSqlCommand.ExecuteNonQuery();
            }
            finally
            {

                if (objSqlCommand.Connection.State != ConnectionState.Closed)
                {
                    objSqlCommand.Connection.Close();
                }
            }

        }
        /// <summary>
        /// execute sql Function command.
        /// </summary>
        /// <param name="commandText"></param>
        public object executeFunctionCommand(string commandText, List<SqlParameter> sqlParameters, int ConnectionTimeout = 50)
        {
            object returnVal = null;
            try
            {
                objClsCommand = new Command();
                objSqlCommand = objClsCommand.getCommand;
                objSqlCommand.CommandText = commandText;
                objSqlCommand.CommandTimeout = ConnectionTimeout;
                objSqlCommand.CommandType = CommandType.StoredProcedure;
                if (sqlParameters != null)
                {
                    if (sqlParameters.Count != 0)
                    {
                        //  Check on NullValues in the SqlParameter list
                        CheckParameters(sqlParameters);

                        //after the check change the list to an array and 
                        //add to the SqlCommand
                        objSqlCommand.Parameters.AddRange(sqlParameters.ToArray());
                    }
                }
                SqlParameter returnValue = objSqlCommand.Parameters.Add("@RETURN_VALUE", DbType.Object);
                returnValue.Direction = ParameterDirection.ReturnValue;
                objSqlCommand.Connection.Open();
                objSqlCommand.ExecuteNonQuery();
                returnVal = returnValue.Value;
            }
            finally
            {

                if (objSqlCommand.Connection.State != ConnectionState.Closed)
                {
                    objSqlCommand.Connection.Close();
                }

            }
            return returnVal;
        }

     
        private static void CheckParameters(List<SqlParameter> sqlParameters)
        {
            foreach (SqlParameter parm in sqlParameters)
            {
                // when a parm.Value is null, the parm is not send to 
                // the database so the stored procedure returns with the error
                // that it misses a parameter
                // it is very possible that the parameter should be null, 
                // so when set it DBNull.Value the parameter
                // is send to the database

                if (parm.Value == null)
                    parm.Value = DBNull.Value;
            }
        }

       
    }

}
