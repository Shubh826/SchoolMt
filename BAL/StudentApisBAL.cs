using DAL.WebAPI;
using MDL;
using MDL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class StudentApisBAL
    {
        StudentApisDAL objStudentApisDAL = null;
        public StudentApisBAL()
        {
            objStudentApisDAL = new StudentApisDAL();
        }
        public ServiceResult<DropDownMDL> GetClass(out List<DropDownMDL> _ClassDataList)
        {
            return objStudentApisDAL.GetClass(out _ClassDataList);
        }
        public ServiceResult<DropDownMDL> GetStudents(int FK_ClassId, out List<DropDownMDL> _StudentDataList)
        {
            return objStudentApisDAL.GetStudents(FK_ClassId,out _StudentDataList);
        }
    }
}
