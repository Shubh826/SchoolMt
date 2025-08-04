using BAL;
using FRGMBSystem.Common;
using FRGMBSystem.Controllers;
using MDL;
using MDL.Common;
using SchoolMt.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SchoolMt.Controllers
{
    public class FormMasterController : BasicController
    {
        // GET: FormMST
        private List<FormMasterMDL> _FormMasterlist;
        FormMasterBAL objFormMasterBal = null;
        BasicPagingMDL objBasicPagingMDL = null;
        public FormMasterController()
        {
            objFormMasterBal = new FormMasterBAL();
        }
        // GET: FormMaster
        public ActionResult Index()
        {
            //ViewBag.CanAdd = UserDetailMDL.GetUserRoleAndRights.CanAdd;
            //ViewBag.CanEdit = UserDetailMDL.GetUserRoleAndRights.CanEdit;
            //ViewBag.CanView = UserDetailMDL.GetUserRoleAndRights.CanView;

            if (TempData["Message"] != null)
            {
                ViewBag.Msg = (Messages)TempData["Message"];
            }

            FormMasterMDL objFormMasterMDL = new FormMasterMDL();
            return View();

        }
        [HttpGet]
        public PartialViewResult getForms(int CurrentPage = 1, string SearchBy = "", string SearchValue = "")
        {
            //ViewBag.CanAdd = UserDetailMDL.GetUserRoleAndRights.CanAdd;
            //ViewBag.CanEdit = UserDetailMDL.GetUserRoleAndRights.CanEdit;
            //ViewBag.CanView = UserDetailMDL.GetUserRoleAndRights.CanView;

            FormMasterMDL objFormMasterMDL = new FormMasterMDL();

            objFormMasterBal.getFormsDetails(out _FormMasterlist, out objBasicPagingMDL, 0, Convert.ToInt32(20), CurrentPage, SearchBy, SearchValue);
            ViewBag.paging = objBasicPagingMDL;
            return PartialView("_getFormDetailGrid", _FormMasterlist);
        }
        [NonAction]
        private dynamic GetDropdownData()
        {

            SelectList formSelectList;

            string selectFormId = null;

            List<MDL.FormMasterMDL> formlist = objFormMasterBal.getMenu();
            if (formlist != null)
            {
                List<SelectListItem> items = formlist.Select(m => new SelectListItem()
                {
                    Text = m.FormName,
                    Value = m.Pk_FormId.ToString()
                }).ToList();
                items[0].Selected = true;
                selectFormId = items[0].Value;
                formSelectList = new SelectList(items, "Value", "Text");
            }

            else
            {
                List<SelectListItem> items = new List<SelectListItem>();
                items.Add(new SelectListItem { Text = "Menu Not Found", Value = "0" });
                formSelectList = new SelectList(items, "Value", "Text");
            }

            return new { FormList = formSelectList, SelectFormId = selectFormId };
        }
        [HttpGet]
        public ActionResult AddEditForm(int id = 0)
        {
            dynamic dropdownData = GetDropdownData();
            ViewData["formSelectList"] = dropdownData.FormList;

            if (id != 0)
            {
                objFormMasterBal.getFormsDetails(out _FormMasterlist, out objBasicPagingMDL, id, SessionInfo.User.fk_companyid);
                return View("AddEditForm", _FormMasterlist[0]);
            }
            else
            {
                FormMasterMDL obj = new FormMasterMDL();
                obj.IsActive = true;
                return View("AddEditForm", obj);
            }
        }

        [HttpPost]
        public ActionResult AddEditForm(FormMasterMDL FormMasterMDL)
        {
            FormMasterMDL.CreatedBy = SessionInfo.User.userid;
            if (FormMasterMDL.IsActive == false)
            {
                FormMasterMDL.IsDeleted = true;
            }
            else
            {
                FormMasterMDL.IsDeleted = false;
            }
            if (ModelState.IsValid)
            {
                Messages msg = objFormMasterBal.AddEditForm(FormMasterMDL);

                TempData["Message"] = msg;
                return RedirectToAction("Index");
            }
            else
            {
                FormMasterMDL objFormMasterMDL = new FormMasterMDL();
                objFormMasterMDL.IsActive = true;
                return View("AddEditForm", objFormMasterMDL);
            }
        }

        public JsonResult GetDataForExport(int CurrentPage=1,int rowperPage =10, string SearchBy = "", string SearchValue = "")
        {
            try
            {
                objFormMasterBal.getFormsDetails(out _FormMasterlist, out objBasicPagingMDL, 0, Convert.ToInt32(rowperPage), CurrentPage, SearchBy, SearchValue);
                TempData["FormMasterlist"] = _FormMasterlist;
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
            List<FormMasterMDL> _listForExcel = (List<FormMasterMDL>)TempData["FormMasterlist"];
            string[] columns = { "Form Name", "Controller Name", "Action Name", "Parent Form", "Class Name", "Area Name", "Web/Mobile"};
            string MDLAttr = "FormName,ControllerName,ActionName,ParentForm,ClassName,AreaName,FormFor";
            ExcelExportHelper objExcelExportHelper = new ExcelExportHelper();
            return objExcelExportHelper.ExportExcel(_listForExcel, "Form Master", ".xls", MDLAttr, columns);
        }

    }
}