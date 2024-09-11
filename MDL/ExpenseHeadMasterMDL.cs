using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL
{
    public class ExpenseHeadMasterMDL
    {
        [Required(ErrorMessage = "Please Select School Name.")]
        public int FK_CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int PK_ExpenseHeadId { get; set; }
        [Required(ErrorMessage = "Please Select Expense Head Name.")]
        public string ExpenseHeadName { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
        public int CreatedBy { get; set; }
    }
}
