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
    public class FeeHeadMasterDAL
    {
        DataFunctions objDataFunctions;
        string _commandText = string.Empty;
        DataSet objDataSet = null;
        //BasicPagingMDL objBasicPagingMDL;
        public FeeHeadMasterDAL()
        {
            objDataFunctions = new DataFunctions();
        }
        public bool GetAllFeeHead(out List<FeeHeadMasterMDL> _FeeHeadMasterList, out BasicPagingMDL objBasicPagingMDL, int Id, int Userid, int Fk_companyid, int RowPerpage, int CurrentPage, string SearchBy, string SearchValue)
        {
            bool result = false;
            objBasicPagingMDL = new BasicPagingMDL();
            _FeeHeadMasterList = new List<FeeHeadMasterMDL>();
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>()
                {
                    new SqlParameter("@iPK_FeeHeadid",Id),
                    new SqlParameter("@iCompanyId",Fk_companyid),
                    new SqlParameter("@iUser",Userid),
                    new SqlParameter("@iRowperPage",RowPerpage),
                    new SqlParameter("@iCurrentPage",CurrentPage),
                    new SqlParameter("@cSearchBy",SearchBy),
                    new SqlParameter("@cSearchValue",SearchValue)
                };
                _commandText = "[dbo].[usp_GetFeeHead]";

                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        _FeeHeadMasterList = objDataSet.Tables[1].AsEnumerable().Select(dr => new FeeHeadMasterMDL()
                        {
                            PK_FeeHeadId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_FeeHeadId")),
                            FeeHeadName = dr.Field<string>("FeeHeadName"),
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
        public Messages AddEditFeeHead(FeeHeadMasterMDL ObjFeeHeadMasterMDL)
        {
            Messages objMessages = new Messages();
            _commandText = "[dbo].[usp_AddEditFeeHead]";
            List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter("@iPK_FeeHeadId",SqlDbType.Int){Value = ObjFeeHeadMasterMDL.PK_FeeHeadId},
                    new SqlParameter("@cFeeHeadName", ObjFeeHeadMasterMDL.FeeHeadName.Trim()),
                    new SqlParameter("@iFK_CompanyId",SqlDbType.Int){Value = ObjFeeHeadMasterMDL.FK_CompanyId},
                    new SqlParameter("@bIsActive", ObjFeeHeadMasterMDL.IsActive),
                    new SqlParameter("@iCreatedBy",SqlDbType.Int){Value =ObjFeeHeadMasterMDL.CreatedBy}

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
