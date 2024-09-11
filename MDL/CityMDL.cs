using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL
{
    public class CityMDL
    {
        [Required(ErrorMessage = "Please Select Country.")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "Please Select State.")]
        public int StateId { get; set; }

        public int CityId { get; set; }

        public string CountryName { get; set; }

        public string StateName { get; set; }

        [Required(ErrorMessage = "Please Enter City Name.")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Please Use letters only.")]
        public string CityName { get; set; }

        [Required(ErrorMessage = "Please Enter City Abbreviation.")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Please Use letters only.")]
        [StringLength(10, ErrorMessage = "Abbreviation Must be between 2 and 10 characters", MinimumLength = 2)]
        public string Abbr { get; set; }

        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        [Required(ErrorMessage = "Please Enter Company Name.")]
        public int FK_CompanyId { get; set; }
        public int fk_cityId { get; set; }
    }
}
