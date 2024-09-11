using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.DataUtility
{
    /// <summary>
    /// return type from database.
    /// </summary>
    public enum DataReturnType
    {
        DataSet,
        Table,
        DataRow,
        Value,
        none
    }
    public enum Database
    {
        Live,
        Report,
    }
}
