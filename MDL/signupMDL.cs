using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL
{
    public class signupMDL
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please Enter Password.")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please Enter Confirm Password.")]
        public string CPassword { get; set; }
        [Required(ErrorMessage = "Please Enter Contact No.")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                   ErrorMessage = "Contact No is not valid")]
        public string ContactNo { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                   ErrorMessage = "Contact No is not valid")]
        public string AlternateNo { get; set; }
        [Required(ErrorMessage = "Please Enter EmailId.")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string EmailID { get; set; }
        [Required(ErrorMessage = "Please Enter First Name.")]
        public string FName { get; set; }
        public string LName { get; set; }
        [Required(ErrorMessage = "Please Enter Company Name.")]
        public string CompanyName { get; set; }


        public string AccountType { get; set; }
        public bool IsClient { get; set; }
        public bool IsAgency { get; set; }
        public bool IsBoth { get; set; }
    }
}
