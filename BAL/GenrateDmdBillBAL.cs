using DAL;
using MDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
   public class GenrateDmdBillBAL
    {

        GenrateDmdBillDAL objGenrateDmdBillDAL = null;
        public GenrateDmdBillBAL()
        {
            objGenrateDmdBillDAL = new GenrateDmdBillDAL();
        }

        public bool getFeedetails(out List<StudentFeeDetailsMDL> _StudentFeeDetailsMDL, GenrateDmdbillMdl obmdl)
        {
           return objGenrateDmdBillDAL.getFeedetails(out _StudentFeeDetailsMDL, obmdl);
        }
        }
}
