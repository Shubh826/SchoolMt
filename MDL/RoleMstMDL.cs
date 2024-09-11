using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MDL
{
    public class RoleMstMDL
    {
        public bool IsCompany { get; set; }
        public bool IsClient { get; set; }
        [Required(ErrorMessage = "Please Select Company Name.")]
        public int FK_CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int FK_ClientId { get; set; }
        public string ClientName { get; set; }
        public int PK_RoleId { get; set; }
        [Required(ErrorMessage = "Please Select Role Name.")]
        public string RoleName { get; set; }
        public int FK_FormId { get; set; }
        public string FormName { get; set; }
        public string HomePage { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
        public int CreatedBy { get; set; }
    }
}
