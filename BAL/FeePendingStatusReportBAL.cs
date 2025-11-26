using DAL;
using MDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
  public  class FeePendingStatusReportBAL
    {
        FeePendingStatusReportDAL objDAL = null;
        public FeePendingStatusReportBAL()
        {
            objDAL = new FeePendingStatusReportDAL();
        }

        public List<FeePendingStatusReportMDL> GetFeePendingStatusReport(int FK_CompanyId, string ClassName, int RowPerpage, int CurrentPage, string SearchBy, string SearchValue)
        {
            return objDAL.GetFeePendingStatusReport(FK_CompanyId, ClassName, RowPerpage, CurrentPage, SearchBy, SearchValue);
        }
    }
}
