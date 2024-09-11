using DAL;
using MDL;
using MDL.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class ExpenseHeadMasterBAL
    {
        private ExpenseHeadMasterDAL objExpenseHeadMasterDAL;
        private DataSet _dataSet;
        public ExpenseHeadMasterBAL()
        {
            objExpenseHeadMasterDAL = new ExpenseHeadMasterDAL();
        }
        public bool GetExpenseHead(out List<ExpenseHeadMasterMDL> _ExpenseHeadMasterList, out BasicPagingMDL objBasicPagingMDL, int Id, int Userid, int Fk_companyid, int RowPerpage = 20, int CurrentPage = 1, string SearchBy = "", string SearchValue = "")
        {
            _ExpenseHeadMasterList = new List<ExpenseHeadMasterMDL>();
            objBasicPagingMDL = new BasicPagingMDL();
            return objExpenseHeadMasterDAL.GetExpenseHead(out _ExpenseHeadMasterList, out objBasicPagingMDL, Id, Userid, Fk_companyid, RowPerpage, CurrentPage, SearchBy, SearchValue);
        }
        public Messages AddEditExpenseHead(ExpenseHeadMasterMDL ObjExpenseHeadMasterMDL)
        {
            return objExpenseHeadMasterDAL.AddEditExpenseHead(ObjExpenseHeadMasterMDL);
        }
    }
}
