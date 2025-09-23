using DAL.DataUtility;
using MDL.Common;
using MDL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class SubjectMarkMappingDAL
    {
        static DataFunctions objDataFunctions = null;
        System.Data.DataSet objDataSet = null;
        static string _commandText = string.Empty;

        public SubjectMarkMappingDAL()
        {
            objDataFunctions = new DataFunctions();
        }
        public bool GetSubjectMarkMappingData(out List<SubjectMarkMappingMDL> List, out BasicPagingMDL objBasicPagingMDL, int id, int rowPerpage, int currentPage, int FK_CompanyId, string SearchBy, string SearchValue)
        {
            List = new List<SubjectMarkMappingMDL>();
            objBasicPagingMDL = new BasicPagingMDL();
            bool result = false;
            Messages objMessages = new Messages();
            _commandText = "[dbo].[usp_GetSchoolConfigurationData]";
            List<SqlParameter> parms = new List<SqlParameter>
               {
                    new SqlParameter("@iRowperPage",rowPerpage),
                    new SqlParameter("@iCurrentPage",currentPage),
                    new SqlParameter("@FK_CompanyId",FK_CompanyId),
                    new SqlParameter("@SearchBy",SearchBy),
                    new SqlParameter("@SearchValue",SearchValue),
                    new SqlParameter("@icompId",id)
              };
            try
            {
                CheckParameters.ConvertNullToDBNull(parms);
                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        List = objDataSet.Tables[1].AsEnumerable().Select(dr => new SubjectMarkMappingMDL()
                        {
                            PK_SchoolConfigurationId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_SchoolConfigurationId")),
                            FK_CompanyId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_CompanyId")),
                            FK_ClassId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_ClassId")),
                            LookUpDetailId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_LookUpDetailId")),
                            TotalMark = dr.Field<string>("TotalMark"),

                            ClassName = dr.Field<string>("ClassName"),
                            CompanyName = dr.Field<string>("CompanyName"),
                            LookUpDetailName = dr.Field<string>("LookUpDetailName"),
                            LookUpName = dr.Field<string>("LookUpName"),

                            // Since your query is returning '' (empty string), these are safe as string
                            CreatedBy = dr.Field<string>("CreatedBy"),
                            CreatedDate = dr.Field<string>("CreatedDate")

                        }).ToList();


                        objBasicPagingMDL = new BasicPagingMDL()
                        {
                            TotalItem = WrapDbNull.WrapDbNullValue<int>(objDataSet.Tables[2].Rows[0].Field<int?>("TotalItem")),
                            RowParPage = rowPerpage,
                            CurrentPage = currentPage
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
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public Messages InsertSubjectMarkMappingData(SubjectMarkMappingMDL obj)
        {
            Messages objMessages = new Messages();
            _commandText = "[dbo].[USP_InsertOrUpdateSchoolConfiguration]";
            List<SqlParameter> parms = new List<SqlParameter>
               {
                    new SqlParameter("@CompanyId",obj.FK_CompanyId),
                    new SqlParameter("@ClassId" ,obj.FK_ClassId),
                    new SqlParameter("@JsonData", obj.JsonData),
                    new SqlParameter("@CreatedBy" ,obj.userId)
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

        public Messages DeleteSchoolConfigurationData(string pkIds)
        {
            Messages objMessages = new Messages();
            _commandText = "[dbo].[usp_DeleteSchoolConfigurationData]";
            List<SqlParameter> parms = new List<SqlParameter>
               {
                    new SqlParameter("@cPkIds",pkIds)
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
