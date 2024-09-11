using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL
{
    public class CompanyMappingMDL
    {
        public int PK_FormCompanyId { get; set; }
        public int FK_FormId { get; set; }
        //public int FK_RoleId { get; set; }
        public int PK_MenuId { get; set; }
        public string FormName { get; set; }
        public bool CanAll { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanView { get; set; }
        public bool CanDelete { get; set; }
        public int CreatedBy { get; set; }
        public int CompanyId { get; set; }
    }
}
