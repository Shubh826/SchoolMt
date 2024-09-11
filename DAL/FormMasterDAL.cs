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
    public class FormMasterDAL
    {
        #region
        static DataFunctions objDataFunctions = null;
        DataSet objDataSet = null;
        static string _commandText = string.Empty;
        #endregion
        public FormMasterDAL()
        {
            objDataFunctions = new DataFunctions();
        }
        public bool getFormsDetails(out List<FormMasterMDL> _FormMasterlist, out BasicPagingMDL objBasicPagingMDL, int id, int RowPerpage, int CurrentPage, string SearchBy, string SearchValue)
        {
            bool result = false;
            objBasicPagingMDL = new BasicPagingMDL();
            _FormMasterlist = new List<FormMasterMDL>();
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>()
                {
                    new SqlParameter("@iFormId",id),
                      new SqlParameter("@iRowperPage",RowPerpage),
                        new SqlParameter("@iCurrentPage",CurrentPage),
                         new SqlParameter("@cSearchBy",SearchBy),
                          new SqlParameter("@cSearchValue",SearchValue)
                };

                _commandText = "[dbo].[usp_GetFormdetails]";

                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        _FormMasterlist = objDataSet.Tables[1].AsEnumerable().Select(dr => new FormMasterMDL()
                        {
                            Pk_FormId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("Pk_FormId")),
                            FormName = dr.Field<string>("FormName"),
                            ControllerName = dr.Field<string>("ControllerName"),
                            ActionName = dr.Field<string>("ActionName"),
                            Fk_ParentId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("Fk_ParentId")),
                            //sortId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("sortId")),
                            ClassName = dr.Field<string>("ClassName"),
                            AreaName = dr.Field<string>("Area"),
                            ParentForm = dr.Field<string>("parentform"),
                            IsActive = dr.Field<bool>("IsActive"),
                            FormFor = dr.Field<string>("FormFor")
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
        public Messages AddEditForm(FormMasterMDL FormMasterMDL)
        {
            if (FormMasterMDL.Fk_ParentId == null)
            {
                FormMasterMDL.Fk_ParentId = 0;
            }


            Messages objMessages = new Messages();
            _commandText = "[dbo].[usp_AddEditForm]";
            List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter("@iPk_FormId",SqlDbType.Int){Value = FormMasterMDL.Pk_FormId},
                    new SqlParameter("@cFormName", FormMasterMDL.FormName),
                    new SqlParameter("@cControllerName", FormMasterMDL.ControllerName),
                    new SqlParameter("@cActionName",FormMasterMDL.ActionName),
                    new SqlParameter("@iFk_ParentId", FormMasterMDL.Fk_ParentId),
                   /// new SqlParameter("@iSortId", FormMasterMDL.sortId),
                    new SqlParameter("@cClassName", FormMasterMDL.ClassName),
                    new SqlParameter("@cAreaName", FormMasterMDL.AreaName),
                   // new SqlParameter("@iLevelId",FormMasterMDL.LevelId),
                    new SqlParameter("@bIsActive", FormMasterMDL.IsActive),
                    new SqlParameter("@bIsDeleted", FormMasterMDL.IsDeleted),
                    new SqlParameter("@iCreatedBy",SqlDbType.Int){Value =FormMasterMDL.CreatedBy},
                    new SqlParameter("@cFormFor",FormMasterMDL.FormFor)
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
        public DataSet getMenu()
        {
            _commandText = "[dbo].[usp_getMenu]";
            try
            {
                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet);
            }
            catch (Exception)
            {
            }
            return objDataSet;
        }



    }
}
