using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL.Common
{
    public class BasicPagingMDL
    {
        public int TotalItem { get; set; }
        public int CurrentPage { get; set; }
        public int RowParPage { get; set; }
        public int TotalPage { get; set; }
    }
}
