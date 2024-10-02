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
            _commandText = "[usp_InsertUpdateStudent]";
            List<SqlParameter> parms = new List<SqlParameter>
               {
                    new SqlParameter("@PK_SudentId"                 ,   objStudentMasterMDL.PK_SudentId),
                    new SqlParameter("@Fk_CompanyId"                ,   objStudentMasterMDL.FK_CompanyId),
                    new SqlParameter("@StudentName"                 ,   objStudentMasterMDL.StudentName),
                    new SqlParameter("@Gender"                      ,   objStudentMasterMDL.Gender),
                    new SqlParameter("@Category"                      ,   objStudentMasterMDL.Category),
                    new SqlParameter("@ClassName"                   ,   objStudentMasterMDL.ClassName),
                    new SqlParameter("@ClassCode"                   ,   objStudentMasterMDL.ClassCode),
                    new SqlParameter("@FatherName"                   ,   objStudentMasterMDL.FatherName),
                    new SqlParameter("@MotherName"                   ,   objStudentMasterMDL.MotherName),
                    new SqlParameter("@GuardianContactNo"           ,   objStudentMasterMDL.GuardianContactNo),
                    new SqlParameter("@EmergencyContactNo"          ,   objStudentMasterMDL.Emergency_Contact_No),
                    //new SqlParameter("@StudentAddress"              ,   objStudentMasterMDL.Address),
                    new SqlParameter("@StudentImageUrl"             ,   objStudentMasterMDL.StudentImageUrl),
                    new SqlParameter("@FK_AreaId"                 ,   objStudentMasterMDL.FK_AreaId),
                    new SqlParameter("@MonthlyFee"                   ,   objStudentMasterMDL.MonthlyFee),
                    new SqlParameter("@TransportFee"                   ,   objStudentMasterMDL.TransportFee),
                    new SqlParameter("@Discount"                   ,   objStudentMasterMDL.Discount),
                    new SqlParameter("@Address"                   ,   objStudentMasterMDL.Address),
                    new SqlParameter("@CreatedBy"                   ,   objStudentMasterMDL.CreatedBy),
                    new SqlParameter("@IsActive"                    ,   objStudentMasterMDL.IsActive),
                    new SqlParameter("@ImageName"                   ,   objStudentMasterMDL.ImageName),
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
                            FK_CompanyId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_CompanyId")),
                            StudentName = dr.Field<string>("StudentName"),
                            ClassName = dr.Field<string>("ClassName"),
                            ClassCode = dr.Field<string>("ClassCode"),
                            FatherName = dr.Field<string>("FatherName"),
                            MotherName = dr.Field<string>("MotherName"),
                            Address = dr.Field<string>("Address"),
                            GuardianContactNo = dr.Field<string>("GuardianContactNo"),
                            Emergency_Contact_No = dr.Field<string>("Emergency_Contact_No"),
                            StudentImageUrl = dr.Field<string>("StudentImageUrl"),
                            Gender = dr.Field<string>("Gender"),
                            Category = dr.Field<string>("Category"),
                            IsActive = dr.Field<bool>("IsActive"),
                            ImageName = dr.Field<string>("ImageName"),
                            FK_AreaId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_AreaId")),
                            MonthlyFee = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("MonthlyFee")),
                            TransportFee = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("TransportFee")),
                            Discount = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("Discount")),

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
        //public bool GetStudentList(int companyId, out List<StudentMasterMDL> objStudentLst)
        //{

        //    objStudentLst = new List<StudentMasterMDL>();
        //    Messages objMessages = new Messages();
        //    _commandText = "[SBTMS].[usp_GetStudentList]";
        //    List<SqlParameter> param = new List<SqlParameter>
        //    {
        //        new SqlParameter("@FK_CompanyId",companyId)
        //    };
        //    try
        //    {
        //        CheckParameters.ConvertNullToDBNull(param);
        //        objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, param);
        //        if (objDataSet.Tables[0].Rows.Count > 0)
        //        {
        //            objStudentLst = objDataSet.Tables[0].AsEnumerable().Select(dr => new StudentMasterMDL()
        //            {
        //                Student_Code = dr.Field<string>("Student_Code"),
        //                RFID = dr.Field<string>("RFId")
        //            }).ToList();
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }

        //}
    }
}
