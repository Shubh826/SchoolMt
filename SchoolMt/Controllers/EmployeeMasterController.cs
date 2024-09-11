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
    public class EmployeeMasterController : Controller
    {
        private List<EmployeeMasterMDL> _Employeelist;

        public List<EmployeeMasterMDL> ValidEmployeelist;
        public List<EmployeeMasterMDL> InValidEmployeelist;

        BasicPagingMDL objBasicPagingMDL = null;
        EmployeeMasterBAL objEmployeeMasterBAL = null;
        public EmployeeMasterController()
        {
            objEmployeeMasterBAL = new EmployeeMasterBAL();
        }
        public ActionResult Index()
        {
           
            ViewData["companylist"] = CommonBAL.FillCompany(SessionInfo.User.fk_companyid);
            ViewData["IDCardlist"] = CommonBAL.FillIdCard();

            if (TempData["Message"] != null)
            {
                ViewBag.Msg = (Messages)TempData["Message"];
                TempData["Message"] = null;
            }
            return View();
        }
        public PartialViewResult getEmployee(int CurrentPage = 1, string SearchBy = "", string SearchValue = "")
        {
            objEmployeeMasterBAL.getEmployee(out _Employeelist, out objBasicPagingMDL, 0, SessionInfo.User.fk_companyid, SessionInfo.User.userid, Convert.ToInt32(20), CurrentPage, SearchBy, SearchValue);
            foreach (var r in _Employeelist)
            {
                if (r.ImageName != null && r.ImageName != "" && r.ImageName != "NA" && r.ImageName != "N/A")
                {
                    r.profileImage = Server.MapPath("/App_Images/SBTMSEMPImages/EMPImage/") + r.ImageName;
                }
                else
                {
                    r.profileImage = "";
                }
                if (r.EMPImageName != null && r.EMPImageName != "" && r.EMPImageName != "NA" && r.EMPImageName != "N/A")
                {
                    r.EmpProofprofileImage = Server.MapPath("/App_Images/SBTMSEMPPoofImages/EMPProofImage/") + r.EMPImageName;
                }
                else
                {
                    r.EmpProofprofileImage = "";
                }
            }
            ViewBag.paging = objBasicPagingMDL;
            return PartialView("_EmployeeGrid", _Employeelist);
        }

        [HttpGet]
        public ActionResult AddEmployee(int id = 0)
        {
            ViewData["companylist"] = CommonBAL.FillCompany(SessionInfo.User.fk_companyid);
            ViewData["IDCardlist"] = CommonBAL.FillIdCard();

            if (id != 0)
            {
                objEmployeeMasterBAL.getEmployee(out _Employeelist, out objBasicPagingMDL, id, SessionInfo.User.fk_companyid, SessionInfo.User.userid, Convert.ToInt32(20));
                return View("AddEditEmployee", _Employeelist[0]);
            }
            else
            {
                return View("AddEditEmployee", new EmployeeMasterMDL());
            }
        }


        [HttpPost]
        public ActionResult AddEmployee(EmployeeMasterMDL objEmployeeMasterMDL)
        {
            objEmployeeMasterMDL.CreatedBy = SessionInfo.User.userid;

            // Profile Pic upload[Code Start]
            #region
            if (objEmployeeMasterMDL.EmpImage != null)
            {
                string ServerImagePath = Server.MapPath("/App_Images/" + "SBTMSEMPImages/EMPImage/");
                ImageError ErrorEmpImg = VTSFileHelper.CheckValidImage(objEmployeeMasterMDL.EmpImage);
                if (ImageError.None == ErrorEmpImg)
                {
                    objEmployeeMasterMDL.ImageName = VTSFileHelper.ResetFileNames(objEmployeeMasterMDL.EmpImage, objEmployeeMasterMDL.Employee_Code + "_EMPImage");

                    DeleteIfFileExists(ServerImagePath + objEmployeeMasterMDL.EmpImage);//DELETES IF FILE EXISTS BEFORE UPLOADING   
                    objEmployeeMasterMDL.EMPImageUrl = ServerImagePath + objEmployeeMasterMDL.ImageName;
                }

                bool ProfileImage = VTSFileHelper.Upload(objEmployeeMasterMDL.EmpImage, ServerImagePath, objEmployeeMasterMDL.ImageName);

                if (ProfileImage == false)
                {
                    objEmployeeMasterMDL.FK_CompanyId = SessionInfo.User.fk_companyid;

                    return View("AddEmployee", objEmployeeMasterMDL);
                }

            }
            if (objEmployeeMasterMDL.EmpProofImage != null)
            {
                string ServerImagePath = Server.MapPath("/App_Images/" + "SBTMSEMPPoofImages/EMPProofImage/");
                ImageError ErrorEmpProofImg = VTSFileHelper.CheckValidImage(objEmployeeMasterMDL.EmpProofImage);
                if (ImageError.None == ErrorEmpProofImg)
                {
                    objEmployeeMasterMDL.EMPImageName = VTSFileHelper.ResetFileNames(objEmployeeMasterMDL.EmpProofImage, objEmployeeMasterMDL.Employee_Code + "_EMPProofImage");

                    DeleteIfFileExists(ServerImagePath + objEmployeeMasterMDL.EmpProofImage);//DELETES IF FILE EXISTS BEFORE UPLOADING   
                    objEmployeeMasterMDL.EMPProofImageUrl = ServerImagePath + objEmployeeMasterMDL.EMPImageName;
                }

                bool EmpProfileImage = VTSFileHelper.Upload(objEmployeeMasterMDL.EmpProofImage, ServerImagePath, objEmployeeMasterMDL.EMPImageName);

                if (EmpProfileImage == false)
                {
                    objEmployeeMasterMDL.FK_CompanyId = SessionInfo.User.fk_companyid;

                    return View("AddEmployee", objEmployeeMasterMDL);
                }

            }
            #endregion
            // Profile Pic upload[Code End]


            if (ModelState.IsValid)
            {
                Messages msg = objEmployeeMasterBAL.AddEmployee(objEmployeeMasterMDL);
                msg.Message = "Employee " + msg.Message;
                TempData["Message"] = msg;
                return RedirectToAction("Index");
            }
            return View("AddEditEmployee");
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