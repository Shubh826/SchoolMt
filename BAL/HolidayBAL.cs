using DAL;
using MDL;
using MDL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class HolidayBAL
    {
        HolidayDAL objHolidayMasterDAL = null;
        public HolidayBAL()
        {
            objHolidayMasterDAL = new HolidayDAL();
        }
        public bool GetHolidayDetails(out List<HolidayMDL> _HolidayDetailsMDL, int PK_HolidayID, string SearchBy, string SearchValue)
        {
            _HolidayDetailsMDL = new List<HolidayMDL>();
            return objHolidayMasterDAL.GetHolidayDetails(out _HolidayDetailsMDL, PK_HolidayID, SearchBy, SearchValue);


        }
        public Messages DeleteHoliday(int HolidayId)
        {

            return objHolidayMasterDAL.DeleteHoliday(HolidayId);
        }

        public Messages AddEditHolidayDetails(string[] CityIds, string[] WeekDay, int PK_HolidayId = 0, string Holidaydate = "", string HolidayName = "", string HolidayType = "", int CompanyId = 0, int UserId = 0)//HolidayMDL objHolidayMDL
        {
            return objHolidayMasterDAL.AddEditHolidayDetails(CityIds, WeekDay, PK_HolidayId, Holidaydate, HolidayName, HolidayType, CompanyId, UserId);
        }

    }
}
