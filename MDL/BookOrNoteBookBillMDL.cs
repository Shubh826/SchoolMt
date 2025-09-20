using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL
{
    public class BookOrNoteBookBillMDL
    {
        [Key]
        public int PK_BorNBillID { get; set; }
        public string BILLNo { get; set; }
        [Required]
        [StringLength(100)]
        public string StudentName { get; set; }
        public int FK_StudentId { get; set; }
        public string FatherName { get; set; }
        public int FK_ClassId { get; set; }
        [Required]
        [StringLength(30)]
        public string ClassName { get; set; }

        [Required]
        public bool IsBook { get; set; } = false;

        [Required]
        public bool IsNotebook { get; set; } = false;
        public int Quantity { get; set; }
        [Required]
        public decimal Subtotal { get; set; }
        public int Discount { get; set; }

        [Required]
        public decimal GST { get; set; }

        [Required]
        public decimal GrandTotal { get; set; }
        public int DueAmount { get; set; }
        public string PaymentDate { get; set; }
        public int Cash { get; set; }
        public int Other { get; set; }
        public int Online { get; set; }

        public string HdnPaymentDate { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentMode { get; set; }

        public DateTime BillDate { get; set; } = DateTime.Now;
        public string BillingDate { get; set; }

        [Required]
        public int CreatedBy { get; set; }
        public int CompanyId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public string SelectedBookIds { get; set; }
        public string SelectedNotebookIds { get; set; }
    }


    public class BookOrNotebookDetail
    {
        public int PK_BookId { get; set; }
        public string BookName { get; set; }
        public int NotebookPage { get; set; } // Example: "180 Page"
        public decimal Price { get; set; }
        public bool Ischeck { get; set; }
    }
    public class BookPaymentDetails
    {
        public int PK_BillId { get; set; }
        public string FatherName { get; set; }
        public string StudentName { get; set; }
        public string ClassName { get; set; }
        public string BillNo { get; set; }
        public string PaymentDate { get; set; }
        public decimal TotalFee { get; set; }
        public int Cash { get; set; }
        public int Online { get; set; }
        public int Other { get; set; }
        public int DueAmount { get; set; }
        public int Discount { get; set; }
        public int PaidAmount { get; set; }
        public string PdfContent { get; set; } 
        public List<ItemDetails> ItemList { get; set; } = new List<ItemDetails>();
    }

    public class ItemDetails
    {
        public string ItemName { get; set; }
        public string ItemType { get; set; }
        public int PageCount { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
    }
    public class BookOrNoteBookBillDetailMDL
    {
        public int PK_BorNBillDetID { get; set; }
        public int FK_BorNBillID { get; set; }
        public string BILLNo { get; set; }
        public string ItemName { get; set; }
        public string ItemType { get; set; }
        public int PageCount { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }

}
