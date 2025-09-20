using DAL;
using MDL;
using MDL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class BookOrNoteBookBillBAL
    {
        BookOrNoteBookBillDAL objBookOrNoteBookBillDAL = null;
        public BookOrNoteBookBillBAL()
        {
            objBookOrNoteBookBillDAL = new BookOrNoteBookBillDAL();
        }

        public List<BookOrNotebookDetail> GetBooksOrNoteBooks(string Type, string ClassName, int BillId)
        {
            return objBookOrNoteBookBillDAL.GetBooksOrNoteBooks(Type, ClassName, BillId);

        }

        public Messages PostBookOrNoteBookBill(BookOrNoteBookBillMDL obj, out BookPaymentDetails _BookPaymentDetails)
        {
            return objBookOrNoteBookBillDAL.PostBookOrNoteBookBill(obj, out _BookPaymentDetails);
        }

        public bool GetBookOrNoteBookBillData(out List<BookOrNoteBookBillMDL> billList, out BasicPagingMDL objBasicPagingMDL, int id, int fk_companyid, int rowPerPage, int currentPage, string SearchBy, string SearchValue, string fromDate, string toDate)
        {
            billList = new List<BookOrNoteBookBillMDL>();
            objBasicPagingMDL = new BasicPagingMDL();
            return objBookOrNoteBookBillDAL.GetBookOrNoteBookBillData(out billList, out objBasicPagingMDL, id, fk_companyid, rowPerPage, currentPage, SearchBy, SearchValue, fromDate, toDate);

        }
        public bool GetBookOrNoteBookBillForEdit(out BookOrNoteBookBillMDL billData, out List<BookOrNoteBookBillDetailMDL> detailList, int billId)
        {
            billData = new BookOrNoteBookBillMDL();
            detailList = new List<BookOrNoteBookBillDetailMDL>();

            return objBookOrNoteBookBillDAL.GetBookOrNoteBookBillForEdit(out billData, out detailList, billId);
        }


    }
}
