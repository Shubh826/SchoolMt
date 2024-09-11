using DAL.DataUtility;
using MDL;
using MDL.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UserMstDAL
    {
        DataFunctions objDataFunctions;
        string _commandText = string.Empty;
        DataSet objDataSet = null;
        //BasicPagingMDL objBasicPagingMDL;
        public UserMstDAL()
        {
            objDataFunctions = new DataFunctions();
        }
        public bool GetUserData(out List<UserMDL> _Userlist, out BasicPagingMDL objBasicPagingMDL, int id, int user, int cmpId, int RowPerpage, int CurrentPage, string SearchBy = "", string SearchValue = "")
        {
            bool result = false;
            objBasicPagingMDL = new BasicPagingMDL();
            _Userlist = new List<UserMDL>();
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>()
                {
                    new SqlParameter("@iCompanyId",cmpId),
                    new SqlParameter("@iUser",user),
                    new SqlParameter("@iUserid",id),
                    new SqlParameter("@iRowperPage",RowPerpage),
                    new SqlParameter("@iCurrentPage",CurrentPage),
                    new SqlParameter("@cSearchBy",SearchBy),
                    new SqlParameter("@cSearchValue",SearchValue)
                };
                _commandText = "[dbo].[usp_GetUserdetails]";

                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        _Userlist = objDataSet.Tables[1].AsEnumerable().Select(dr => new UserMDL()
                        {
                            userid = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_UserId")),
                            name = dr.Field<string>("UserName"),
                            FirstName = dr.Field<string>("FirstName"),
                            MiddleName = dr.Field<string>("MiddleName"),
                            LastName = dr.Field<string>("LastName"),
                            Password = dr.Field<string>("Password"),
                            DateOfBirth = dr.Field<string>("DateOfBirth"),
                            Gender = dr.Field<string>("Gender"),
                            Email = dr.Field<string>("Email"),
                            MobileNo = dr.Field<string>("MobileNo"),
                            Address = dr.Field<string>("Address"),
                            PhoneNo = WrapDbNull.WrapDbNullValue<string>(dr.Field<string>("PhoneNo")),
                            stateid = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_StateId")),
                            statename = dr.Field<string>("StateName"),
                            IsActive = dr.Field<bool>("IsActive"),
                            roleid = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_RoleId")),
                            RoleName = dr.Field<string>("RoleName"),
                            CompanyName = dr.Field<string>("CompanyName"),
                            fk_companyid = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_CompanyId")),
                            CreatedDate = WrapDbNull.WrapDbNullValue<DateTime>(dr.Field<DateTime?>("CreatedDate")).ToString("dd/MM/yyyy"),

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
        public Messages AddEditUser(UserMDL userMDL)
        {
            Messages objMessages = new Messages();
            _commandText = "[dbo].[usp_AddEditUser]";
            DateTime BirthDate = string.IsNullOrEmpty(userMDL.DateOfBirth) ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : DateTime.ParseExact(userMDL.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture);


            List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter("@iPK_UserId",SqlDbType.Int){Value = userMDL.userid},
                    new SqlParameter("@cUserName", userMDL.name),
                    new SqlParameter("@cFirstName", userMDL.FirstName),
                    new SqlParameter("@cMiddleName", userMDL.MiddleName),
                    new SqlParameter("@cLastName", userMDL.LastName),
                    new SqlParameter("@cPassword", userMDL.Password),
                    new SqlParameter("@iFK_RoleId",SqlDbType.Int){Value = userMDL.roleid},
                    new SqlParameter("@cGender", userMDL.Gender),
                    new SqlParameter("@cDateOfBirth", BirthDate),
                    new SqlParameter("@cEmail", userMDL.Email),
                    new SqlParameter("@cMobileNo", userMDL.MobileNo),
                    new SqlParameter("@cAddress", userMDL.Address),
                    new SqlParameter("@cPhoneNo", userMDL.PhoneNo),
                    new SqlParameter("@bIsActive", userMDL.IsActive),
                    new SqlParameter("@bIsDeleted", userMDL.IsDeleted),
                    new SqlParameter("@iCreatedBy",SqlDbType.Int){Value =userMDL.CreatedBy},
                    new SqlParameter("@iUpdatedBy",SqlDbType.Int){Value =userMDL.UpdatedBy},
                    new SqlParameter("@iClientId",SqlDbType.Int) {Value=userMDL.Fk_ClientId},
                    new SqlParameter("@iFK_CompanyId",SqlDbType.Int) {Value=userMDL.fk_companyid},

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
        public Messages DeleteUser(int id, int createdBy)
        {
            Messages objMessages = new Messages();
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>()
                {
                    new SqlParameter("@iPK_UserId",id),
                    new SqlParameter("@iCreatedBy",createdBy),
                };
                _commandText = "USP_DeleteUser";

                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        objMessages.Message_Id = objDataSet.Tables[0].Rows[0].Field<int>("Message_Id");
                        objMessages.Message = objDataSet.Tables[0].Rows[0].Field<string>("Message");
                    }
                    else
                    {
                        objMessages.Message_Id = 0;
                        objMessages.Message = objDataSet.Tables[0].Rows[0].Field<string>("Message");// "Failed";
                    }
                }
                else
                {
                    objMessages.Message_Id = 0;
                    objMessages.Message = objDataSet.Tables[0].Rows[0].Field<string>("Message");// "Failed";
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
