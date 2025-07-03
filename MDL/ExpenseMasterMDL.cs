using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL
{
    public class ExpenseMasterMDL
    {
        public int PK_ExpenseId { get; set; }
        public int FK_ExpenseHeadId { get; set; }
        public string ExpenseHeadName { get; set; }
        public string Date { get; set; }
        public int Amount { get; set; }
        public string Remarks { get; set; }

        public int CreatedBy { get; set; }
        public int FK_CompanyId { get; set; }
    }
}
