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
    public class StudentFeeDetailsBAL
    {
        private StudentFeeDetailsDAL objStudentFeeDetailsDAL;
        public StudentFeeDetailsBAL()
        {
            objStudentFeeDetailsDAL = new StudentFeeDetailsDAL();
        }
        public Messages UploadValidStudentDataToDB(string jsondata, int UserId,int CompanyId)
        {
            return objStudentFeeDetailsDAL.UploadValidStudentDataToDB(jsondata, UserId, CompanyId);
        }
        public bool GetStudentFeeData(out List<StudentFeeDetailsMDL> objStudentList, out BasicPagingMDL objBasicPagingMDL, int id, int FK_CompanyId, int rowPerpage = 10, int currentPage = 1, string SearchBy = "", string SearchValue = "")
        {
            objStudentList = new List<StudentFeeDetailsMDL>();
            objBasicPagingMDL = new BasicPagingMDL();
            return objStudentFeeDetailsDAL.GetStudentFeeData(out objStudentList, out objBasicPagingMDL, id, rowPerpage, currentPage, FK_CompanyId, SearchBy, SearchValue);
        }
    }
}
