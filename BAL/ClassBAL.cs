using DAL;
using MDL;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class ClassBAL
    {
        ClassDAL objclassDAL = null;
        public ClassBAL()
        {
            objclassDAL = new ClassDAL();
        }
        public bool GetClassDetails(out List<ClassMdl> _ClassList, int ClassID, string SearchBy, string SearchValue, int CompanyId)
        {
            _ClassList = new List<ClassMdl>();
            return objclassDAL.GetClassDetails(out _ClassList, ClassID, SearchBy, SearchValue, CompanyId);


        }
        public Messages AddEditClass(ClassMdl ObjClassMDL)
        {
            return objclassDAL.AddEditClass(ObjClassMDL);
        }
        }
}
