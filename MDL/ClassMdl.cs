using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL
{
    public class ClassMdl
    {
        [Required(ErrorMessage = "Please Select Company.")]
        public int CompID { get; set; }
        public string CompName { get; set; }
        public int ClassId { get; set; }
        [Required(ErrorMessage = "Please Select Class.")]
        [RegularExpression("^[a-zA-Z]+$|^[0-9]+$", ErrorMessage = "Class Name must be alphabetic or numeric only.")]
        public string ClassName { get; set; }

        [Required(ErrorMessage = "Monthly Fee is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Monthly Fee must be a positive number.")]
        public int MonthlyFee { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy{ get; set; }
        [Required(ErrorMessage = "Status is required.")]
        public string Status { get; set; }
    }
}
