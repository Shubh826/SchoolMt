using BAL;
using BAL.Common;
using MDL;
using MDL.Common;
using SchoolMt.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolMt.Controllers
{
    public class HolidayMasterController : Controller
    {
        // GET: HolidayMaster
        private List<HolidayMDL> _Holidaylist;
        HolidayBAL objHolidayMasterBAL = null;
        HolidayMDL objHolidayMDL = null;
        BasicPagingMDL objBasicPagingMDL = null;
        public HolidayMasterController()
        {

            objHolidayMasterBAL = new HolidayBAL();
        }
        // GET: /HolidayMaster
        public ActionResult Index()
        {

            if (TempData["Message"] != null)
            {
                ViewBag.Msg = (Messages)TempData["Message"];
            }

            return View();

        }


        public PartialViewResult getHolidayDetails(int CurrentPage = 1, string SearchBy = "", string SearchValue = "")
        {
            objHolidayMasterBAL.GetHolidayDetails(out _Holidaylist, 0, SearchBy, SearchValue);
            //ViewBag.paging = objBasicPagingMDL;
            return PartialView("_HolidayListGrid", _Holidaylist);
        }
        [HttpGet]
        public ActionResult AddEditHolidayDetails(int PK_HolidayId = 0, string Holidaydate = "", string HolidayName = "", string CityIds = "", string SearchBy = "", string SearchValue = "")
        {
            objHolidayMDL = new HolidayMDL();
            List<HolidayMDL> _Holidaylist = new List<HolidayMDL>();
            if (PK_HolidayId != 0)
            {
                objHolidayMasterBAL.GetHolidayDetails(out _Holidaylist, PK_HolidayId, SearchBy, SearchValue);
                return View("AddEditHolidayDetails", _Holidaylist[0]);
            }
            else
            {
                HolidayMDL obj = new HolidayMDL();
                obj.FK_CompanyId = SessionInfo.User.fk_companyid;
                obj.IsActive = true;
                return View("AddEditHolidayDetails", obj);
            }
            
        }

        [HttpPost]
        public ActionResult AddEditHolidayDetails(string[] CityIds, string[] WeekDay, string Holidaydate = "", string HolidayName = "", string HolidayType = "Other", int PKHolidayId = 0)
        {
            Messages msg = objHolidayMasterBAL.AddEditHolidayDetails(CityIds, WeekDay, PKHolidayId, Holidaydate, HolidayName, HolidayType, Convert.ToInt32(SessionInfo.User.fk_companyid), Convert.ToInt32(SessionInfo.User.userid));
            return Json(msg.Message, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCities()
        {
            return Json(CommonBAL.BindCity(3), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult DeleteHoliday(int HolidayId)
        {
            Messages msg = objHolidayMasterBAL.DeleteHoliday(HolidayId);
            return Json(msg.Message, JsonRequestBehavior.AllowGet);
        }
    }
}