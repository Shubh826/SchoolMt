using DAL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Common
{
   public class ErrorlogBal
    {
        public static void SetError(Exception Ex, MethodBase objBase, string Source, string Remarks = "")
        {
            ErrorLogDAL objErrorLogDAL = new ErrorLogDAL();
            objErrorLogDAL.SetError(Source, objBase.DeclaringType.Assembly.GetName().Name, objBase.DeclaringType.FullName, objBase.Name, Ex.Message, Remarks);

        }

    }
}
