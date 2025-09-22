using DAL;
using MDL.Common;
using MDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class SchoolConfigurationBAL
    {
        private SchoolConfigurationDAL objDal;
        public SchoolConfigurationBAL()
        {
            objDal = new SchoolConfigurationDAL();
        }
        public bool GetSchoolConfigurationData(out List<SchoolConfigurationMDL> List, out BasicPagingMDL objBasicPagingMDL, int id, int FK_CompanyId, int rowPerpage = 10, int currentPage = 1, string SearchBy = "", string SearchValue = "")
        {
            return objDal.GetSchoolConfigurationData(out List, out objBasicPagingMDL, id, rowPerpage, currentPage, FK_CompanyId, SearchBy, SearchValue);
        }
        public Messages InsertSchoolConfigurationData(SchoolConfigurationMDL obj)
        {
            return objDal.InsertSchoolConfigurationData(obj);
        }

        public Messages DeleteSchoolConfigurationData(string pkIds)
        {
            return objDal.DeleteSchoolConfigurationData(pkIds);
        }
    }
}
