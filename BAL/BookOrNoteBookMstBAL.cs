using DAL;
using MDL;
using MDL.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class BookOrNoteBookMstBAL
    {
        BookOrNoteBookMstDAL objBookOrNoteBookMstDAL = null;
        public BookOrNoteBookMstBAL()
        {
            objBookOrNoteBookMstDAL = new BookOrNoteBookMstDAL();
        }
        public Messages AddEditBookOrNoteBook(BookOrNoteBookMst ObjBookOrNoteBookMstMDL)
        {
            return objBookOrNoteBookMstDAL.AddEditBookOrNoteBook(ObjBookOrNoteBookMstMDL);
        }
        public bool GetBookOrNoteBookData(out List<BookOrNoteBookMst> _booklist, out BasicPagingMDL objBasicPagingMDL, int Userid, int RowPerpage = 20, int CurrentPage = 1, int PK_BookId = 0, string SearchBy = "", string SearchValue = "", int Fk_companyid = 0)
        {
            _booklist = new List<BookOrNoteBookMst>();
            return objBookOrNoteBookMstDAL.GetBookOrNoteBookData(out _booklist, out objBasicPagingMDL, Userid, RowPerpage, CurrentPage, PK_BookId, SearchBy, SearchValue, Fk_companyid);
        }

    }
}
