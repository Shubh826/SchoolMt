using DAL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class ErrorLogBAL
    {
        public static void SetError(Exception Ex, MethodBase objBase, string controller, string action, string Source, string Remarks)
        {
            ErrorLogDAL objErrorLogDAL = new ErrorLogDAL();
            //objErrorLogDAL.SetError(objBase.DeclaringType.Assembly.GetName().Name, objBase.DeclaringType.FullName, objBase.Name, Ex.Message, "");
            objErrorLogDAL.SetError(objBase.DeclaringType.Assembly.GetName().Name, controller, action, Source, Ex.Message, Ex.GetType().ToString(), Remarks);

        }
    }
}
