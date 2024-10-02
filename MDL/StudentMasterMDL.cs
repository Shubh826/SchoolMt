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
        [Required(ErrorMessage = "Guardian Contact No. Required.")]
        [RegularExpression(@"^([6-9]{1})([0-9]{9})$", ErrorMessage = "Invalid Mobile No.")]
        [MinLength(length: 10, ErrorMessage = "Mobile No. Is Not Valid")]
        public string GuardianContactNo { get; set; }
        //[Required(ErrorMessage = "Emergency Contact No. Required.")]
        [RegularExpression("^[0-9]+(?:[-,_]?[0-9])+$", ErrorMessage = "Invalid Emergency Contact No.")]
        [MinLength(length: 10, ErrorMessage = "Contact No. Not Valid")]
        public string Emergency_Contact_No { get; set; }
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public HttpPostedFileBase StudentImage { get; set; }
        public string StudentImageUrl { get; set; }
        public string Gender { get; set; }
        public string ImageName { get; set; }
        public string Category { get; set; }
        [Required(ErrorMessage = "Father Name Required.")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Invalid Father Name")]
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string StudentDOB { get; set; }
        public int FK_AreaId { get; set; }
        public int MonthlyFee { get; set; }
        public int TransportFee { get; set; }
        public int Discount { get; set; }
        public string profileImage { get; set; }
        public string AccSession { get; set; }

    }
}
