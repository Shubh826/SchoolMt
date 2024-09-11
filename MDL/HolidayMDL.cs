using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL
{
    public class HolidayMDL
    {
        public int FK_CompanyId { get; set; }
        public int FK_CityId { get; set; }
        public string HolidayDate { get; set; }
        public string[] WeekDays { get; set; }
        public string HolidayName { get; set; }
        public string Day { get; set; }
        public string Month { get; set; }
        public string[] Applicability { get; set; }
        public string CityName { get; set; }
        public int CreatedBy { get; set; }
        public int StateId { get; set; }
        public string Statename { get; set; }
        public int PK_HolidayId { get; set; }
        public string CityList { get; set; }
        public int FK_City_Id { get; set; }
        public string HolidayType { get; set; }
        public bool IsActive { get; set; }


    }
}


