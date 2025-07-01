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

        public List<BookOrNotebookDetail> GetBooksOrNoteBooks(string Type,string ClassName)
        {
            return objBookOrNoteBookBillDAL.GetBooksOrNoteBooks(Type, ClassName);

        }

        public Messages PostBookOrNoteBookBill(BookOrNoteBookBillMDL obj, out BookPaymentDetails _BookPaymentDetails)
        {
            return objBookOrNoteBookBillDAL.PostBookOrNoteBookBill(obj,out _BookPaymentDetails);
        }
    }
    }
