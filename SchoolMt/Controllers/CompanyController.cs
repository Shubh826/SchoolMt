using BAL;
using BAL.Common;
using MDL;
using MDL.Common;
using Newtonsoft.Json;
using SchoolMt.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SchoolMt.Controllers
{
    public class CompanyController : Controller
    {
        private List<CompanyMDL> _Companylist;
        CompanyBAL objCompanyBal = null;
        BasicPagingMDL objBasicPagingMDL = null;

        public CompanyController()
        {
            objCompanyBal = new CompanyBAL();
        }
        // GET: Company
        public ActionResult Index()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Msg = (Messages)TempData["Message"];
                TempData["Message"] = null;
            }

            CompanyMDL objCompanyMDL = new CompanyMDL();
            return View();
        }
        public PartialViewResult getCompany(int CurrentPage = 1, string SearchBy = "", string SearchValue = "")
        {
            CompanyMDL objCompanyMDL = new CompanyMDL();

            objCompanyBal.getCompanyDetails(out _Companylist, out objBasicPagingMDL, 0, SessionInfo.User.fk_companyid, Convert.ToInt32(20), CurrentPage, SearchBy, SearchValue);

            ViewBag.paging = objBasicPagingMDL;
            return PartialView("_CompanyGrid", _Companylist);
        }
        [HttpGet]
        public ActionResult AddEditCompany(int id = 0)
        {
            ViewData["countrylist"] = CommonBAL.FillCountry();
            ViewData["companylist"] = CommonBAL.FillCompany(SessionInfo.User.fk_companyid);

            if (id != 0)
            {
                objCompanyBal.getCompanyDetails(out _Companylist, out objBasicPagingMDL, id, id);
                return View("AddEdit", _Companylist[0]);
            }
            else
            {
                CompanyMDL obj = new CompanyMDL();
                obj.IsActive = true;
                obj.locationBuildAllow = true;
                return View("AddEdit", obj);

            }
        }
        [HttpPost]
        public ActionResult AddEditCompany(CompanyMDL companyMDL)
        {
            ViewData["countrylist"] = CommonBAL.FillCountry();
            ViewData["companylist"] = CommonBAL.FillCompany(SessionInfo.User.fk_companyid);
            companyMDL.CreatedBy = SessionInfo.User.userid;

            Messages msg = new Messages();

            string ServerImagePath = Server.MapPath("/App_Images/" + "CompanyLogo" + "/");

            if (ModelState.IsValid && companyMDL.CompanyName != null)
            {
                if (companyMDL.CompanyParentId == null)
                {
                    companyMDL.CompanyParentId = 0;
                }

                #region Company LOGO Document CODE
                if (companyMDL.CompanyLogo != null)
                {
                    ImageError CompLogo = VTSFileHelper.CheckValidImageFile(companyMDL.CompanyLogo);
                    if (ImageError.None == CompLogo)
                    {
                        companyMDL.CompanyLogoName = VTSFileHelper.ResetFileNames(companyMDL.CompanyLogo, Convert.ToString(companyMDL.CompanyId));
                    }

                    DeleteExistingFile(ServerImagePath + companyMDL.CompanyLogoName);//DELETES IF FILE EXISTS BEFORE UPLOADING

                    bool CompanyDocSaveStatus = VTSFileHelper.Upload(companyMDL.CompanyLogo, ServerImagePath, companyMDL.CompanyLogoName);

                    if (CompanyDocSaveStatus == true)
                    {
                        msg = objCompanyBal.AddEditCompany(companyMDL);

                        if (msg.Message_Id != 1)
                        {
                            DeleteExistingFile(ServerImagePath + companyMDL.CompanyLogoName);//DELETES THE SAVED FILE IF SAVING TO DB FAILS
                        }
                    }
                }

                else
                {
                    if (!string.IsNullOrEmpty(companyMDL.CurrentLogoURL))
                    {
                        DeleteExistingFile(ServerImagePath + companyMDL.CurrentLogoURL);
                    }
                    msg = objCompanyBal.AddEditCompany(companyMDL);
                }
            }
            #endregion
            else
            {
                msg.Message = "Failed to add/edit company";
            }
            msg.Message = "Company " + msg.Message;
            TempData["Message"] = msg;
            return RedirectToAction("Index");
            return View("Index");
        }

        public JsonResult getstate(int countryid)
        {
            return Json(CommonBAL.FillState(countryid), JsonRequestBehavior.AllowGet);
        }

        public JsonResult getcity(int stateid)
        {
            return Json(CommonBAL.FillCity(stateid), JsonRequestBehavior.AllowGet);
        }




        public string DeleteExistingFile(string FullFilePath)
        {
            if (System.IO.File.Exists(@FullFilePath))
            {
                System.IO.File.Delete(@FullFilePath);
                return "SUCCESS";
            }
            else
            {
                return "NOT_EXISTS";
            }
        }

    }
}