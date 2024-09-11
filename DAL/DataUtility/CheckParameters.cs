using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL.DataUtility
{
    public class CheckParameters
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlParameters"></param>
        internal static void ConvertNullToDBNull(List<SqlParameter> sqlParameters)
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
        internal static void ConvertNullToDBNull(SqlParameter[] sqlParameters)
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
