using DAL;
using MDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class AdminDashboardBAL
    {
        AdminDashboardDAL objAdminDashboardDAL = null;

        public AdminDashboardBAL()
        {
            objAdminDashboardDAL = new AdminDashboardDAL();
        }

        public bool GetAdminDashboardData(out AdminDashboardMDL objmdl, int fk_companyid, int userid, int clientId)
        {
            return objAdminDashboardDAL.GetAdminDashboardData(out objmdl, fk_companyid, userid, clientId);
        }

        public bool GetMonthWiseAdminDashboardData(out List<MonthwiseData> _MonthwiseData, int Year, int fk_companyid, int userid, int clientId)
        {
            return objAdminDashboardDAL.GetMonthWiseAdminDashboardData(out _MonthwiseData,Year,fk_companyid, userid, clientId);
        }

    }
}
