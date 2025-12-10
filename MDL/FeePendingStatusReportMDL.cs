using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL
{
    public class FeePendingStatusReportMDL
    {
        public int StudentId { get; set; }
        public int FK_CompanyId { get; set; }
        public string StudentName { get; set; }
        public string ClassName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public int DueAmount { get; set; }
        public string Gender { get; set; }
        public string ClassCode { get; set; }    
        public string NoOfStudents { get; set; }
        public int AprilFee { get; set; }
        public int AprilTrnsFee { get; set; }
        public int MayFee { get; set; }
        public int MayTrnsFee { get; set; }
        public int JuneFee { get; set; }
        public int JuneTrnsFee { get; set; }
        public int JulyFee { get; set; }
        public int JulyTrnsFee { get; set; }
        public int AugustFee { get; set; }
        public int AugustTrnsFee { get; set; }
        public int SeptemberFee { get; set; }
        public int HalfYearlyExamFee { get; set; }
        public int SeptemberTrnsFee { get; set; }
        public int OctoberFee { get; set; }
        public int OctoberTrnsFee { get; set; }
        public int NovemberFee { get; set; }
        public int NovemberTrnsFee { get; set; }
        public int DecemberFee { get; set; }
        public int DecemberTrnsFee { get; set; }
        public int JanuaryFee { get; set; }
        public int JanuaryTrnsFee { get; set; }
        public int FebruaryFee { get; set; }
        public int YearlyExamFee { get; set; }
        public int FebruaryTrnsFee { get; set; }
        public int MarchFee { get; set; }
        public int MarchTrnsFee { get; set; }
        public int Totalfee { get; set; }
        public int payablefee { get; set; }
        public int Cash { get; set; }
        public int Online { get; set; }
        public int Discount { get; set; }
        public int ApplicableTrnsFee { get; set; }
        public int PreDueAmount { get; set; }
        public int AdmissionFee { get; set; }
        public string PaymentDate { get; set; }

  
        public string ApplicableMonth { get; set; }

        public string DueMonths { get; set; }            // Comma-separated pending months (April,May,…)
     public int DueTransportAmount { get; set; }      // Total pending transport fee
        public int DueExamFee { get; set; }              // Pending Exam fee (ExamFee1 + ExamFee2 logic)

        /*START:new properties*/
        public int PreviousDueAmount { get; set; }
        public int ExaminationFee1 { get; set; }
        public int ExaminationFee2 { get; set; }

        public int ApplicableMonthFee { get; set; }
        public int MonthdueFee { get; set; }
        public int TransportdueFee { get; set; }

        public int TotalDueAmount { get; set; }
        /*END:new properties*/
    }
}
