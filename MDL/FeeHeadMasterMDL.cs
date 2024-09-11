using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL
{
    public class FeeHeadMasterMDL
    {
        [Required(ErrorMessage = "Please Select School Name.")]
        public int FK_CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int PK_FeeHeadId { get; set; }
        [Required(ErrorMessage = "Please Select Fee Head Name.")]
        public string FeeHeadName { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
        public int CreatedBy { get; set; }
    }
}
