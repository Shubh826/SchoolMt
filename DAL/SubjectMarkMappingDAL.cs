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
            _commandText = "[dbo].[usp_GetSubjectMarkMappingData]";
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
                            PKId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_ExamMarksMasterId")),
                            FK_CompanyId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_SchoolId")),
                            FK_ClassId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_ClassId")),
                            FK_StudentId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_StudentId")),
                            FK_LookUpId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_ExamTypeId")),
                            LookUpDetailId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_ExamTypeId")),
                            ClassName = dr.Field<string>("ClassName"),
                            CompanyName = dr.Field<string>("CompanyName"),
                            StudentName = dr.Field<string>("StudentName"),
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
            _commandText = "[dbo].[USP_InsertOrUpdateSubjectmark]";
            List<SqlParameter> parms = new List<SqlParameter>
               {

                    new SqlParameter("@PKId", obj.PKId == null? 0:obj.PKId),
                    new SqlParameter("@CompanyId",obj.FK_CompanyId),
                    new SqlParameter("@ClassId" ,obj.FK_ClassId),
                    new SqlParameter("@StudentId" ,obj.FK_StudentId),
                    new SqlParameter("@ExamTypeId" ,obj.LookUpDetailId),
                    new SqlParameter("@MarksData", obj.JsonData),
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

        public Messages DeleteSchoolConfigurationData(int pkId)
        {
            Messages objMessages = new Messages();
            _commandText = "[dbo].[usp_DeleteSubjectMarkMappingData]";
            List<SqlParameter> parms = new List<SqlParameter>
               {
                    new SqlParameter("@iPkId",pkId)
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
