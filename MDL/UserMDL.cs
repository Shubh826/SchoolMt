using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL
{
    public class UserMDL
    {
        public int userid { get; set; }

        [Required(ErrorMessage = "Please Enter First Name.")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Please Use letters only.")]
        public string FirstName { get; set; }


        //[Required(ErrorMessage = "Please Enter First Name.")]
        //[RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Please Use letters only.")]
        public string MiddleName { get; set; }
        //public int StateID { get; set; }
       


        //[Required(ErrorMessage = "Please Enter Last Name.")]
        //[RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Please Use letters only.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Enter Password.")]
        [StringLength(14, ErrorMessage = "Password Must be between 4 and 14 characters", MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please Select Gender.")]
        public string Gender { get; set; }

        //[DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Please Enter Date Of Birth.")]
        public string DateOfBirth { get; set; }

        [Required(ErrorMessage = "Please Enter Email ID.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter Valid Email Id.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter Mobile No.")]
        [StringLength(13, MinimumLength = 10)]
        [RegularExpression(@"^([6-9]{1})([0-9]{9})$", ErrorMessage = "Entered Mobile No format is not valid.")]
        public string MobileNo { get; set; }
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please Use Numbers only.")]
        public string PhoneNo { get; set; }

        //[DataType(DataType.MultilineText)]
        //[StringLength(100), Display(Name = "Address")]
        //[Required(ErrorMessage = "Please Enter Address.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please Select Role Name.")]
        [Range(1, 9999, ErrorMessage = "Please Select Role Name.")]
        public int roleid { get; set; }

        public string RoleName { get; set; }
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Please Select Company Name.")]
        public int fk_companyid { get; set; }

        [Required(ErrorMessage = "Please Enter User Name.")]
        [RegularExpression(@"^[a-zA-Z0-9]+[a-zA-Z0-9-_.@]*$", ErrorMessage = "Only Alphabets, Numerals, Hyphen, Underscore, '@' & Dot Is Allowed. First Character Must Be An Alpha Numeral.")]
        public string name { get; set; }

        [Required(ErrorMessage = "Please Select City.")]
        [Range(1, 9999, ErrorMessage = "Please Select City.")]
        public int cityid { get; set; }

        public string cityname { get; set; }
        [Required(ErrorMessage = "Please Select State.")]
        [Range(1, 9999, ErrorMessage = "Please Select State.")]
        public int stateid { get; set; }

        public string statename { get; set; }
        ///[Required(ErrorMessage = "Please Select Country.")]
        public int countryid { get; set; }

        public string countryname { get; set; }

        public bool IsActive { get; set; }
        public string Status { get; set; }
        public bool IsDeleted { get; set; }

        public int CreatedBy { get; set; }

        public string CreatedDate { get; set; }

        public int UpdatedBy { get; set; }

        public string logoClass { get; set; }
        public int ClientId { get; set; }

        //added on 24 march 2018//
    
        public int Fk_ClientId { get; set; }
    
    }
}
