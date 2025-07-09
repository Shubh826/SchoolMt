using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL
{
    public class ExpenseMasterMDL
    {
        public int PK_ExpenseId { get; set; }
        [Required(ErrorMessage = "Expense Head is required.")]
        public int FK_ExpenseHeadId { get; set; }

        public string ExpenseHeadName { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Please enter a valid date.")]
        public string Date { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public int Amount { get; set; }
        public string Remarks { get; set; }

        public int CreatedBy { get; set; }
        public int FK_CompanyId { get; set; }
    }
}
