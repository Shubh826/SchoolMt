using DAL;
using MDL;
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
    }
}
