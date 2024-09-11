using DAL;
using MDL;
using MDL.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class RoleMstBAL
    {
        private RoleMstDAL objRoleDal;
        private DataSet _dataSet;
        public RoleMstBAL()
        {
            objRoleDal = new RoleMstDAL();
        }
        public bool GetAllRoles(out List<RoleMstMDL> _Rolelist, out BasicPagingMDL objBasicPagingMDL, int id, int user, int cmpId, int RowPerpage = 20, int CurrentPage = 1, string SearchBy = "", string SearchValue = "")
        {
            _Rolelist = new List<RoleMstMDL>();
            objBasicPagingMDL = new BasicPagingMDL();
            return objRoleDal.GetAllRoles(out _Rolelist, out objBasicPagingMDL, id, user, cmpId, RowPerpage, CurrentPage, SearchBy, SearchValue);
        }
        public Messages AddEditRole(RoleMstMDL ObjRoleMstDAL)
        {
            return objRoleDal.AddEditRole(ObjRoleMstDAL);
        }
        public Messages SaveRoleMapping(List<RoleMapping> roleMappings, int currentUser, string MappingFor)
        {
            _dataSet = objRoleDal.SaveRoleMapping(roleMappings, currentUser, MappingFor);
            Messages msg = null;
            if (_dataSet != null)
            {
                if (_dataSet.Tables[0] == null)
                {
                    msg = new Messages();
                }
                else
                {
                    DataRow dr = _dataSet.Tables[0].Rows[0];
                    if (dr != null)
                    {
                        msg = new Messages()
                        {
                            Message = dr.Field<string>("Message"),
                            Message_Id = dr.Field<int>("MessageId")
                        };
                    }
                }
                _dataSet.Dispose();
            }
            return msg;
        }
        public List<RoleMstMDL> getAllRoles()
        {
            _dataSet = objRoleDal.getAllRoles();
            List<RoleMstMDL> listRoles;
            if (_dataSet != null)
            {
                listRoles = _dataSet.Tables[0].AsEnumerable().Select(r => new RoleMstMDL()
                {
                    PK_RoleId = r.Field<int>("PK_RoleId"),
                    RoleName = r.Field<string>("RoleName"),
                    IsActive = r.Field<bool>("IsActive")
                }).ToList();

                _dataSet.Dispose();
            }
            else
            {
                listRoles = new List<RoleMstMDL>();
            }
            return listRoles;

        }
        public List<FormsMDL> getMenu()
        {
            _dataSet = objRoleDal.getMenu();
            List<FormsMDL> forms = null;
            if (_dataSet != null)
            {
                forms = _dataSet.Tables[0].AsEnumerable().Select(r => new FormsMDL()
                {
                    PK_FormId = r.Field<int>("PK_FormId"),
                    FormName = r.Field<string>("FormName")
                }).ToList();
                _dataSet.Dispose();
            }
            else
            {
                forms = new List<FormsMDL>();
            }
            return forms;
        }
        public List<FormsMDL> getMenuForMapping(int CompanyId)
        {
            _dataSet = objRoleDal.getMenuForMapping(CompanyId);
            List<FormsMDL> forms = null;
            if (_dataSet != null)
            {
                forms = _dataSet.Tables[0].AsEnumerable().Select(r => new FormsMDL()
                {
                    PK_FormId = r.Field<int>("PK_FormId"),
                    FormName = r.Field<string>("FormName")
                }).ToList();
                _dataSet.Dispose();
            }
            else
            {
                forms = new List<FormsMDL>();
            }
            return forms;
        }
        public List<RoleMapping> GetSubMenuForMapping(string MappingFor, int roleId, int formId, int CompId)
        {
            _dataSet = objRoleDal.GetSubMenuForMapping(MappingFor, roleId, formId, CompId);
            List<RoleMapping> forms = null;
            if (_dataSet != null)
            {
                forms = _dataSet.Tables[0].AsEnumerable().Select(r => new RoleMapping()
                {
                    PK_FormRoleId = WrapDbNullValue<int>(r.Field<int?>("PK_FormRoleId")),
                    FK_FormId = WrapDbNullValue<int>(r.Field<int?>("FK_FormId")),
                    FormName = WrapDbNullValue<string>(r.Field<string>("FormName")),
                    CanView = WrapDbNullValue<bool>(r.Field<bool?>("CanView")),
                    CanAdd = WrapDbNullValue<bool>(r.Field<bool?>("CanAdd")),
                    CanEdit = WrapDbNullValue<bool>(r.Field<bool?>("CanEdit")),
                    CanDelete = WrapDbNullValue<bool>(r.Field<bool?>("CanDelete")),
                    FK_RoleId = roleId
                }).ToList();
                forms.ForEach(m =>
                {
                    m.All = (m.CanAdd == true && m.CanEdit == true && m.CanDelete == true && m.CanView == true) ? true : false;
                });
                _dataSet.Dispose();
            }
            else
            {
                forms = new List<RoleMapping>();
            }
            return forms;
        }
        private static T WrapDbNullValue<T>(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return default(T);
            }
            return (T)Convert.ChangeType(value, typeof(T));
        }
        public List<RoleMapping> getSubMenu(string MappingFor, int roleId, int formId)
        {
            _dataSet = objRoleDal.getSubMenu(MappingFor, roleId, formId);
            List<RoleMapping> forms = null;
            if (_dataSet != null)
            {
                forms = _dataSet.Tables[0].AsEnumerable().Select(r => new RoleMapping()
                {
                    PK_FormRoleId = WrapDbNullValue<int>(r.Field<int?>("PK_FormRoleId")),
                    FK_FormId = WrapDbNullValue<int>(r.Field<int?>("FK_FormId")),
                    FormName = WrapDbNullValue<string>(r.Field<string>("FormName")),
                    CanView = WrapDbNullValue<bool>(r.Field<bool?>("CanView")),
                    CanAdd = WrapDbNullValue<bool>(r.Field<bool?>("CanAdd")),
                    CanEdit = WrapDbNullValue<bool>(r.Field<bool?>("CanEdit")),
                    CanDelete = WrapDbNullValue<bool>(r.Field<bool?>("CanDelete")),
                    FK_RoleId = roleId
                }).ToList();
                forms.ForEach(m =>
                {
                    m.All = (m.CanAdd == true && m.CanEdit == true && m.CanDelete == true && m.CanView == true) ? true : false;
                });
                _dataSet.Dispose();
            }
            else
            {
                forms = new List<RoleMapping>();
            }
            return forms;
        }

    }
}
