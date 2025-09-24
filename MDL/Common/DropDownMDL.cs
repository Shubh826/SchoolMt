using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL.Common
{
    public class DropDownMDL
    {
        public Int64 ID { get; set; }
        public string Value { get; set; }
    }

    public class LookUpDropDownMDL
    {
        public Int64 ID { get; set; }
        public string Value { get; set; }
        public bool? IsSelected { get; set; }
        public string TotalMark { get; set; }
        public string ObtainMark { get; set; }


    }

  
}
