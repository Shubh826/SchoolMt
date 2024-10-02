using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL
{
    public class GenrateDmdbillMdl
    {
        
        [Required(ErrorMessage = "Please Select Company Name.")]
        public int FK_CompanyId { get; set; }
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Please Select Class Name.")]
        public string ClassName { get; set; }

        public bool IsActive { get; set; }
        public string Status { get; set; }
        public int CreatedBy { get; set; }
    }
}
