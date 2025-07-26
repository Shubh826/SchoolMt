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

namespace DAL.WebAPI
{
    public class StudentApisDAL
    {
        #region
        DataFunctions objDataFunctions = null;
        DataSet objDataSet = null;
        DataTable objDataTable = null;
        string _commandText = string.Empty;
        #endregion
        public StudentApisDAL()
        {
            objDataFunctions = new DataFunctions();

        }
        public ServiceResult<DropDownMDL> GetClass(out List<DropDownMDL> _ClassDataList)
        {
            ServiceResult<DropDownMDL> objResult = new ServiceResult<DropDownMDL>();

            _ClassDataList = new List<DropDownMDL>();
            bool reslt = false;
            try
            {
                _commandText = "SMS.dbo_GetClass";

                DataSet objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet);

                if (objDataSet.Tables != null && objDataSet.Tables.Count > 1)
                {
                    if (objDataSet.Tables[0].Rows.Count > 0)
                    {
                        if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                        {
                            if (objDataSet.Tables[1] != null && objDataSet.Tables[1].Rows.Count > 0)
                            {
                                _ClassDataList = objDataSet.Tables[1].AsEnumerable().Select(dr => new DropDownMDL()
                                {
                                    ID = dr.Field<int>("ID"),
                                    Value = dr.Field<string>("Value"),

                                }).ToList();
                                reslt = true;
                            }
                            else
                            {
                                objResult.Data = null;
                                objResult.Result = Convert.ToBoolean(objDataSet.Tables[0].Rows[0]["Message_Id"]);
                                objResult.Message = Convert.ToString(objDataSet.Tables[0].Rows[0]["Message"]);
                            }
                        }
                        else
                        {
                            objResult.Data = null;
                            objResult.Result = false;
                            objResult.Message = "No Data Found.";
                            reslt = false;
                        }
                    }
                    else
                    {
                        objResult.Data = null;
                        objResult.Result = false;
                        objResult.Message = "No Data Found.";
                        reslt = false;
                    }
                }

                else
                {
                    objResult.Data = null;
                    objResult.Result = false;
                    objResult.Message = "No Data Found.";
                    reslt = false;

                }

                return objResult;
            }
            catch (Exception Ex)
            {
                var objBase = System.Reflection.MethodBase.GetCurrentMethod();
                objResult.Data = null;
                objResult.Result = false;
                objResult.Message = "Error Occured.";
                return objResult;
            }

        }

        public ServiceResult<DropDownMDL> GetStudents(int FK_ClassId, out List<DropDownMDL> _StudentDataList)
        {
            ServiceResult<DropDownMDL> objResult = new ServiceResult<DropDownMDL>();

            _StudentDataList = new List<DropDownMDL>();
            bool reslt = false;
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>()
                {
                     new SqlParameter("@iFK_ClassId",FK_ClassId)
                };
                _commandText = "SMS.dbo_GetStudent";

                DataSet objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);

                if (objDataSet.Tables != null && objDataSet.Tables.Count > 1)
                {
                    if (objDataSet.Tables[0].Rows.Count > 0)
                    {
                        if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                        {
                            if (objDataSet.Tables[1] != null && objDataSet.Tables[1].Rows.Count > 0)
                            {
                                _StudentDataList = objDataSet.Tables[1].AsEnumerable().Select(dr => new DropDownMDL()
                                {
                                    ID = dr.Field<int>("ID"),
                                    Value = dr.Field<string>("Value"),

                                }).ToList();
                                reslt = true;
                            }
                            else
                            {
                                objResult.Data = null;
                                objResult.Result = Convert.ToBoolean(objDataSet.Tables[0].Rows[0]["Message_Id"]);
                                objResult.Message = Convert.ToString(objDataSet.Tables[0].Rows[0]["Message"]);
                            }
                        }
                        else
                        {
                            objResult.Data = null;
                            objResult.Result = false;
                            objResult.Message = "No Data Found.";
                            reslt = false;
                        }
                    }
                    else
                    {
                        objResult.Data = null;
                        objResult.Result = false;
                        objResult.Message = "No Data Found.";
                        reslt = false;
                    }
                }

                else
                {
                    objResult.Data = null;
                    objResult.Result = false;
                    objResult.Message = "No Data Found.";
                    reslt = false;

                }

                return objResult;
            }
            catch (Exception Ex)
            {
                var objBase = System.Reflection.MethodBase.GetCurrentMethod();
                objResult.Data = null;
                objResult.Result = false;
                objResult.Message = "Error Occured.";
                return objResult;
            }

        }
        public Messages PostStudentImage(PostStudentImageMDL objStudentData)
        {
            bool result = false;
            Messages msg = new Messages();
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>()
                {
                     new SqlParameter("@cUrl",objStudentData.StudentImageURL),
                     new SqlParameter("@iStudentId",objStudentData.StudentId),
                     new SqlParameter("@iClassId",objStudentData.ClassId),
                     new SqlParameter("@cSection",objStudentData.SectionName)
                };
                _commandText = "USP_APIUpdateStudentImage";

                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {

                    if (objDataSet.Tables[0].Rows.Count > 0)
                    {
                        msg.Message_Id = objDataSet.Tables[0].Rows[0].Field<int>("Message_Id");
                        msg.Message = objDataSet.Tables[0].Rows[0].Field<string>("Message");
                    }
                    else
                    {
                        msg.Message_Id = 0;
                        msg.Message = "Process Failed";
                    }
                }

            }

            catch (Exception ex)
            {
                result = false;
            }
            return msg;
        }
        public ServiceResult<DropDownMDL> GetSection(out List<DropDownMDL> _SectionDataList)
        {
            ServiceResult<DropDownMDL> objResult = new ServiceResult<DropDownMDL>();

            _SectionDataList = new List<DropDownMDL>();
            bool reslt = false;
            try
            {
                _commandText = "USP_GetAllClassCode";

                DataSet objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet);

                if (objDataSet.Tables != null && objDataSet.Tables.Count > 0)
                {
                    if (objDataSet.Tables[0] != null && objDataSet.Tables[0].Rows.Count > 0)
                    {
                        _SectionDataList = objDataSet.Tables[0].AsEnumerable().Select(dr => new DropDownMDL()
                        {
                            ID = dr.Field<int>("Id"),
                            Value = dr.Field<string>("Value"),
                        }).ToList();
                        reslt = true;
                    }
                    else
                    {
                        objResult.Data = null;
                        objResult.Result = false;
                        objResult.Message = "No Data Found.";
                    }
                }

                else
                {
                    objResult.Data = null;
                    objResult.Result = false;
                    objResult.Message = "No Data Found.";
                    reslt = false;

                }

                return objResult;
            }
            catch (Exception Ex)
            {
                var objBase = System.Reflection.MethodBase.GetCurrentMethod();
                objResult.Data = null;
                objResult.Result = false;
                objResult.Message = "Error Occured.";
                return objResult;
            }

        }
    }
}
