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
    public class InstallationBAL
    {
        InstallationDAL objInstallationDAL = null;
        public Messages AddEdit_MapMachineDeviceSyncStatus(int GaugeFuelconfigId, bool IsSync)
        {
            return objInstallationDAL.AddEdit_MapMachineDeviceSyncStatus(GaugeFuelconfigId, IsSync);
        }
    }
}
