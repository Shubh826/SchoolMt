using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL
{
    public class FormMappingViewModel
    {
        public int PK_FormRoleId { get; set; }
        public int FK_RoleId { get; set; }
        public int FK_FormId { get; set; }
        public int FK_CompanyID { get; set; }
        public int CreatedBy { get; set; }
        //added on 16 march 2018 (for adding whether the mapping is for mobile app or web app) **start
        public string MappingFor { get; set; }
        public string Mapping { get; set; }
        // ** end
        public List<RoleMapping> Forms { get; set; }

    }
    public class FormsMDL
    {

        public int PK_FormId { get; set; }
        public string FormName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public int FK_ParentId { get; set; }
        public int FK_MainId { get; set; }
        public int LevelId { get; set; }
        public int SortId { get; set; }
        public string Image { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanView { get; set; }
        public bool CanDelete { get; set; }
        public List<SubForms> SubForms { get; set; }
        public string ClassName { get; set; }
        public int HomePage { get; set; }
        public string Area { get; set; }
    }
    public class SubForms
    {

        public int PK_FormId { get; set; }
        public string FormName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public int FK_ParentId { get; set; }
        public int FK_MainId { get; set; }
        public int LevelId { get; set; }
        public int SortId { get; set; }
        public string Image { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanView { get; set; }
        public bool CanDelete { get; set; }
        public string Area { get; set; }
    }
    public class FormCompanyMappingModel
    {
        public int PK_FormCompanyId { get; set; }
        // public int FK_RoleId { get; set; }
        public int FK_FormId { get; set; }
        public int FK_CompanyID { get; set; }
        public int CreatedBy { get; set; }
        //added on 16 march 2018 (for adding whether the mapping is for mobile app or web app) **start
        public string MappingFor { get; set; }
        public string Mapping { get; set; }
        // ** end
        public List<CompanyMappingMDL> Forms { get; set; }

    }
}
