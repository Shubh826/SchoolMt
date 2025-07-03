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
    public class ExpenseMasterBAL
    {
        private ExpenseMasterDAL objExpenseMasterDAL;
        public ExpenseMasterBAL()
        {
            objExpenseMasterDAL = new ExpenseMasterDAL();
        }
        public bool GetExpenseList(out List<ExpenseMasterMDL> objExpenseList, out BasicPagingMDL objBasicPagingMDL, int id, int FK_CompanyId, int rowPerPage = 20, int currentPage = 1, string SearchBy = "", string SearchValue = "")
        {
            objExpenseList = new List<ExpenseMasterMDL>();
            objBasicPagingMDL = new BasicPagingMDL();
            return objExpenseMasterDAL.GetExpenseList(out objExpenseList, out objBasicPagingMDL, id, rowPerPage, currentPage, FK_CompanyId, SearchBy, SearchValue);
        }

        public Messages AddEditExpense(ExpenseMasterMDL expenseMasterMDL)
        {
            return objExpenseMasterDAL.AddEditExpense(expenseMasterMDL);
        }
    }
}
