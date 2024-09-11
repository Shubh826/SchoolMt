using System.Data.SqlClient;

namespace DAL.DataUtility
{
    /// <summary>
    /// use to create sql command
    /// </summary>
    public class Command
    {
        /// <summary>
        /// return sql command.
        /// </summary>C:\Sachin\CurrentProject\VintyImpex\DataUtility\Command.cs
        public SqlCommand getCommand
        {
            get
            {
                Connection con = new Connection();
                return con.getConnection.CreateCommand();
            }
        }

    }
}
