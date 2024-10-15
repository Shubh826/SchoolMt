using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MDL
{
    public class StudentFeeDetailsMDL
    {
        public int StudentId { get; set; }
        public int CompanyId { get; set; }
        public string StudentName { get; set; }
        public string FatherName { get; set; }
        public string MobileNo { get; set; }
        public string ClassName { get; set; }
        public string Address { get; set; }
        public int AdmissionFee { get; set; }
        public string AdmissionDate { get; set; }
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
        public int FebruaryTrnsFee { get; set; }
        public int MarchFee { get; set; }
        public int MarchTrnsFee { get; set; }
        public int ExaminationFee1 { get; set; }
        public int ExaminationFee2 { get; set; }
        public int PreviousDueAmount { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int CreatedBy { get; set; }
        public HttpPostedFileBase ExcelFile { get; set; }
        public string Remark { get; set; }
        public int ApplicableMonthFee { get; set; }
        public int ApplicableTrnsFee { get; set; }
        public string ApplicableMonth { get; set; }
        public string AcademicSession { get; set; }
    }
    public class StudentFeeDetailsValidExcelDataMDL
    {
        public string StudentName { get; set; } = "NA";
        public string FatherName { get; set; } =  "NA";
        public string MobileNo { get; set; } =  "NA";
        public string ClassName { get; set; } =  "NA";
        public string Address { get; set; } =  "NA";
        public int AdmissionFee { get; set; } = 0; 
        public string AdmissionDate { get; set; } =  "NA";

        public int AprilFee { get; set; } = 0;
        public int AprilTrnsFee { get; set; } = 0;
        public int MayFee { get; set; } = 0;
        public int MayTrnsFee { get; set; } = 0;
        public int JuneFee { get; set; } = 0;
        public int JuneTrnsFee { get; set; } = 0;
        public int JulyFee { get; set; } = 0;
        public int JulyTrnsFee { get; set; } = 0;
        public int AugustFee { get; set; } = 0;
        public int AugustTrnsFee { get; set; } = 0;
        public int SeptemberFee { get; set; } = 0;
        public int SeptemberTrnsFee { get; set; } = 0;
        public int OctoberFee { get; set; } = 0;
        public int OctoberTrnsFee { get; set; } = 0;
        public int NovemberFee { get; set; } = 0;
        public int NovemberTrnsFee { get; set; } = 0;
        public int DecemberFee { get; set; } = 0;
        public int DecemberTrnsFee { get; set; } = 0;
        public int JanuaryFee { get; set; } = 0;
        public int JanuaryTrnsFee { get; set; } = 0;
        public int FebruaryFee { get; set; } = 0;
        public int FebruaryTrnsFee { get; set; } = 0;
        public int MarchFee { get; set; } = 0;
        public int MarchTrnsFee { get; set; } = 0;

        public int ExaminationFee1 { get; set; } = 0;
        public int ExaminationFee2 { get; set; } = 0;
        public int PreviousDueAmount { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int CreatedBy { get; set; } = 0;
        public int ApplicableMonthFee { get; set; } = 0;
        public int ApplicableTransFee { get; set; } = 0;
        public string ApplicableMonth { get; set; } =  "NA";
    }
}
