using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL
{
    public class FormMasterMDL
    {
        public int Pk_FormId { get; set; }
        [Required(ErrorMessage = "Please Select Form Name.")]
        public string FormName { get; set; }
        [Required(ErrorMessage = "Please Select Controller Name.")]
        public string ControllerName { get; set; }
        [Required(ErrorMessage = "Please Select Action Name.")]
        public string ActionName { get; set; }
        //[Required(ErrorMessage = "Please Select Parent Form.")]
        public int? Fk_ParentId { get; set; }
        public int Fk_MainId { get; set; }
        public int LevelId { get; set; }

        public string ParentForm { get; set; }
        public int sortId { get; set; }
        public string ClassName { get; set; }
        public string AreaName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
        public string FormFor { get; set; }
        public List<FormMasterMDL> Forms { get; set; }
    }
}
