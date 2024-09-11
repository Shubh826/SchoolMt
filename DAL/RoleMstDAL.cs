using DAL.DataUtility;
using MDL;
using MDL.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class RoleMstDAL
    {
        DataFunctions objDataFunctions;
        string _commandText = string.Empty;
        DataSet objDataSet = null;
        //BasicPagingMDL objBasicPagingMDL;
        public RoleMstDAL()
        {
            objDataFunctions = new DataFunctions();
        }
        public bool GetAllRoles(out List<RoleMstMDL> _Rolelist, out BasicPagingMDL objBasicPagingMDL, int id, int user, int cmpId, int RowPerpage, int CurrentPage, string SearchBy, string SearchValue)
        {
            bool result = false;
            objBasicPagingMDL = new BasicPagingMDL();
            _Rolelist = new List<RoleMstMDL>();
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>()
                {
                    new SqlParameter("@iRoleid",id),
                    new SqlParameter("@iCompanyId",cmpId),
                    new SqlParameter("@iUser",user),
                    new SqlParameter("@iRowperPage",RowPerpage),
                    new SqlParameter("@iCurrentPage",CurrentPage),
                    new SqlParameter("@cSearchBy",SearchBy),
                    new SqlParameter("@cSearchValue",SearchValue)
                };
                _commandText = "[dbo].[usp_GetRoles]";

                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        _Rolelist = objDataSet.Tables[1].AsEnumerable().Select(dr => new RoleMstMDL()
                        {
                            PK_RoleId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_RoleId")),
                            RoleName = dr.Field<string>("RoleName"),
                            FK_CompanyId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_CompanyId")),
                            CompanyName = dr.Field<string>("CompanyName"),
                            FK_FormId = WrapDbNull.WrapDbNullValue<int>(dr.Field<string>("HomePage")),
                            FormName = dr.Field<string>("FormName"),
                            IsActive = dr.Field<bool>("IsActive"),
                            FK_ClientId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_ClientId")),
                            IsClient = dr.Field<bool>("IsClient"),
                            IsCompany = dr.Field<bool>("IsCompany")
                        }).ToList();
                        objBasicPagingMDL = new BasicPagingMDL()
                        {
                            TotalItem = WrapDbNull.WrapDbNullValue<int>(objDataSet.Tables[2].Rows[0].Field<int?>("TotalItem")),
                            RowParPage = RowPerpage,
                            CurrentPage = CurrentPage
                        };
                        if (objBasicPagingMDL.TotalItem % objBasicPagingMDL.RowParPage == 0)
                        {
                            objBasicPagingMDL.TotalPage = objBasicPagingMDL.TotalItem / objBasicPagingMDL.RowParPage;
                        }
                        else
                            objBasicPagingMDL.TotalPage = objBasicPagingMDL.TotalItem / objBasicPagingMDL.RowParPage + 1;


                        objDataSet.Dispose();
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
        public DataSet getMenu()
        {
            _commandText = "usp_getMenu";
            try
            {
                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet);
            }
            catch (Exception)
            {
            }
            return objDataSet;
        }
        public Messages AddEditRole(RoleMstMDL ObjRoleMstMDL)
        {
            Messages objMessages = new Messages();
            _commandText = "[dbo].[usp_AddEditRole]";
            List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter("iPK_RoleId",SqlDbType.Int){Value = ObjRoleMstMDL.PK_RoleId},
                    new SqlParameter("@cRoleName", ObjRoleMstMDL.RoleName.Trim()),
                    new SqlParameter("@cHomePage", ObjRoleMstMDL.FK_FormId),//FK_FormId
                    new SqlParameter("@iFK_CompanyId",SqlDbType.Int){Value = ObjRoleMstMDL.FK_CompanyId},
                    new SqlParameter("@iFK_ClientId",SqlDbType.Int){Value = ObjRoleMstMDL.FK_ClientId},
                    new SqlParameter("@bIsActive", ObjRoleMstMDL.IsActive),
                    new SqlParameter("@bIsClient", ObjRoleMstMDL.IsClient),
                    new SqlParameter("@bIsCompany", ObjRoleMstMDL.IsCompany),
                    new SqlParameter("@iCreatedBy",SqlDbType.Int){Value =ObjRoleMstMDL.CreatedBy}

                };
            try
            {
                CheckParameters.ConvertNullToDBNull(parms);
                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    objMessages.Message_Id = objDataSet.Tables[0].Rows[0].Field<int>("Message_Id");
                    objMessages.Message = objDataSet.Tables[0].Rows[0].Field<string>("Message");
                }
                else
                {
                    objMessages.Message_Id = 0;
                    objMessages.Message = "Failed";
                }
            }
            catch (Exception ex)
            {
                objMessages.Message_Id = 0;
                objMessages.Message = "Failed";
            }
            return objMessages;
        }

        public DataSet getMenuForMapping(int CompanyId)
        {
            _commandText = "[dbo].[usp_getFormsByCompany]";
            List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter("@iCompanyId",CompanyId),
            };
            try
            {
                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
            }
            catch (Exception ex)
            {
            }
            return objDataSet;
        }

        public DataSet SaveRoleMapping(List<RoleMapping> roleMappings, int currentUser, string MappingFor)
        {
            _commandText = "[dbo].[usp_FormRoleAddEdit]";
            DataTable Dt = DataTransformer.ConvertTo(roleMappings);

            List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@iCreatedBy",currentUser),
                        new SqlParameter("@FormRole", Dt ),
                        new SqlParameter("@cMappingFor",MappingFor)
                    };

            try
            {
                CheckParameters.ConvertNullToDBNull(parms);
                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);

            }
            catch (Exception ex)
            {

            }

            return objDataSet;
        }

        public DataSet getSubMenu(string MappingFor, int roleId, int formId)
        {
            _commandText = "[dbo].[usp_GetSubMenu]";
            List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter("@cMappingFor", SqlDbType.VarChar){Value = MappingFor},
                    new SqlParameter("@iRoleId", SqlDbType.Int,roleId){Value = roleId},
                    new SqlParameter("@iFormId", SqlDbType.Int,formId){Value = formId}
                };

            try
            {
                CheckParameters.ConvertNullToDBNull(parms);
                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);

            }
            catch (Exception ex)
            {

            }
            return objDataSet;
        }
        public DataSet getAllRoles()
        {
            _commandText = "usp_GetRole";
            try
            {
                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet);
            }
            catch (Exception)
            {
            }
            return objDataSet;
        }
        public DataSet GetSubMenuForMapping(string MappingFor, int roleId, int formId, int CompId)
        {
            _commandText = "[dbo].[usp_GetSubMenuForMapping]";

            List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter("@cMappingFor", SqlDbType.VarChar){Value = MappingFor},
                    new SqlParameter("@iRoleId", SqlDbType.Int,roleId){Value = roleId},
                    new SqlParameter("@iFormId", SqlDbType.Int,formId){Value = formId},
                    new SqlParameter("@iCompanyId", SqlDbType.Int,CompId){Value = CompId},
                };

            try
            {
                CheckParameters.ConvertNullToDBNull(parms);
                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);

            }
            catch (Exception ex)
            {

            }
            return objDataSet;
        }

    }
}
