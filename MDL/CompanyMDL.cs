using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MDL
{
    public class CompanyMDL
    {
        public int CompanyId { get; set; }
        [Required(ErrorMessage = "Please Enter Company Name.")]
        [StringLength(100, MinimumLength = 4)]
        public string CompanyName { get; set; }
        public int? CompanyParentId { get; set; }
        public string CompanyParentName { get; set; }
        public bool locationBuildAllow { get; set; }
        public bool chkIsSameAddress { get; set; }
        [DataType(DataType.MultilineText)]
        [StringLength(200), Display(Name = "Address")]
        [Required(ErrorMessage = "Please Enter Company Address.")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Please Enter Email ID.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter valid Email ID.")]
        public string EmailId { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter valid Email ID.")]
        public string AltEmailId { get; set; }
        [Required(ErrorMessage = "Please Enter Mobile No.")]
        [StringLength(13, MinimumLength = 10)]
        [RegularExpression(@"^([7-9]{1})([0-9]{9})$", ErrorMessage = "Entered Mobile No format is not valid.")]
        public string MobileNo { get; set; }
        [Required(ErrorMessage = "Please Select Country.")]
        public int FK_CountryId { get; set; }
        [Required(ErrorMessage = "Please Select State.")]
        [Range(1, 9999, ErrorMessage = "Please Select State.")]
        public int FK_StateId { get; set; }
        [Required(ErrorMessage = "Please Select City.")]
        [Range(1, 9999, ErrorMessage = "Please Select City.")]
        public int FK_CityId { get; set; }
        public int FK_SalesHeadId { get; set; }
        public int FK_DealerId { get; set; }
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please Use Numbers only.")]
        [StringLength(6, MinimumLength = 4, ErrorMessage = "Entered Pincode must be 6 digits.")]
        public string Company_Pin { get; set; }
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please Use Numbers only.")]
        public string Company_Phone { get; set; }
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Enter valid contact person name")]
        public string ContactPerson { get; set; }
        [Required(ErrorMessage = "Please Enter Contact No.")]
        [StringLength(15, ErrorMessage = "Contact No length can't be greater than 15.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please Use Numbers only.")]
        public string ContactNo { get; set; }
        [StringLength(15, ErrorMessage = "Contact No length can't be greater than 15.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please Use Numbers only.")]
        public string ContactAltNo { get; set; }
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter valid Contact Email ID.")]
        public string ContactEmailId { get; set; }
        [StringLength(200), Display(Name = "Address")]
        public string Billing_Address { get; set; }
        public int? Billing_FkCountryId { get; set; }
        public int? Billing_FkStateId { get; set; }

        public int? Billing_FkCityId { get; set; }
        [StringLength(6, MinimumLength = 4, ErrorMessage = "Entered Pincode must be 6 digits.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please Use Numbers only.")]
        public string Billing_Pin { get; set; }
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please Use Numbers only.")]
        public string Billing_Phone { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public string countryname { get; set; }
        public string statename { get; set; }
        public string cityname { get; set; }
        public string UpdatedDate { get; set; }
        public string Operation { get; set; }



        [StringLength(15, ErrorMessage = "GST No. Must Be Alpha Numeric And Exactly 15 Characters.", MinimumLength = 15)]
        [RegularExpression(@"^([a-zA-Z0-9])+$", ErrorMessage = "Only Alphabets, Numerals With Total No. Of Characters 15, Is Allowed.")]
        //[RegularExpression(@"^([0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[0-9]{1}Z[0-9]{1})+$", ErrorMessage = "Invalid GST No. Format. Valid Format : 11AAAAA1111A1Z1")]
        public string GstNo { get; set; }

        [StringLength(10, ErrorMessage = "PAN No. Must Be Alpha Numeric And Exactly 10 Characters.", MinimumLength = 10)]
        [RegularExpression(@"^([a-zA-Z0-9])+$", ErrorMessage = "Only Alphabets, Numerals With Total No. Of Characters 10, Is Allowed.")]
        //[RegularExpression(@"^([A-Z]{5}[0-9]{4}[A-Z]{1})+$", ErrorMessage = "Invalid PAN No. Format. Valid Format : AAAAA1111A")]
        public string PanNo { get; set; }

        [StringLength(21, ErrorMessage = "CIN No. Must Be Alpha Numeric And Exactly 21 Characters.", MinimumLength = 21)]
        [RegularExpression(@"^([a-zA-Z0-9])+$", ErrorMessage = "Only Alphabets, Numerals With Total No. Of Characters 21, Is Allowed.")]
        public string CIN { get; set; }


        public HttpPostedFileBase CompanyLogo { get; set; }


        public string CompanyLogoName { get; set; }

        public string CurrentLogoURL { get; set; }
        public string DDLTitleHTML { get; set; }

    }
}
