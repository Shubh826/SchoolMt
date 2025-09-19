using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL
{
    public class LookUpMDL
    {
        public int PkId { get; set; }
        public string LookupName { get; set; }
        public bool IsActive { get; set; }
        public int UserId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string status { get; set; }
    }
}
