using BAL;
using BAL.Common;
using SchoolMt.Controllers;
using MDL;
using MDL.Common;
using SchoolMt.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using FRGMBSystem.Controllers;

namespace SchoolMt.Controllers
{
    public class StudentMasterController : BasicController
    {
        // GET: StudentMaster
        private List<StudentMasterMDL> _Studentlist;
        List<StudentMasterMDL> _stuLst;
        BasicPagingMDL objBasicPagingMDL = null;
        private StudentMasterBAL objStudentMasterBal;
        StoppageMstBAL objStoppageMstBAL = null;
        public StudentMasterController()
        {
            objStudentMasterBal = new StudentMasterBAL();
            objStoppageMstBAL = new StoppageMstBAL();
        }
        public ActionResult Index()
        {
            ViewBag.InvalidLink = false;
            ViewData["companylist"] = CommonBAL.FillCompany(SessionInfo.User.fk_companyid);
            //ViewData["Shiftlist"] = CommonBAL.BindShift(SessionInfo.User.fk_companyid);
            if (TempData["Message"] != null)
            {
                ViewBag.Msg = (Messages)TempData["Message"];
                TempData["Message"] = null;
            }
            return View();
        }
        public PartialViewResult GetStudentData(int CurrentPage = 1, string SearchBy = "", string SearchValue = "")
        {
            objStudentMasterBal.GetStudentData(out _Studentlist, out objBasicPagingMDL, 0, SessionInfo.User.fk_companyid, Convert.ToInt32(20), CurrentPage, SearchBy, SearchValue);
            foreach (var r in _Studentlist)
            {
                if (r.ImageName != null && r.ImageName != "" && r.ImageName != "NA" && r.ImageName != "N/A")
                {
                    r.profileImage = Server.MapPath("/App_Images/SBTMSStudentImages/StudentImage/") + r.ImageName;
                }
                else
                {
                    r.profileImage = "";
                }
            }

            ViewBag.paging = objBasicPagingMDL;
            TempData["studentlist"] = _Studentlist;
            return PartialView("_StudentMasterGrid", _Studentlist);
        }
        [HttpGet]
        public ActionResult AddEditStudent(int id = 0)
        {
            ViewData["companylist"] = CommonBAL.FillCompany(SessionInfo.User.fk_companyid);
            ViewData["Classlist"] = CommonBAL.FillClass();
            ViewData["ClassCodelist"] = CommonBAL.FillClassCode();
            ViewData["Arealist"] = CommonBAL.BindArea(SessionInfo.User.fk_companyid);

            if (id > 0)
            {
                objStudentMasterBal.GetStudentData(out _Studentlist, out objBasicPagingMDL, id, SessionInfo.User.fk_companyid);
                return View("AddEditStudent", _Studentlist[0]);
            }
            else
            {
                StudentMasterMDL obj = new StudentMasterMDL();
                obj.FK_CompanyId = 1;
                obj.CompanyId = 1;
                //obj.vCompanyId = 1;
                obj.IsActive = true;
                return View("AddEditStudent", obj);
            }
        }
        [HttpPost]
        public ActionResult AddEditStudent(StudentMasterMDL objStudentMasterMDL)
        {
            ViewData["companylist"] = CommonBAL.FillCompany(SessionInfo.User.fk_companyid);
            ViewData["Classlist"] = CommonBAL.FillClass();
            ViewData["ClassCodelist"] = CommonBAL.FillClassCode();
            ViewData["Arealist"] = CommonBAL.BindArea(SessionInfo.User.fk_companyid);
            // Profile Pic upload[Code Start]
            #region
            if (objStudentMasterMDL.StudentImage != null)
            {
                string ServerImagePath = Server.MapPath("/App_Images/" + "SBTMSStudentImages/StudentImage/");
                ImageError ErrorEmpImg = VTSFileHelper.CheckValidImage(objStudentMasterMDL.StudentImage);
                if (ImageError.None == ErrorEmpImg)
                {
                    objStudentMasterMDL.ImageName = VTSFileHelper.ResetFileNames(objStudentMasterMDL.StudentImage, "_StudentImage");

                    DeleteIfFileExists(ServerImagePath + objStudentMasterMDL.StudentImage);//DELETES IF FILE EXISTS BEFORE UPLOADING   
                    objStudentMasterMDL.StudentImageUrl = ServerImagePath + objStudentMasterMDL.ImageName;
                }

                bool ProfileImage = VTSFileHelper.Upload(objStudentMasterMDL.StudentImage, ServerImagePath, objStudentMasterMDL.ImageName);

                if (ProfileImage == false)
                {
                    objStudentMasterMDL.CompanyId = SessionInfo.User.fk_companyid;

                    return View("AddEditStudent", objStudentMasterMDL);
                }

            }
            #endregion
            // Profile Pic upload[Code End]


            int roleId = SessionInfo.User.roleid;
            objStudentMasterMDL.CreatedBy = SessionInfo.User.userid;
            objStudentMasterMDL.FK_CompanyId = SessionInfo.User.fk_companyid;
            if (ModelState.IsValid)
            {
                Messages msg = objStudentMasterBal.InsertStudentData(objStudentMasterMDL);
                TempData["Message"] = msg;
                return RedirectToAction("Index");
            }
            return View("AddEditStudent");


        }
        public string DeleteIfFileExists(string FileFullPath)
        {
            if (System.IO.File.Exists(@FileFullPath))
            {
                System.IO.File.Delete(@FileFullPath);
                return "Existing File Deleted";
            }
            else
            {
                return "File Does Not Exists.";
            }
        }
    }
}