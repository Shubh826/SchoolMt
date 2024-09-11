using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace MDL
{
    public class GenerateInvoiceMDL
    {
        public Int64 PK_InvoiceId { get; set; }
        [Required(ErrorMessage = "Please Select School Name.")]
        public int FK_CompanyId { get; set; }
        public string SchoolName { get; set; }
        public int FK_ClassId { get; set; }
        [Required(ErrorMessage = "Please Select Class.")]

        public string ClassName { get; set; }
        [Required(ErrorMessage = "Please Select Student.")]

        public int FK_StudentId { get; set; }
        public string StudentName { get; set; }
        [Required(ErrorMessage = "Please Select Year.")]
        public int Year { get; set; }
        public string YearName { get; set; }
        [Required(ErrorMessage = "Please Select Month.")]
        public string Month { get; set; }
        public string GrandTotal { get; set; }
        public string InCash { get; set; }
        public string InAccount { get; set; }
        public string DueAmount { get; set; }

        public string TotalDue { get; set; }
        public string Section { get; set; }
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public string InvoiceDetailJson { get; set; }
        public string AdmissionFee { get; set; }
        public string TuitionFee { get; set; }
        public string BusFee { get; set; }
        public string OtherCharges { get; set; }
        public string Miscellaneous { get; set; }
        public string Discount { get; set; }
        public string PreviousDue { get; set; }
        public List<feehead> _feeHeadlist { get; set; }
    }
    public class feehead
    {
        public int PK_feeheadId { get; set; }
        public string feeheadName { get; set; }
        public int Value { get; set; }
    }
}
