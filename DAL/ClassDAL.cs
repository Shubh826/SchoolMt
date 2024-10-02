using DAL.DataUtility;
using MDL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ClassDAL
    {
        #region
        static DataFunctions objDataFunctions = null;
        DataSet objDataSet = null;
        static string _commandText = string.Empty;
        Messages objMessages = null;
        #endregion
        public ClassDAL()
        {
            objDataFunctions = new DataFunctions();
            objMessages = new Messages();
        }

        /// <summary>
        /// Get Holiday List
        /// </summary>
        /// <param name="_HolidayDetailsMDL"></param>
        /// <param name="PK_HolidayId"></param>
        /// <param name="SearchBy"></param>
        /// <param name="SearchValue"></param>
        /// <returns></returns>
        public bool GetClassDetails(out List<ClassMdl> _ClassList, int ClassId, string SearchBy, string SearchValue,int CompanyId)
        {
            bool result = false;
            _ClassList = new List<ClassMdl>();

            List<SqlParameter> parms = new List<SqlParameter>()
                {
                     new SqlParameter("@iClassID",ClassId),
                     new SqlParameter("@cSearchBy",SearchBy),
                     new SqlParameter("@cSearchValue",SearchValue),
                     new SqlParameter("@iCompanyId",CompanyId)

                };

            try
            {

                _commandText = "[USP_Get_ClassList]";

                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        _ClassList = objDataSet.Tables[1].AsEnumerable().Select(dr => new ClassMdl()
                        {
                            ClassId = dr.Field<int>("PK_ClassId"),
                            ClassName = dr.Field<string>("ClassName"),
                            CompID = dr.Field<int>("Fk_comp_id"),
                            CompName = dr.Field<string>("CompanyName"),
                            MonthlyFee = dr.Field<int>("MonthlyFee"),
                            IsActive = dr.Field<bool>("IsActive"),
                            //Status = dr.Field<string>("Status"),
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


        public Messages AddEditClass(ClassMdl ObjClassMDL)
        {
            Messages objMessages = new Messages();
            _commandText = "[dbo].[usp_AddEditClass]";
            List<SqlParameter> parms = new List<SqlParameter>
                {

                 
                    new SqlParameter("@cClassName",ObjClassMDL.ClassName.Trim()),
                    new SqlParameter("@bIsActive", ObjClassMDL.IsActive),
                    new SqlParameter("@comp_id", ObjClassMDL.CompID),//FK_FormId
                    new SqlParameter("@iMonthlyFee",SqlDbType.Int){Value = ObjClassMDL.MonthlyFee},
                    new SqlParameter("@iClassid",SqlDbType.Int){Value = ObjClassMDL.ClassId},
                    new SqlParameter("@iCreatedBy", ObjClassMDL.CreatedBy),
                  

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
