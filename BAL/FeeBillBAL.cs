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
    public class FeeBillBAL
    {
        private FeeBillDAL objFeeBillDal;
        public FeeBillBAL()
        {
            objFeeBillDal = new FeeBillDAL();
        }
        public bool GetFeeBillData(out List<FeeBillMDL> objFeeBillList, out BasicPagingMDL objBasicPagingMDL, int id, int FK_CompanyId, int rowPerpage = 10, int currentPage = 1, string SearchBy = "", string SearchValue = "", string ClassName = "", string Section = "")
        {
            objFeeBillList = new List<FeeBillMDL>();
            objBasicPagingMDL = new BasicPagingMDL();
            return objFeeBillDal.GetFeeBillData(out objFeeBillList, out objBasicPagingMDL, id, rowPerpage, currentPage, FK_CompanyId, SearchBy, SearchValue, ClassName, Section);
        }
        public Messages AddEditFeeBill(FeeBillMDL objFeeBillMDL, out PaymentDetails _PaymentDetails)
        {
            return objFeeBillDal.AddEditFeeBill(objFeeBillMDL, out _PaymentDetails);
        }
    }
}
