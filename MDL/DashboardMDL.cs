using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL
{
    public class DashboardMDL
    {

        // 1. Total Fee (SUM of Totalfeeamount from Student Fee Bill)
        public string TotalFee { get; set; }
        // 2. Collected Fee (TotalFee - PendingFee)
        public string TotalCollectedFee { get; set; }

        // 3. Pending Fee (SUM of DueAmount from Student Fee Bill)
        public string TotalPendingFee { get; set; }

       
        // 4. Total Expense (SUM of Amount from Expense Table)
        public string TotalExpense { get; set; }

        // 5. Book Collection
        public string TotalBookCollection { get; set; }

        // 6. Notebook Collection
        public string TotalNoteBookCollection { get; set; }

        // 7. Total Student
        public string TotalStudent { get; set; }
        public string ChartJsonData { get; set; }


    }
}
