using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL
{
   public class CompanyProMDL
    {
        public int Pk_ComProId { get; set; }
        public int FK_ClientId { get; set; }
        public int Fk_CompanyID { get; set; }
        public int Fk_userID { get; set; }
        public int CreatedON { get; set; }
        public int CreatedBy { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNumber { get; set; }
        public string ContactEmail { get; set; }
        public string WebsiteUrL { get; set; }
        public string location { get; set; }
        public string GSTIN { get; set; }
        public string otherLocation { get; set; }
        public string OtherContactPerson { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public string Designation { get; set; }
        public string Industry { get; set; }
        public string AnnualRevenue { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }

    }
}
