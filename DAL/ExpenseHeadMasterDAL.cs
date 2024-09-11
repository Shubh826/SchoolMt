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
    public class ExpenseHeadMasterDAL
    {
        DataFunctions objDataFunctions;
        string _commandText = string.Empty;
        DataSet objDataSet = null;
        //BasicPagingMDL objBasicPagingMDL;
        public ExpenseHeadMasterDAL()
        {
            objDataFunctions = new DataFunctions();
        }
        public bool GetExpenseHead(out List<ExpenseHeadMasterMDL> _ExpenseHeadMasterList, out BasicPagingMDL objBasicPagingMDL, int Id, int Userid, int Fk_companyid, int RowPerpage, int CurrentPage, string SearchBy, string SearchValue)
        {
            bool result = false;
            objBasicPagingMDL = new BasicPagingMDL();
            _ExpenseHeadMasterList = new List<ExpenseHeadMasterMDL>();
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>()
                {
                    new SqlParameter("@iPK_ExpenseHeadid",Id),
                    new SqlParameter("@iCompanyId",Fk_companyid),
                    new SqlParameter("@iUser",Userid),
                    new SqlParameter("@iRowperPage",RowPerpage),
                    new SqlParameter("@iCurrentPage",CurrentPage),
                    new SqlParameter("@cSearchBy",SearchBy),
                    new SqlParameter("@cSearchValue",SearchValue)
                };
                _commandText = "[dbo].[usp_GetExpenseHead]";

                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        _ExpenseHeadMasterList = objDataSet.Tables[1].AsEnumerable().Select(dr => new ExpenseHeadMasterMDL()
                        {
                            PK_ExpenseHeadId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_ExpenseHeadId")),
                            ExpenseHeadName = dr.Field<string>("ExpenseHeadName"),
                            FK_CompanyId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_CompanyId")),
                            CompanyName = dr.Field<string>("CompanyName"),
                            IsActive = dr.Field<bool>("IsActive")
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
        public Messages AddEditExpenseHead(ExpenseHeadMasterMDL ObjExpenseHeadMasterMDL)
        {
            Messages objMessages = new Messages();
            _commandText = "[dbo].[usp_AddEditExpenseHead]";
            List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter("@iPK_ExpenseHeadId",SqlDbType.Int){Value = ObjExpenseHeadMasterMDL.PK_ExpenseHeadId},
                    new SqlParameter("@cExpenseHeadName", ObjExpenseHeadMasterMDL.ExpenseHeadName.Trim()),
                    new SqlParameter("@iFK_CompanyId",SqlDbType.Int){Value = ObjExpenseHeadMasterMDL.FK_CompanyId},
                    new SqlParameter("@bIsActive", ObjExpenseHeadMasterMDL.IsActive),
                    new SqlParameter("@iCreatedBy",SqlDbType.Int){Value =ObjExpenseHeadMasterMDL.CreatedBy}

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
    }
}
