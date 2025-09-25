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

    public class LookUpMasterDAL
    {
        #region
        DataSet objDataSet = null;
        static string _commandText = string.Empty;
        private DataFunctions objDataFunctions;
        Messages objMessages = null;
        #endregion
        public LookUpMasterDAL()
        {
            objDataFunctions = new DataFunctions();
            objMessages = new Messages();
        }
        public Messages AddEditLookUp(LookUpMDL Obj)
        {
            Messages objMessages = new Messages();
            _commandText = "[SMS].[usp_AddEditLookUp]";
            List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter("@iPkId",Obj.PkId),
                    new SqlParameter("@cLookUpName", Obj.LookupName),
                    new SqlParameter("@bIsActive", Obj.IsActive),
                    new SqlParameter("@iUserId", Obj.UserId),


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

        public bool GetLookUpData(out List<LookUpMDL> _List, int Id, string SearchBy, string SearchValue)
        {
            bool result = false;
            _List = new List<LookUpMDL>();
            List<SqlParameter> parms = new List<SqlParameter>()
                {
                     new SqlParameter("@iPkId",Id),
                     new SqlParameter("@cSearchBy",SearchBy),
                     new SqlParameter("@cSearchValue",SearchValue)

                };

            try
            {

                _commandText = "[SMS].[usp_GetLookUp]";

                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        _List = objDataSet.Tables[1].AsEnumerable().Select(dr => new LookUpMDL()
                        {
                            PkId = dr.Field<int>("PK_LookUpId"),

                            LookupName = dr.Field<string>("LookUpName"),
                            IsActive = dr.Field<bool>("IsActive"),
                            CreatedBy = dr.Field<string>("CreatedBy"),
                            CreatedDate = dr.Field<string>("CreatedDate")
                        }).ToList();

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

        public Messages AddEditLookUpDetail(LookUpDetailMDL Obj)
        {
            Messages objMessages = new Messages();
            _commandText = "[SMS].[usp_AddEditLookUpDetail]";
            List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter("@iPkId",Obj.PkId),
                    new SqlParameter("@iFk_CompanyId",Obj.Fk_CompanyId),
                     new SqlParameter("@iFk_LookUpId",Obj.Fk_LookUpId),
                    new SqlParameter("@cLookUpDetailName", Obj.LookupDetailName),
                    new SqlParameter("@bIsActive", Obj.IsActive),
                    new SqlParameter("@iUserId", Obj.UserId),
                     new SqlParameter("@iMinValue", Obj.MinValue),
                      new SqlParameter("@iMaxValue", Obj.MaxValue),
                      new SqlParameter("@iSortid", Obj.SortId),
                      new SqlParameter("@cAbbr", Obj.AbbrValue),


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



        public bool GetLookUpDetailData(out List<LookUpDetailMDL> _List, out BasicPagingMDL objBasicPagingMDL, int Id, int CurrentPage, int RowPerpage, string SearchBy, string SearchValue)
        {
            bool result = false;
            _List = new List<LookUpDetailMDL>();
            objBasicPagingMDL = new BasicPagingMDL();
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>()
                {
                     new SqlParameter("@iPkId",Id),
                     new SqlParameter("@iCurrentPage",CurrentPage),
                     new SqlParameter("@iRowPerPage",RowPerpage),
                     new SqlParameter("@cSearchBy",SearchBy),
                     new SqlParameter("@cSearchValue",SearchValue)
                };
               
                 _commandText = "[SMS].[usp_GetLookUpDetail]";

                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        _List = objDataSet.Tables[1].AsEnumerable().Select(dr => new LookUpDetailMDL()
                        {
                            PkId = dr.Field<int>("PK_LookUpDetailId"),
                            Fk_LookUpId = dr.Field<int>("FK_LookUpId"),
                            Fk_CompanyId= dr.Field<int>("Fk_CompanyId"),
                            LookupDetailName = dr.Field<string>("LookUpDetailName"),
                            LookupName = dr.Field<string>("LookUpName"),
                            IsActive = dr.Field<bool>("IsActive"),
                            CreatedBy = dr.Field<string>("CreatedBy"),
                            CreatedDate = dr.Field<string>("CreatedDate"),
                            Status= dr.Field<string>("Status"),
                            CompanyName= dr.Field<string>("CompanyName"),
                            AbbrValue = dr.Field<string>("AbbrValue"),
                            SortId = dr.Field<int>("SortId"),
                            MinValue = dr.Field<int?>("MinValue"),
                            MaxValue= dr.Field<int?>("MaxValue")
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


    }
}
