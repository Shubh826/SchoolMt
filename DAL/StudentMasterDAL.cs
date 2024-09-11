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
    public class StudentMasterDAL
    {
        static DataFunctions objDataFunctions = null;
        System.Data.DataSet objDataSet = null;
        static string _commandText = string.Empty;

        public StudentMasterDAL()
        {
            objDataFunctions = new DataFunctions();
        }

        public Messages InsertStudentData(StudentMasterMDL objStudentMasterMDL)
        {
            Messages objMessages = new Messages();
            _commandText = "[usp_InsertStudentData]";
            List<SqlParameter> parms = new List<SqlParameter>
               {
                    new SqlParameter("@PK_SudentId"                 ,   objStudentMasterMDL.PK_SudentId),
                    new SqlParameter("@Fk_CompanyId"                ,   objStudentMasterMDL.FK_CompanyId),
                    new SqlParameter("@Student_Code"                ,   objStudentMasterMDL.Student_Code),
                    new SqlParameter("@StudentName"                 ,   objStudentMasterMDL.StudentName),
                    new SqlParameter("@ClassName"                   ,   objStudentMasterMDL.ClassName),
                    new SqlParameter("@ClassCode"                   ,   objStudentMasterMDL.ClassCode),
                    new SqlParameter("@GuardianName"                ,   objStudentMasterMDL.GuardianName),
                    new SqlParameter("@Relation"                    ,   objStudentMasterMDL.Relation),
                    new SqlParameter("@GuardianContactNo"           ,   objStudentMasterMDL.GuardianContactNo),
                    new SqlParameter("@GuardianAlternateContactNo"  ,   objStudentMasterMDL.AlternateGuardianContactNo),
                    new SqlParameter("@EmergencyContactNo"          ,   objStudentMasterMDL.Emergency_Contact_No),
                    new SqlParameter("@StudentAddress"              ,   objStudentMasterMDL.Address),
                    //new SqlParameter("@Fk_StoppageID"               ,   objStudentMasterMDL.Fk_StoppageID),
                    new SqlParameter("@StudentImageUrl"             ,   objStudentMasterMDL.StudentImageUrl),
                    new SqlParameter("@Gender"                      ,   objStudentMasterMDL.Gender),
                    //new SqlParameter("@FK_Shift_Id"                 ,   objStudentMasterMDL.FK_ShiftId),
                    new SqlParameter("@CreatedBy"                   ,   objStudentMasterMDL.CreatedBy),
                    new SqlParameter("@IsActive"                    ,   objStudentMasterMDL.IsActive),
                    new SqlParameter("@ImageName"                   ,   objStudentMasterMDL.ImageName),
                    new SqlParameter("@RFId"                        ,   objStudentMasterMDL.RFID),
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

        public bool GetStudentData(out List<StudentMasterMDL> objStudentList, out BasicPagingMDL objBasicPagingMDL, int id, int rowPerpage, int currentPage, int FK_CompanyId, string SearchBy, string SearchValue)
        {
            objStudentList = new List<StudentMasterMDL>();
            objBasicPagingMDL = new BasicPagingMDL();
            bool result = false;
            Messages objMessages = new Messages();
            _commandText = "[usp_GetStudentData]";
            List<SqlParameter> parms = new List<SqlParameter>
               {
                    new SqlParameter("@iRowperPage",rowPerpage),
                    new SqlParameter("@iCurrentPage",currentPage),
                    new SqlParameter("@Fk_CompanyId",FK_CompanyId),
                    new SqlParameter("@SearchBy",SearchBy),
                    new SqlParameter("@SearchValue",SearchValue),
                    new SqlParameter("@PK_StudentId",id)
              };
            try
            {
                CheckParameters.ConvertNullToDBNull(parms);
                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        objStudentList = objDataSet.Tables[1].AsEnumerable().Select(dr => new StudentMasterMDL()
                        {
                            PK_SudentId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_SudentId")),
                            FK_CompanyId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("Fk_CompanyId")),
                            //Fk_StoppageID = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("Fk_StoppageID")),
                            StudentName = dr.Field<string>("StudentName"),
                            Student_Code = dr.Field<string>("Student_Code"),
                            //FatherName = dr.Field<string>("FatherName"),
                            //MotherName = dr.Field<string>("MotherName"),
                            GuardianName = dr.Field<string>("GuardianName"),
                            Relation = dr.Field<string>("Relation"),
                            Address = dr.Field<string>("StudentAddress"),
                            ClassName = dr.Field<string>("ClassName"),
                            ClassCode = dr.Field<string>("ClassCode"),
                            GuardianContactNo = dr.Field<string>("GuardianContactNo"),
                            //StoppageName = dr.Field<string>("StoppageName"),
                            StudentImageUrl = dr.Field<string>("StudentImageUrl"),
                            Gender = dr.Field<string>("Gender"),
                            AlternateGuardianContactNo = dr.Field<string>("GuardianAlternateContactNo"),
                            Emergency_Contact_No = dr.Field<string>("EmergencyContactNo"),
                            IsActive = dr.Field<bool>("IsActive"),
                            //Shift_Name = dr.Field<string>("Shift_Name"),
                            //FK_ShiftId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_Shift_Id")),
                            ImageName = dr.Field<string>("ImageName"),
                            RFID = dr.Field<string>("RFId"),

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
        public bool GetAllStoppageByCompany(out List<StoppageMDL> objStoppageList, int FK_CompanyId)
        {
            objStoppageList = new List<StoppageMDL>();
            bool result = false;
            Messages objMessages = new Messages();
            _commandText = "[SBTMS].[usp_GetAllStoppageByCompany]";
            List<SqlParameter> parms = new List<SqlParameter>
               {
                    new SqlParameter("@FK_CompanyID",FK_CompanyId)
              };
            try
            {
                CheckParameters.ConvertNullToDBNull(parms);
                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        objStoppageList = objDataSet.Tables[1].AsEnumerable().Select(dr => new StoppageMDL()
                        {
                            StoppageName = dr.Field<string>("StoppageName"),
                            Location = dr.Field<string>("Location"),
                            Pk_StoppageID = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("Pk_StoppageID")),

                        }).ToList();

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

        public bool GetAllAddressWithoutFilter(DataTable dt, int companyId, out List<StoppageMDL> objNewAddress, out List<StoppageMDL> objAlreadyAddress)
        {

            objNewAddress = new List<StoppageMDL>();
            objAlreadyAddress = new List<StoppageMDL>();
            Messages objMessages = new Messages();
            _commandText = "[SBTMS].[usp_GetFilterAddress]";
            List<SqlParameter> param = new List<SqlParameter>
            {
                new SqlParameter("@GetAddressDetails_Type", SqlDbType.Structured)
                {
                    Value=dt
                },
                new SqlParameter("@CompanyId",companyId)
            };
            try
            {
                CheckParameters.ConvertNullToDBNull(param);
                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, param);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    objNewAddress = objDataSet.Tables[0].AsEnumerable().Select(dr => new StoppageMDL()
                    {
                        StoppageName = dr.Field<string>("StoppageName")

                    }).ToList();
                }

                if (objDataSet.Tables[1].Rows.Count > 0)
                {
                    objAlreadyAddress = objDataSet.Tables[1].AsEnumerable().Select(dr => new StoppageMDL()
                    {
                        Location = dr.Field<string>("Location"),
                        StoppageName = dr.Field<string>("StoppageName"),
                        CenterLat = dr.Field<string>("Lat"),
                        CenterLong = dr.Field<string>("Long")
                    }).ToList();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public bool GetStudentList(int companyId, out List<StudentMasterMDL> objStudentLst)
        {

            objStudentLst = new List<StudentMasterMDL>();
            Messages objMessages = new Messages();
            _commandText = "[SBTMS].[usp_GetStudentList]";
            List<SqlParameter> param = new List<SqlParameter>
            {
                new SqlParameter("@FK_CompanyId",companyId)
            };
            try
            {
                CheckParameters.ConvertNullToDBNull(param);
                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, param);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    objStudentLst = objDataSet.Tables[0].AsEnumerable().Select(dr => new StudentMasterMDL()
                    {
                        Student_Code = dr.Field<string>("Student_Code"),
                        RFID = dr.Field<string>("RFId")
                    }).ToList();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
