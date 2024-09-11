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
    public class EmployeeMasterDAL
    {
        #region
        static DataFunctions objDataFunctions = null;
        System.Data.DataSet objDataSet = null;
        static string _commandText = string.Empty;
        #endregion
        public EmployeeMasterDAL()
        {
            objDataFunctions = new DataFunctions();
        }
        public bool getEmployee(out List<EmployeeMasterMDL> _Employeelist, out BasicPagingMDL objBasicPagingMDL, int id, int CompanyId, int userId, int rowPerpage, int currentPage, string searchBy, string searchValue)
        {
            bool result = false;
            _Employeelist = new List<EmployeeMasterMDL>();
            System.Data.DataSet objDataSet = null;
            objBasicPagingMDL = new BasicPagingMDL();
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>()
                {
                    new SqlParameter("@iPK_Employee_ID",id),
                    new SqlParameter("@iFK_CompanyID",CompanyId),
                    new SqlParameter("@iUserId",userId),
                    new SqlParameter("@iRowperPage",rowPerpage),
                    new SqlParameter("@iCurrentPage",currentPage),
                    new SqlParameter("@cSearchBy",searchBy),
                    new SqlParameter("@cSearchValue",searchValue)
                };
                _commandText = "[usp_GetEmployees]";
                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        _Employeelist = objDataSet.Tables[1].AsEnumerable().Select(dr => new EmployeeMasterMDL()
                        {
                            PK_Employee_ID = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_Employee_Id")),
                            FK_CompanyId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_Company_ID")),
                            Employee_Code = dr.Field<string>("Employee_Code"),
                            Employee_Name = dr.Field<string>("Employee_Name"),
                            Gender = dr.Field<string>("Gender"),
                            Mobile_No = dr.Field<string>("Mobile_No"),
                            Emergency_Contact_No = dr.Field<string>("Emergency_Contact_No"),
                            FK_Shift_ID = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_Shift_ID")),
                            Shift_Start_Time = dr.Field<string>("Shift_Start_Time"),
                            Shift_End_Time = dr.Field<string>("Shift_End_Time"),
                            FK_Region_ID = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_Region_ID")),
                            Region_Name = dr.Field<string>("Region_Name"),
                            FK_Area_ID = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_Area_ID")),
                            Area_Name = dr.Field<string>("Area_Name"),
                            Landmark = dr.Field<string>("Landmark"),
                            Landmark_Lat = dr.Field<string>("Landmark_Lat"),
                            Landmark_Long = dr.Field<string>("Landmark_Long"),
                            Address = dr.Field<string>("Address"),
                            Address_Lat = dr.Field<string>("Address_Lat"),
                            Address_Long = dr.Field<string>("Address_Long"),
                            FK_Geofence_ID = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_Geofence_ID")),
                            CompanyName = dr.Field<string>("CompanyName"),
                            Shift_Name = dr.Field<string>("Shift_Name"),
                            IsOnRoute = dr.Field<bool>("IsOnRoute"),
                            IsActive = dr.Field<bool>("IsActive"),
                            Pick_Up_Addres = dr.Field<string>("Pick_Up_Addres"),
                            Drop_Address = dr.Field<string>("Drop_Address"),
                            Created_Datetime = dr.Field<string>("Created_Datetime"),
                            Updated_Datetime = dr.Field<string>("Updated_Datetime"),
                            OTP = dr.Field<string>("OTP"),
                            ImageName = dr.Field<string>("ImageName"),
                            EMPImageUrl = dr.Field<string>("EMPImageUrl"),
                            EMPImageName = dr.Field<string>("EMPImageName"),
                            EMPProofImageUrl = dr.Field<string>("EMPProofImageUrl"),
                            FK_EMPProofId = dr.Field<int>("FK_EMPProofId"),
                            EMPIdProofName = dr.Field<string>("EMPIdProofName"),
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
        public Messages AddEmployee(EmployeeMasterMDL objEmployeeMasterMDL)
        {
            Messages objMessages = new Messages();
            _commandText = "[usp_AddEditEmployee]";
            List<SqlParameter> parms = new List<SqlParameter>
               {
                new SqlParameter("@iPK_Employee_Id" ,       objEmployeeMasterMDL.PK_Employee_ID),
                new SqlParameter("@iFK_Company_ID" ,        objEmployeeMasterMDL.FK_CompanyId),
                new SqlParameter("@cEmployee_Code" ,        objEmployeeMasterMDL.Employee_Code),
                new SqlParameter("@cEmployee_Name" ,        objEmployeeMasterMDL.Employee_Name),
                new SqlParameter("@cMobile_No" ,            objEmployeeMasterMDL.Mobile_No),
                new SqlParameter("@cEmergency_Contact_No" , objEmployeeMasterMDL.Emergency_Contact_No),
                new SqlParameter("@iFK_Shift_ID" ,          objEmployeeMasterMDL.FK_Shift_ID),
                new SqlParameter("@cShift_Start_Time" ,     objEmployeeMasterMDL.Shift_Start_Time),
                new SqlParameter("@cShift_End_Time" ,       objEmployeeMasterMDL.Shift_End_Time),
                new SqlParameter("@iFK_Region_ID" ,         objEmployeeMasterMDL.FK_Region_ID),
                new SqlParameter("@cRegion_Name" ,          objEmployeeMasterMDL.Region_Name),
                new SqlParameter("@iFK_Area_ID" ,           objEmployeeMasterMDL.FK_Area_ID),
                new SqlParameter("@cAreaName" ,             objEmployeeMasterMDL.Area_Name),
                new SqlParameter("@cDropAddress" ,             objEmployeeMasterMDL.Drop_Address),
                new SqlParameter("@cDrop_Lat" ,         objEmployeeMasterMDL.Drop_Lat),
                new SqlParameter("@cDrop_Long" ,        objEmployeeMasterMDL.Drop_Long),
                new SqlParameter("@cPickAddress" ,              objEmployeeMasterMDL.Pick_Up_Addres),
                new SqlParameter("@cPick_Lat" ,          objEmployeeMasterMDL.Pick_Lat),
                new SqlParameter("@cPick_Long" ,         objEmployeeMasterMDL.Pick_Long),
                new SqlParameter("@bIsActive" ,             objEmployeeMasterMDL.IsActive),
                new SqlParameter("@cGender" ,               objEmployeeMasterMDL.Gender),
                new SqlParameter("@iCreatedBy" ,            objEmployeeMasterMDL.CreatedBy),//IsActive
                new SqlParameter("@ImageName"           ,   objEmployeeMasterMDL.ImageName),
                new SqlParameter("@EMPImageUrl"     ,   objEmployeeMasterMDL.EMPImageUrl),
                new SqlParameter("@EMPImageName"           ,   objEmployeeMasterMDL.EMPImageName),
                new SqlParameter("@EMPProofImageUrl"     ,   objEmployeeMasterMDL.EMPProofImageUrl),
                new SqlParameter("@iFK_EMPProofId"           ,   objEmployeeMasterMDL.FK_EMPProofId),
                new SqlParameter("@cEMPIdProofName"     ,   objEmployeeMasterMDL.EMPIdProofName),

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