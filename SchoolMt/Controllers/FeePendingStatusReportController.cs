using BAL;
using BAL.Common;
using FRGMBSystem.Common;
using MDL;
using SchoolMt.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolMt.Controllers
{
    public class FeePendingStatusReportController : Controller
    {
        FeePendingStatusReportBAL objBAL = null;

        public FeePendingStatusReportController()
        {
            objBAL = new FeePendingStatusReportBAL();
        }

        // GET: FeePendingStatusReport
        public ActionResult Index()
        {
            FeePendingStatusReportMDL obj = new FeePendingStatusReportMDL();
            ViewData["CompanyList"] = CommonBAL.FillCompany(SessionInfo.User.fk_companyid);
            ViewData["ClassList"] = CommonBAL.FillClassName(SessionInfo.User.fk_companyid);
            obj.FK_CompanyId = SessionInfo.User.fk_companyid;
            return View("Index", obj);
        }

        public PartialViewResult GetFeePendingStatusReport(int FK_CompanyId = 0, string ClassName = "", int CurrentPage = 1, string SearchBy = "", string SearchValue = "")
        {
            List<FeePendingStatusReportMDL> _List = objBAL.GetFeePendingStatusReport(
                FK_CompanyId, ClassName, 20, CurrentPage, SearchBy, SearchValue
            );
            //#region Calculate Due Amount
            //int startYear = DateTime.Now.Year;
            //int dueamount = 0;
            //int examfee = 0;
            //int duetrnsamount = 0;
            //string duemonthName = string.Empty;
            //foreach (FeePendingStatusReportMDL stu in _List)
            //{
            //    if(!string.IsNullOrEmpty(stu.ApplicableMonth))
            //    { 
            //    // Reset values for each student
            //    dueamount = 0;
            //    examfee = 0;
            //    duetrnsamount = 0;
            //    duemonthName = string.Empty;

            //    // Convert start month (ApplicableMonth)
            //    int startMonth = DateTime.ParseExact(stu.ApplicableMonth, "MMMM", CultureInfo.InvariantCulture).Month;

            //    // Convert student's end month (must be defined in model or database)
            //    int EndMonth = DateTime.Now.Month;


            //    // Loop for max 12 months
            //    for (int m = 0; m < 12; m++)
            //    {
            //        int currentMonth = (startMonth + m - 1) % 12 + 1;

            //        switch (currentMonth)
            //        {
            //            case 4:
            //                if (stu.AprilFee == 0 && stu.AprilTrnsFee == 0)
            //                {
            //                    duemonthName += ",April";
            //                    dueamount += stu.ApplicableMonthFee;
            //                    duetrnsamount += stu.ApplicableTrnsFee;
            //                }
            //                break;

            //            case 5:
            //                if (stu.MayFee == 0 && stu.MayTrnsFee == 0)
            //                {
            //                    duemonthName += ",May";
            //                    dueamount += stu.ApplicableMonthFee;
            //                    duetrnsamount += stu.ApplicableTrnsFee;
            //                }
            //                break;

            //            case 6:
            //                if (stu.JuneFee == 0)
            //                {
            //                    duemonthName += ",June";
            //                    dueamount += stu.ApplicableMonthFee;
            //                }
            //                break;

            //            case 7:
            //                if (stu.JulyFee == 0 && stu.JulyTrnsFee == 0)
            //                {
            //                    duemonthName += ",July";
            //                    dueamount += stu.ApplicableMonthFee;
            //                    duetrnsamount += stu.ApplicableTrnsFee;
            //                }
            //                break;

            //            case 8:
            //                if (stu.AugustFee == 0 && stu.AugustTrnsFee == 0)
            //                {
            //                    duemonthName += ",August";
            //                    dueamount += stu.ApplicableMonthFee;
            //                    duetrnsamount += stu.ApplicableTrnsFee;
            //                }
            //                break;

            //            case 9:
            //                if (stu.SeptemberFee == 0 && stu.SeptemberTrnsFee == 0)
            //                {
            //                    duemonthName += ",September";

            //                    if (stu.ExaminationFee1 == 0)
            //                        examfee = 500;

            //                    dueamount += stu.ApplicableMonthFee;
            //                    duetrnsamount += stu.ApplicableTrnsFee;
            //                }
            //                break;

            //            case 10:
            //                if (stu.OctoberFee == 0 && stu.OctoberTrnsFee == 0)
            //                {
            //                    if (stu.ExaminationFee1 == 0 && examfee == 0)
            //                        examfee = 500;

            //                    duemonthName += ",October";
            //                    dueamount += stu.ApplicableMonthFee;
            //                    duetrnsamount += stu.ApplicableTrnsFee;
            //                }
            //                break;

            //            case 11:
            //                if (stu.NovemberFee == 0 && stu.NovemberTrnsFee == 0)
            //                {
            //                    if (stu.ExaminationFee1 == 0 && examfee == 0)
            //                        examfee = 500;

            //                    duemonthName += ",November";
            //                    dueamount += stu.ApplicableMonthFee;
            //                    duetrnsamount += stu.ApplicableTrnsFee;
            //                }
            //                break;

            //            case 12:
            //                if (stu.DecemberFee == 0 && stu.DecemberTrnsFee == 0)
            //                {
            //                    if (stu.ExaminationFee1 == 0 && examfee == 0)
            //                        examfee = 500;

            //                    duemonthName += ",December";
            //                    dueamount += stu.ApplicableMonthFee;
            //                    duetrnsamount += stu.ApplicableTrnsFee;
            //                }
            //                break;

            //            case 1:
            //                if (stu.JanuaryFee == 0 && stu.JanuaryTrnsFee == 0)
            //                {
            //                    if (stu.ExaminationFee1 == 0 && examfee == 0)
            //                        examfee = 500;

            //                    duemonthName += ",January";
            //                    dueamount += stu.ApplicableMonthFee;
            //                    duetrnsamount += stu.ApplicableTrnsFee;
            //                }
            //                break;

            //            case 2:
            //                if (stu.FebruaryFee == 0 && stu.FebruaryTrnsFee == 0)
            //                {
            //                    if (stu.ExaminationFee1 == 0 && examfee == 0)
            //                        examfee = 500;

            //                    duemonthName += ",February";
            //                    dueamount += stu.ApplicableMonthFee;

            //                    if (stu.ExaminationFee2 == 0)
            //                        examfee += 500;

            //                    duetrnsamount += stu.ApplicableTrnsFee;
            //                }
            //                break;

            //            case 3:
            //                if (stu.MarchFee == 0 && stu.MarchTrnsFee == 0)
            //                {
            //                    if (stu.ExaminationFee1 == 0 && examfee == 0)
            //                        examfee = 500;

            //                    if (stu.ExaminationFee2 == 0 && examfee == 0)
            //                        examfee = 500;

            //                    duemonthName += ",March";
            //                    dueamount += stu.ApplicableMonthFee;
            //                    duetrnsamount += stu.ApplicableTrnsFee;
            //                }
            //                break;
            //        }

            //        if (currentMonth == EndMonth)
            //            break;
            //    }

            //    if (duemonthName.StartsWith(","))
            //        duemonthName = duemonthName.Substring(1);

            //    // Assign back to model
            //    stu.DueAmount = dueamount;
            //    stu.DueTransportAmount = duetrnsamount;
            //    stu.DueExamFee = examfee;
            //    stu.DueMonths = duemonthName;
            //    }
            //}
            //#endregion
            //_List = _List.OrderByDescending(x => x.DueAmount).ToList();

            return PartialView("_FeePendingStatusReport", _List);
        }

        public JsonResult GetDataForExport(int FK_CompanyId = 0, string ClassName = "", int CurrentPage = 1, string SearchBy = "", string SearchValue = "")
        {
            try
            {
               
                List<FeePendingStatusReportMDL> _List = new List<FeePendingStatusReportMDL>();
                _List = objBAL.GetFeePendingStatusReport(FK_CompanyId, ClassName, Convert.ToInt32(20), CurrentPage, SearchBy, SearchValue);
                //#region Calculate Due Amount
                //int startYear = DateTime.Now.Year;
                //int dueamount = 0;
                //int examfee = 0;
                //int duetrnsamount = 0;
                //string duemonthName = string.Empty;
                //foreach (FeePendingStatusReportMDL stu in _List)
                //{
                //    if (!string.IsNullOrEmpty(stu.ApplicableMonth))
                //    {
                //        // Reset values for each student
                //        dueamount = 0;
                //        examfee = 0;
                //        duetrnsamount = 0;
                //        duemonthName = string.Empty;

                //        // Convert start month (ApplicableMonth)
                //        int startMonth = DateTime.ParseExact(stu.ApplicableMonth, "MMMM", CultureInfo.InvariantCulture).Month;

                //        // Convert student's end month (must be defined in model or database)
                //        int EndMonth = DateTime.Now.Month;


                //        // Loop for max 12 months
                //        for (int m = 0; m < 12; m++)
                //        {
                //            int currentMonth = (startMonth + m - 1) % 12 + 1;

                //            switch (currentMonth)
                //            {
                //                case 4:
                //                    if (stu.AprilFee == 0 && stu.AprilTrnsFee == 0)
                //                    {
                //                        duemonthName += ",April";
                //                        dueamount += stu.ApplicableMonthFee;
                //                        duetrnsamount += stu.ApplicableTrnsFee;
                //                    }
                //                    break;

                //                case 5:
                //                    if (stu.MayFee == 0 && stu.MayTrnsFee == 0)
                //                    {
                //                        duemonthName += ",May";
                //                        dueamount += stu.ApplicableMonthFee;
                //                        duetrnsamount += stu.ApplicableTrnsFee;
                //                    }
                //                    break;

                //                case 6:
                //                    if (stu.JuneFee == 0)
                //                    {
                //                        duemonthName += ",June";
                //                        dueamount += stu.ApplicableMonthFee;
                //                    }
                //                    break;

                //                case 7:
                //                    if (stu.JulyFee == 0 && stu.JulyTrnsFee == 0)
                //                    {
                //                        duemonthName += ",July";
                //                        dueamount += stu.ApplicableMonthFee;
                //                        duetrnsamount += stu.ApplicableTrnsFee;
                //                    }
                //                    break;

                //                case 8:
                //                    if (stu.AugustFee == 0 && stu.AugustTrnsFee == 0)
                //                    {
                //                        duemonthName += ",August";
                //                        dueamount += stu.ApplicableMonthFee;
                //                        duetrnsamount += stu.ApplicableTrnsFee;
                //                    }
                //                    break;

                //                case 9:
                //                    if (stu.SeptemberFee == 0 && stu.SeptemberTrnsFee == 0)
                //                    {
                //                        duemonthName += ",September";

                //                        if (stu.ExaminationFee1 == 0)
                //                            examfee = 500;

                //                        dueamount += stu.ApplicableMonthFee;
                //                        duetrnsamount += stu.ApplicableTrnsFee;
                //                    }
                //                    break;

                //                case 10:
                //                    if (stu.OctoberFee == 0 && stu.OctoberTrnsFee == 0)
                //                    {
                //                        if (stu.ExaminationFee1 == 0 && examfee == 0)
                //                            examfee = 500;

                //                        duemonthName += ",October";
                //                        dueamount += stu.ApplicableMonthFee;
                //                        duetrnsamount += stu.ApplicableTrnsFee;
                //                    }
                //                    break;

                //                case 11:
                //                    if (stu.NovemberFee == 0 && stu.NovemberTrnsFee == 0)
                //                    {
                //                        if (stu.ExaminationFee1 == 0 && examfee == 0)
                //                            examfee = 500;

                //                        duemonthName += ",November";
                //                        dueamount += stu.ApplicableMonthFee;
                //                        duetrnsamount += stu.ApplicableTrnsFee;
                //                    }
                //                    break;

                //                case 12:
                //                    if (stu.DecemberFee == 0 && stu.DecemberTrnsFee == 0)
                //                    {
                //                        if (stu.ExaminationFee1 == 0 && examfee == 0)
                //                            examfee = 500;

                //                        duemonthName += ",December";
                //                        dueamount += stu.ApplicableMonthFee;
                //                        duetrnsamount += stu.ApplicableTrnsFee;
                //                    }
                //                    break;

                //                case 1:
                //                    if (stu.JanuaryFee == 0 && stu.JanuaryTrnsFee == 0)
                //                    {
                //                        if (stu.ExaminationFee1 == 0 && examfee == 0)
                //                            examfee = 500;

                //                        duemonthName += ",January";
                //                        dueamount += stu.ApplicableMonthFee;
                //                        duetrnsamount += stu.ApplicableTrnsFee;
                //                    }
                //                    break;

                //                case 2:
                //                    if (stu.FebruaryFee == 0 && stu.FebruaryTrnsFee == 0)
                //                    {
                //                        if (stu.ExaminationFee1 == 0 && examfee == 0)
                //                            examfee = 500;

                //                        duemonthName += ",February";
                //                        dueamount += stu.ApplicableMonthFee;

                //                        if (stu.ExaminationFee2 == 0)
                //                            examfee += 500;

                //                        duetrnsamount += stu.ApplicableTrnsFee;
                //                    }
                //                    break;

                //                case 3:
                //                    if (stu.MarchFee == 0 && stu.MarchTrnsFee == 0)
                //                    {
                //                        if (stu.ExaminationFee1 == 0 && examfee == 0)
                //                            examfee = 500;

                //                        if (stu.ExaminationFee2 == 0 && examfee == 0)
                //                            examfee = 500;

                //                        duemonthName += ",March";
                //                        dueamount += stu.ApplicableMonthFee;
                //                        duetrnsamount += stu.ApplicableTrnsFee;
                //                    }
                //                    break;
                //            }

                //            if (currentMonth == EndMonth)
                //                break;
                //        }

                //        if (duemonthName.StartsWith(","))
                //            duemonthName = duemonthName.Substring(1);

                //        // Assign back to model
                //        stu.DueAmount = dueamount;
                //        stu.DueTransportAmount = duetrnsamount;
                //        stu.DueExamFee = examfee;
                //        stu.DueMonths = duemonthName;
                //    }
                //}
                //#endregion
                //_List = _List.OrderByDescending(x => x.DueAmount).ToList();
                TempData["FeePendingStatuslist"] = _List;
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }


        public FileResult ExportToExcel()
        {
            TempData.Keep();
            List<FeePendingStatusReportMDL> _listForExcel = (List<FeePendingStatusReportMDL>)TempData["FeePendingStatuslist"];

            //string[] columns = { "Student Name", "Class Name", "Class Code", "Father Name", "Mother Name", "Address", "Previous Due Amount", "Examination Fee1", "Examination Fee2", "Applicable Month Fee", "Month Due Fee", "Transport Due Fee","Total Due Amount" };
            //string MDLAttr = "StudentName,ClassName,ClassCode,FatherName,MotherName,Address,PreviousDueAmount,ExaminationFee1,ExaminationFee2,ApplicableMonthFee,MonthdueFee,TransportdueFee,TotalDueAmount";

            string[] columns = { "Student Name", "Class Name", "Class Code", "Father Name", "Mother Name", "Address", 
                "Due Amount" };
            string MDLAttr = "StudentName,ClassName,ClassCode,FatherName,MotherName,Address,TotalDueAmount";
            ExcelExportHelper objExcelExportHelper = new ExcelExportHelper();
            return objExcelExportHelper.ExportExcelByClosedXmlForPendingFeeStatusReport(_listForExcel, "Fee Pending Status Report", ".xls", MDLAttr, columns);
        }
    }
}