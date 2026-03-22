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
    public class GeneratedReportcardBAL
    {
        private GeneratedReportcardDAL objDal;
        public GeneratedReportcardBAL()
        {
            objDal = new GeneratedReportcardDAL();
        }
        public bool GetStudentData(out List<StudentMasterMDL> objStudentList, out BasicPagingMDL objBasicPagingMDL, int id, int rowPerpage, int currentPage, int FK_CompanyId, string SearchBy, string SearchValue, string Classname, string Section )
        {
            return objDal.GetStudentData(out objStudentList, out objBasicPagingMDL, id, rowPerpage, currentPage, FK_CompanyId, SearchBy, SearchValue, Classname, Section);
        }
        public ReportCardViewMDL GetStudentDataForReportCard(int id)
        {
            return objDal.GetStudentDataForReportCard(id);
        }

    }
}
