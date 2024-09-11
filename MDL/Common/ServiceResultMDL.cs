using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL.Common
{
    public class ServiceResultMDL<X>
    {


        public string Message { get; set; }


        public string Description { get; set; }


        public bool Result { get; set; }


        public X Data { get; set; }



    }
}
