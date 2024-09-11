using DAL;
using MDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class ClientDashboardBAL
    {
        ClientDashboardDAL objClientDashboardDAL = null;

        public ClientDashboardBAL()
        {
            objClientDashboardDAL = new ClientDashboardDAL();
        }

        public bool getClientDashboardData(out ClientDashboardMDL objmdl,out List<TotalSkillsWiseProfile> objtotalSkillsProfile, out List<JoiningTotalProfile> objtotalProfile, out List<LocationWiseProfile> objLocationWise, int fk_companyid, int userid, int clientId)
        {
            objtotalSkillsProfile = new List<TotalSkillsWiseProfile>();
            objtotalProfile = new List<JoiningTotalProfile>();
            objLocationWise = new List<LocationWiseProfile>();
            return objClientDashboardDAL.getClientDashboardData(out objmdl,out objtotalSkillsProfile, out objtotalProfile, out objLocationWise, fk_companyid, userid, clientId);
        }
    }
}
