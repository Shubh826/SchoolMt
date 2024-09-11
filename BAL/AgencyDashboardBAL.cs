using DAL;
using MDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class AgencyDashboardBAL
    {
        AgencyDashboardDAL objAgencyDashboardDAL = null;

        public AgencyDashboardBAL()
        {
            objAgencyDashboardDAL = new AgencyDashboardDAL();
        }


        public bool getDashboardData(out AgencyDashboardMDL objmdl, out List<JoiningTotalJobs> objtotal, out List<TotalOpenJobs> objopenjobs, out List<LocationWiseOpen> objLocationWise, int fk_companyid, int userid, int clientId)
        {
            objtotal = new List<JoiningTotalJobs>();
            objopenjobs = new List<TotalOpenJobs>();
            objLocationWise = new List<LocationWiseOpen>();
            //objBasicPagingMDL = new BasicPagingMDL();
            return objAgencyDashboardDAL.getDashboardData(out objmdl, out objtotal, out objopenjobs, out objLocationWise, fk_companyid, userid, clientId);
        }
    }
}
