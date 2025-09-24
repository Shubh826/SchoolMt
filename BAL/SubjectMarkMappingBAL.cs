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
    public class SubjectMarkMappingBAL
    {
        private SubjectMarkMappingDAL objDal;
        public SubjectMarkMappingBAL()
        {
            objDal = new SubjectMarkMappingDAL();
        }
        public bool GetSubjectMarkMappingData(out List<SubjectMarkMappingMDL> List, out BasicPagingMDL objBasicPagingMDL, int id, int FK_CompanyId, int rowPerpage = 10, int currentPage = 1, string SearchBy = "", string SearchValue = "")
        {
            return objDal.GetSubjectMarkMappingData(out List, out objBasicPagingMDL, id, rowPerpage, currentPage, FK_CompanyId, SearchBy, SearchValue);
        }
        public Messages InsertSubjectMarkMappingData(SubjectMarkMappingMDL obj)
        {
            return objDal.InsertSubjectMarkMappingData(obj);
        }

        public Messages DeleteSchoolConfigurationData(int pkId)
        {
            return objDal.DeleteSchoolConfigurationData(pkId);
        }
    }
}
