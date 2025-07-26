using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL
{
    public class PostStudentImageMDL
    {
        public int ClassId { get; set; }
        public int StudentId { get; set; }
        public string SectionName { get; set; }
        public string StudentImageURL { get; set; }
    }
}
