using DAL;
using MDL;
using MDL.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class StudentMasterBAL
    {
        private StudentMasterDAL objStudentMasterDal;
        public StudentMasterBAL()
        {
            objStudentMasterDal = new StudentMasterDAL();
        }

        public Messages InsertStudentData(StudentMasterMDL objStudentMasterMDL)
        {
            return objStudentMasterDal.InsertStudentData(objStudentMasterMDL);
        }
        public bool GetStudentData(out List<StudentMasterMDL> objStudentList, out BasicPagingMDL objBasicPagingMDL, int id, int FK_CompanyId, int rowPerpage = 10, int currentPage = 1, string SearchBy = "", string SearchValue = "")
        {
            objStudentList = new List<StudentMasterMDL>();
            objBasicPagingMDL = new BasicPagingMDL();
            return objStudentMasterDal.GetStudentData(out objStudentList, out objBasicPagingMDL, id, rowPerpage, currentPage, FK_CompanyId, SearchBy, SearchValue);
        }
        
        //public bool GetStudentList(int companyId, out List<StudentMasterMDL> objStudentLst)
        //{
        //    return objStudentMasterDal.GetStudentList(companyId, out objStudentLst);
        //}
    }
}
