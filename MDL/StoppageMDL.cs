using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL
{
    public class StoppageMDL
    {
        public int Pk_StoppageID { get; set; }
        [Required(ErrorMessage = "Please Enter Stoppage Name.")]
        [StringLength(25, MinimumLength = 4, ErrorMessage = "Stoppage Minimum Length Should Be 4 and Maximum Length Should Be 250")]
        public string StoppageName { get; set; }
        [Required(ErrorMessage = "Please Select Company Name.")]
        public int FK_CompanyID { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        [Required(ErrorMessage = "Please Select Color.")]
        //[StringLength(25, MinimumLength = 4, ErrorMessage = "Please select Color.")]
        public string ColorCode { get; set; }
        [Required(ErrorMessage = "Please Select Fence Type.")]
        public string FenceType { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string CenterLat { get; set; }
        public string CenterLong { get; set; }
        [Range(0, 1000, ErrorMessage = "Radius Range Cannot Exceed 1000")]
        public string radius { get; set; }
        public bool Is_Client_Location { get; set; }
        public string Location { get; set; }
        public string CompanyName { get; set; }
    }
}
