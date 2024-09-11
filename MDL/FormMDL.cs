using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL
{
    public class FormMDL
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
}
