using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL
{
    public class StudentBulkMDL
    {
        public int Fk_CompanyId { get; set; }
        public string Student_Code { get; set; }
        public string StudentName { get; set; }
        public string ClassName { get; set; }
        public string ClassCode { get; set; }
        public string GuardianName { get; set; }
        public string Relation { get; set; }
        public string Gender { get; set; }
        public int FK_Shift_Id { get; set; }
        public string GuardianContactNo { get; set; }
        public string GuardianAlternateContactNo { get; set; }
        public string EmergencyContactNo { get; set; }
        public string StudentAddress { get; set; }
        public string StoppageName { get; set; }
        public int Fk_StoppageID { get; set; }
        public int CreatedBy { get; set; }
        public string RFID { get; set; }

    }
}
