using MDL.Common;
using MDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BAL
{
    public class StudentReportBAL
    {
        private StudentReportDAL objStudentReportDAL;
        public StudentReportBAL() 
        {
            objStudentReportDAL=new StudentReportDAL();
        }

        public bool GetStudentReportData(out List<StudentReportMDL> _List, out BasicPagingMDL objBasicPagingMDL, int id, int FK_CompanyId, int rowPerpage = 10, int currentPage = 1, string SearchBy = "", string SearchValue = "", string fromDate = "", string toDate = "")
        {
            return objStudentReportDAL.GetStudentReportData(out _List, out objBasicPagingMDL, id, rowPerpage, currentPage, FK_CompanyId, SearchBy, SearchValue,fromDate,toDate);
        }
    }
}
