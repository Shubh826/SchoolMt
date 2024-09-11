using BAL;
using BAL.Common;
using SchoolMt.Common;
using MDL;
using MDL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolMt.Controllers
{
    public class FormRoleMappingController : Controller
    {
        // GET: FormRoleMapping
        public BasicPagingMDL paging = new BasicPagingMDL();
        private readonly RoleMstBAL objRoleBAL;
        public int CurrentUser = 1;
        public FormRoleMappingController()
        {
            objRoleBAL = new RoleMstBAL();
        }
        public ActionResult Index()
        {
            
            if (TempData["Message"] != null)
            {
                ViewBag.Msg = (Messages)TempData["Message"]; 
                TempData["Message"] = null;
            }
            dynamic dropdownData = GetDataForDropdown();
            ViewData["companylist"] = CommonBAL.FillCompany(SessionInfo.User.fk_companyid);
            ViewData["Mappinglist"] = CommonBAL.GetMappingCompanyWise(SessionInfo.User.fk_companyid);
            ViewData["roleSelectlist"] = dropdownData.rolelist;
            ViewData["Rolelist"] = CommonBAL.FillRole(SessionInfo.User.fk_companyid);

            ViewData["formSelectList"] = dropdownData.FormList;
            FormMappingViewModel viewModel = new FormMappingViewModel();
            if (dropdownData.SelectRoleID != null && dropdownData.SelectFormId != null)
            {
                int selectRoleId = Convert.ToInt32(dropdownData.SelectRoleID);
                int selectFormId = Convert.ToInt32(dropdownData.SelectFormId);
                viewModel.FK_RoleId = selectRoleId;
                if (TempData["roleId"] != null)
                {
                    viewModel.FK_RoleId = (int)TempData["roleId"];
                    selectRoleId = (int)TempData["roleId"];
                }
                string MappingFor = "WebApp";
                viewModel.Forms = GetSubMenu(MappingFor, selectRoleId, selectFormId);
                viewModel.FK_FormId = selectFormId;


            }
            if (TempData["Message"] != null)
            {
                ViewBag.Msg = (Messages)TempData["Message"];
            }
            viewModel.FK_CompanyID = SessionInfo.User.fk_companyid;
            return View(viewModel);

        }
        [HttpPost]
        public ActionResult Index(FormMappingViewModel obj)
        {
            List<RoleMapping> roleMappings = obj.Forms;
            string MappingFor = obj.Mapping;
            if (roleMappings.Count > 1 && roleMappings.Any(x => x.CanView == true))
            {
                roleMappings[0].CanAdd = true;
                roleMappings[0].CanEdit = true;
                roleMappings[0].CanDelete = true;
                roleMappings[0].CanView = true;
            }
            if (roleMappings != null)
            {
                roleMappings.ForEach(r =>
                {
                    r.FK_RoleId = obj.FK_RoleId;
                    r.CreatedBy = CurrentUser;
                });
                Messages msg = objRoleBAL.SaveRoleMapping(roleMappings, CurrentUser, MappingFor);
                if (msg != null && msg.Message_Id == 1)
                {
                    msg.Message = "Rights Updated Successfully ";
                    TempData["roleId"] = obj.FK_RoleId;
                }
                else
                    msg.Message = "Sorry ,Rights Not Update Successfully ";
                TempData["Message"] = msg;
                return RedirectToAction("Index", "FormRoleMapping");
            }
            else
            {
                ModelState.AddModelError("", "Sorry Not Updated");
                obj.FK_CompanyID = SessionInfo.User.fk_companyid;
                return PartialView("_FormMapping", obj);
            }
            return View();

        }
        [NonAction]
        private dynamic GetDropdownData()
        {
            SelectList roleSelectList;
            SelectList formSelectList;
            string selectRoleId = null;
            string selectFormId = null;
            List<RoleMstMDL> roleList = objRoleBAL.getAllRoles();
            if (roleList != null)
            {
                List<SelectListItem> list = roleList.Select(g => new SelectListItem()
                {
                    Text = g.RoleName,
                    Value = g.PK_RoleId.ToString(),
                }).ToList();
                selectRoleId = Convert.ToString(SessionInfo.User.roleid);
                roleSelectList = new SelectList(list, "Value", "Text");
            }
            else
            {
                List<SelectListItem> items = new List<SelectListItem>();
                items.Add(new SelectListItem { Text = "Role Not Found", Value = "0" });
                roleSelectList = new SelectList(items, "Value", "Text");
            }

            List<MDL.FormsMDL> formlist = objRoleBAL.getMenu();
            if (formlist != null)
            {
                List<SelectListItem> items = formlist.Select(m => new SelectListItem()
                {
                    Text = m.FormName,
                    Value = m.PK_FormId.ToString()
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

            return new { rolelist = roleSelectList, FormList = formSelectList, SelectRoleID = selectRoleId, SelectFormId = selectFormId };
        }
        private dynamic GetDataForDropdown()
        {
            SelectList roleSelectList;
            SelectList formSelectList;
            string selectRoleId = null;
            string selectFormId = null;
            List<RoleMstMDL> roleList = objRoleBAL.getAllRoles();
            if (roleList != null)
            {
                List<SelectListItem> list = roleList.Select(g => new SelectListItem()
                {
                    Text = g.RoleName,
                    Value = g.PK_RoleId.ToString(),
                }).ToList();
                selectRoleId = Convert.ToString(SessionInfo.User.roleid);
                roleSelectList = new SelectList(list, "Value", "Text");
            }
            else
            {
                List<SelectListItem> items = new List<SelectListItem>();
                items.Add(new SelectListItem { Text = "Role Not Found", Value = "0" });
                roleSelectList = new SelectList(items, "Value", "Text");
            }

            List<MDL.FormsMDL> formlist = objRoleBAL.getMenuForMapping(SessionInfo.User.fk_companyid);
            if (formlist != null)
            {
                List<SelectListItem> items = formlist.Select(m => new SelectListItem()
                {
                    Text = m.FormName,
                    Value = m.PK_FormId.ToString()
                }).ToList();
                if (items.Count > 0)
                {
                    items[0].Selected = true;
                    selectFormId = items[0].Value;
                }
                formSelectList = new SelectList(items, "Value", "Text");
            }

            else
            {
                List<SelectListItem> items = new List<SelectListItem>();
                items.Add(new SelectListItem { Text = "Menu Not Found", Value = "0" });
                formSelectList = new SelectList(items, "Value", "Text");
            }

            return new { rolelist = roleSelectList, FormList = formSelectList, SelectRoleID = selectRoleId, SelectFormId = selectFormId };
        }
        public PartialViewResult FormRole(int roleId = 0, int formId = 0, string MappingFor = "", int companyid = 0)
        {
            FormMappingViewModel viewModel = new FormMappingViewModel();
            viewModel.FK_RoleId = roleId;
            viewModel.FK_FormId = formId;
            viewModel.Forms = GetSubMenuForMapping(MappingFor, roleId, formId, companyid);
            ModelState.Clear();
            return PartialView("_FormMapping", viewModel);
        }
        [NonAction]
        private List<RoleMapping> GetSubMenuForMapping(string MappingFor, int roleId = 0, int formId = 0, int CompId = 0)
        {
            List<RoleMapping> roleMappings = objRoleBAL.GetSubMenuForMapping(MappingFor, roleId, formId, CompId);
            return roleMappings;
        }

        [NonAction]
        private List<RoleMapping> GetSubMenu(string MappingFor, int roleId = 0, int formId = 0)
        {
            List<RoleMapping> roleMappings = objRoleBAL.getSubMenu(MappingFor, roleId, formId);
            return roleMappings;
        }
        public JsonResult getRole(int companyId)
        {
            return Json(CommonBAL.FillRole(companyId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult getForms(int companyId)
        {
            return Json(objRoleBAL.getMenuForMapping(companyId), JsonRequestBehavior.AllowGet);
        }
    }
}