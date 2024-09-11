using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MDL
{
    public class StudentMasterMDL
    {
        public int PK_SudentId { get; set; }
        [Required(ErrorMessage = "School Name Required.")]
        public int FK_CompanyId { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Student Name Required.")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Invalid Student Name")]
        public string StudentName { get; set; }
        [Required(ErrorMessage = "Class Name Required.")]
        public string ClassName { get; set; }
        [Required(ErrorMessage = "Section Required.")]
        public string ClassCode { get; set; }
        [Required(ErrorMessage = "Student Address Required.")]
        public string Address { get; set; }
        //[Required(ErrorMessage ="Father Name Required.")]
        //public string FatherName { get; set; }
        //[Required(ErrorMessage ="Mother Name Required.")]
        //public string MotherName { get; set; }
        [Required(ErrorMessage = "Guardian Contact No. Required.")]
        [RegularExpression(@"^([6-9]{1})([0-9]{9})$", ErrorMessage = "Invalid Mobile No.")]
        [MinLength(length: 10, ErrorMessage = "Mobile No. Is Not Valid")]
        public string GuardianContactNo { get; set; }

        public int Fk_StoppageID { get; set; }

        public string StoppageName { get; set; }
        //[Required(ErrorMessage = "Alternate Contact No. Required.")]
        [RegularExpression("^[0-9]+(?:[-,_]?[0-9])+$", ErrorMessage = "Invalid Alternate Guardian Contact No.")]
        [MinLength(length: 10, ErrorMessage = "Contact No. Not Valid")]

        public string AlternateGuardianContactNo { get; set; }
        [Required(ErrorMessage = "Student Code Required.")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Invalid Student Code")]
        public string Student_Code { get; set; }
        //[Required(ErrorMessage = "Emergency Contact No. Required.")]
        [RegularExpression("^[0-9]+(?:[-,_]?[0-9])+$", ErrorMessage = "Invalid Emergency Contact No.")]
        [MinLength(length: 10, ErrorMessage = "Contact No. Not Valid")]
        public string Emergency_Contact_No { get; set; }
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public HttpPostedFileBase ExcelFile { get; set; }
        public string Remarks { get; set; }
        [Required(ErrorMessage = "Route Name Required.")]
        public int FK_RouteId { get; set; }
        public string Route_Name { get; set; }
        public string Alternate_ContactNo { get; set; }
        public int vCompanyId { get; set; }
        public HttpPostedFileBase ValidateExcel { get; set; }
        public HttpPostedFileBase StudentImage { get; set; }
        public string StudentImageUrl { get; set; }
        public string Gender { get; set; }
        [Required(ErrorMessage = "Guardian Name Required.")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Invalid Guardian Name")]
        public string GuardianName { get; set; }
        [Required(ErrorMessage = "Relation Required.")]
        public string Relation { get; set; }
        public string LocationName { get; set; }
        public int FK_ShiftId { get; set; }
        public string Shift_Name { get; set; }
        public string RFID { get; set; }
        public string ImageName { get; set; }

        public string profileImage { get; set; }
        public string Address_Lat { get; set; }
        public string Address_Long { get; set; }
        public string Icon_Value { get; set; }
        public string StoppageLocation { get; set; }
    }
}
