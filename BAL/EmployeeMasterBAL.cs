using DAL;
using MDL;
using MDL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class EmployeeMasterBAL
    {
        EmployeeMasterDAL objEmployeeMasterDAL = null;
        public EmployeeMasterBAL()
        {
            objEmployeeMasterDAL = new EmployeeMasterDAL();
        }
        public bool getEmployee(out List<EmployeeMasterMDL> _Employeelist, out BasicPagingMDL objBasicPagingMDL, int id, int companyId, int UserId, int RowPerpage, int CurrentPage = 1, string SearchBy = "", string SearchValue = "")
        {

            return objEmployeeMasterDAL.getEmployee(out _Employeelist, out objBasicPagingMDL, id, companyId, UserId, RowPerpage, CurrentPage, SearchBy, SearchValue);
        }
        public Messages AddEmployee(EmployeeMasterMDL objEmployeeMasterMDL)
        {
            return objEmployeeMasterDAL.AddEmployee(objEmployeeMasterMDL);
        }
    }
}
