using DAL.DataUtility;
using MDL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Account
{

    public class LoginDAL
    {
        #region
        DataFunctions objDataFunctions = null;
        //DataTable objDataTable = null;
        DataSet objDataSet = null;
        string _commandText = string.Empty;
        //static string CommandText;
        #endregion

        public LoginDAL()
        {
            objDataFunctions = new DataFunctions();
        }

        public Messages AuthenticateUser(LoginMDL ObjLoginMDL, out UserMDL _User, out List<FormMDL> _formlist)
        {
            Messages objMessages = new Messages();
            bool reslt = false;
            _User = new UserMDL();
            // _CompanyRightsMDL = new CompanyRightsMDL();
            _formlist = new List<FormMDL>();
            try
            {

                List<SqlParameter> parms = new List<SqlParameter>()
            {
                new SqlParameter("@cUserName",ObjLoginMDL.UserName),
                new SqlParameter("@cPassword",ObjLoginMDL.Password)
            };
                CheckParameters.ConvertNullToDBNull(parms);
                _commandText = "usp_AuthenticateUser";
                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);

                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    objMessages.Message_Id = objDataSet.Tables[0].Rows[0].Field<int>("Message_Id");
                    objMessages.Message = objDataSet.Tables[0].Rows[0].Field<string>("Message");

                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        DataRow _dr = objDataSet.Tables[1].Rows[0];

                        _User = new UserMDL()
                        {
                            userid = WrapDbNull.WrapDbNullValue<int>(_dr.Field<int?>("UserId")),
                            roleid = WrapDbNull.WrapDbNullValue<int>(_dr.Field<int?>("RoleId")),
                            fk_companyid = WrapDbNull.WrapDbNullValue<int>(_dr.Field<int?>("FK_CompanyId")),
                            name = _dr.Field<string>("Name"),
                            cityid = WrapDbNull.WrapDbNullValue<int>(_dr.Field<int?>("CityId")),
                            cityname = _dr.Field<string>("CityName"),
                            stateid = WrapDbNull.WrapDbNullValue<int>(_dr.Field<int?>("StateId")),
                            statename = _dr.Field<string>("StateName"),
                            countryid = WrapDbNull.WrapDbNullValue<int>(_dr.Field<int?>("CountryId")),
                            countryname = _dr.Field<string>("CountryName"),
                            CompanyName = _dr.Field<string>("CompanyName"),
                            Password = _dr.Field<string>("Password"),
                            logoClass = _dr.Field<string>("logoClass"),
                            //ClientId = WrapDbNull.WrapDbNullValue<int>(_dr.Field<int?>("FK_ClientId")),
                            //FK_TransporterId = WrapDbNull.WrapDbNullValue<int>(_dr.Field<int?>("TransporterId")),
                            //LoginType = _dr.Field<string>("LoginType"),
                            //IsMachineSpecific = _dr.Field<bool>("IsMachineSpecific"),
                            //Transporter_Logo = _dr.Field<string>("Transporter_Logo"),
                            //RoleName = _dr.Field<string>("RoleName"),
                            //StaffingPartner_ID = WrapDbNull.WrapDbNullValue<int>(_dr.Field<int?>("Fk_StaffingPartnerID")),
                        };

                        _formlist = objDataSet.Tables[2].AsEnumerable().Select(dr => new FormMDL()
                        {
                            PK_FormId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_FormId")),
                            FormName = dr.Field<string>("FormName"),
                            ControllerName = dr.Field<string>("ControllerName"),
                            ActionName = dr.Field<string>("ActionName"),
                            FK_ParentId = dr.Field<int>("FK_ParentId"),
                            FK_MainId = dr.Field<int>("FK_MainId"),
                            LevelId = dr.Field<int>("LevelId"),
                            SortId = dr.Field<int>("SortId"),
                            Image = dr.Field<string>("Image"),
                            CanAdd = WrapDbNull.WrapDbNullValue<bool>(dr.Field<bool?>("CanAdd")),
                            CanEdit = WrapDbNull.WrapDbNullValue<bool>(dr.Field<bool?>("CanEdit")),
                            CanDelete = WrapDbNull.WrapDbNullValue<bool>(dr.Field<bool?>("CanDelete")),
                            CanView = WrapDbNull.WrapDbNullValue<bool>(dr.Field<bool?>("CanView")),
                            ClassName = dr.Field<string>("ClassName"),
                            HomePage = Convert.ToInt32(dr.Field<string>("HomePage")),
                            Area = dr.Field<string>("Area"),
                        }).ToList();


                        objDataSet.Dispose();
                        reslt = true;

                    }
                    else
                    {
                        reslt = false;
                    }
                }
            }
            catch (Exception ex)
            {
                var objBase = System.Reflection.MethodBase.GetCurrentMethod();
                SetError("Login Controller", "Account", "Login", "Login DAL > AuthenticateUser > Catch Block", ex.Message.ToString(), "Exception", "Exception During Login: " + ex.Message.ToString());
                objMessages.Message_Id = 0;
                objMessages.Message = "Authentication Failed.";
            }
            return objMessages;
        }

        public Messages signup(signupMDL ObjsignupMDL)
        {
            Messages objMessages = new Messages();

            try
            {

                List<SqlParameter> parms = new List<SqlParameter>()
            {
                new SqlParameter("@cUserName",ObjsignupMDL.EmailID),
                new SqlParameter("@cCompanyName",ObjsignupMDL.CompanyName),
                new SqlParameter("@cFname",ObjsignupMDL.FName),
                new SqlParameter("@cLName",ObjsignupMDL.LName),
                new SqlParameter("@contactno",ObjsignupMDL.ContactNo),
                new SqlParameter("@Alternate",ObjsignupMDL.AlternateNo),
                new SqlParameter("@bisClient",ObjsignupMDL.IsClient),
                new SqlParameter("@bisboth",ObjsignupMDL.IsBoth),
                new SqlParameter("@bisagency",ObjsignupMDL.IsAgency),
                new SqlParameter("@cPassword",ObjsignupMDL.Password),
                new SqlParameter("@ConfiPassword",ObjsignupMDL.CPassword)
            };
                CheckParameters.ConvertNullToDBNull(parms);
                _commandText = "usp_SignUp";
                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);

                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    objMessages.Message_Id = objDataSet.Tables[0].Rows[0].Field<int>("Message_Id");
                    objMessages.Message = objDataSet.Tables[0].Rows[0].Field<string>("Message");

                }
            }
            catch (Exception ex)
            {
                var objBase = System.Reflection.MethodBase.GetCurrentMethod();
                SetError("Login Controller", "Signup", "Login", "Login DAL > signup > Catch Block", ex.Message.ToString(), "Exception", "Exception During Signup: " + ex.Message.ToString());
                objMessages.Message_Id = 0;
                objMessages.Message = "Signup Failed.";
            }
            return objMessages;
        }


        public Messages InsertResetcode(string resetcode,string account)
        {
            Messages objMessages = new Messages();
            int isadd = 1;

            try
            {

                List<SqlParameter> parms = new List<SqlParameter>()
            {
                new SqlParameter("@cresetcode",resetcode),
                new SqlParameter("@cEmail",account),
                new SqlParameter("@IsAdd",isadd)
            };
                CheckParameters.ConvertNullToDBNull(parms);
                _commandText = "dbo.Usp_AddandGetResetCode";
                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);

                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    objMessages.Message_Id = objDataSet.Tables[0].Rows[0].Field<int>("Message_Id");
                    objMessages.Message = objDataSet.Tables[0].Rows[0].Field<string>("Message");

                }
            }
            catch (Exception ex)
            {
                var objBase = System.Reflection.MethodBase.GetCurrentMethod();
                SetError("Login Controller", "Signup", "Login", "Login DAL > signup > Catch Block", ex.Message.ToString(), "Exception", "Exception During Signup: " + ex.Message.ToString());
                objMessages.Message_Id = 0;
                objMessages.Message = "Signup Failed.";
            }
            return objMessages;
        }
        public Messages Updatepassword(ResetPassword modal)
        {
            Messages objMessages = new Messages();
            int isadd = 1;

            try
            {

                List<SqlParameter> parms = new List<SqlParameter>()
            {
                new SqlParameter("@cresetcode",modal.ResetCode),
                new SqlParameter("@cEmail",modal.EmailID),
                new SqlParameter("@password",modal.Password)
            };
                CheckParameters.ConvertNullToDBNull(parms);
                _commandText = "dbo.Usp_UpdateNewPassword";
                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);

                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    objMessages.Message_Id = objDataSet.Tables[0].Rows[0].Field<int>("Message_Id");
                    objMessages.Message = objDataSet.Tables[0].Rows[0].Field<string>("Message");

                }
            }
            catch (Exception ex)
            {
                var objBase = System.Reflection.MethodBase.GetCurrentMethod();
                SetError("Home Controller", "Signup", "Login", "Login DAL > Reset password > Catch Block", ex.Message.ToString(), "Exception", "Exception During Signup: " + ex.Message.ToString());
                objMessages.Message_Id = 0;
                objMessages.Message = "Reset Failed.";
            }
            return objMessages;
        }



        public LoginMDL GetUserDetails(string userName)
        {
            LoginMDL objlogin = new LoginMDL();
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>()
                {
                        new SqlParameter("@cUserName",userName)
                };
                _commandText = "[dbo].[CheckUserName]";

                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        var dr = objDataSet.Tables[1].Rows[0];
                        objlogin = new LoginMDL()
                        {
                            UserName = dr.Field<string>("FullName"),
                            FullName = dr.Field<string>("UserName"),
                            Password = dr.Field<string>("Password"),
                            EmailID = dr.Field<string>("Email"),
                        };


                        objDataSet.Dispose();

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objlogin;

        }


        //public Messages ChangePassword(int id, string password)
        //{
        //    Messages objMessages = new Messages();
        //    _commandText = "usp_UpdatePassword";


        //    List<SqlParameter> parms = new List<SqlParameter>()
        //       {

        //         new SqlParameter("@iId"          ,SqlDbType.Int){Value = id },
        //         new SqlParameter("@cPassword"          ,password )

        //      };
        //    try
        //    {
        //        CheckParameters.ConvertNullToDBNull(parms);
        //        objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
        //        if (objDataSet.Tables[0].Rows.Count > 0)
        //        {
        //            objMessages.Message_Id = objDataSet.Tables[0].Rows[0].Field<int>("Message_Id");
        //            objMessages.Message = objDataSet.Tables[0].Rows[0].Field<string>("Message");
        //        }
        //        else
        //        {
        //            objMessages.Message_Id = 0;
        //            objMessages.Message = "Password updated successfully";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objMessages.Message_Id = 0;
        //        objMessages.Message = "Password not updated successfully";
        //    }
        //    return objMessages;
        //}
        //public LoginMDL GetUserDetails(string userName)
        //{
        //    LoginMDL objlogin = new LoginMDL();
        //    try
        //    {
        //        List<SqlParameter> parms = new List<SqlParameter>()
        //        {
        //                new SqlParameter("@cUserName",userName)
        //        };
        //        _commandText = "usp_GetUserlists ";

        //        objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
        //        if (objDataSet.Tables[0].Rows.Count > 0)
        //        {
        //            if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
        //            {
        //                var dr = objDataSet.Tables[1].Rows[0];
        //                objlogin = new LoginMDL()
        //                {
        //                    UserName = dr.Field<string>("FullName"),
        //                    FullName = dr.Field<string>("UserName"),
        //                    Password = dr.Field<string>("Password"),
        //                    EmailID = dr.Field<string>("Email"),
        //                };


        //                objDataSet.Dispose();

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return objlogin;

        //}
        public void SetError(string name, string controller, string action, string Source, string message, string type, string Remarks)
        {
            try
            {
                _commandText = "[dbo].[usp_LogApplicationError]";
                List<SqlParameter> paramList = new List<SqlParameter>();

                SqlParameter objSqlParameter = new SqlParameter("@cSource", SqlDbType.VarChar);
                objSqlParameter.Value = Source;
                paramList.Add(objSqlParameter);

                objSqlParameter = new SqlParameter("@cAssemblyName", SqlDbType.VarChar);
                objSqlParameter.Value = name;
                paramList.Add(objSqlParameter);

                objSqlParameter = new SqlParameter("@cClassName", SqlDbType.VarChar);
                objSqlParameter.Value = controller;
                paramList.Add(objSqlParameter);

                objSqlParameter = new SqlParameter("@cMethodName", SqlDbType.VarChar);
                objSqlParameter.Value = action;
                paramList.Add(objSqlParameter);

                objSqlParameter = new SqlParameter("@cErrorMessage", SqlDbType.VarChar);
                objSqlParameter.Value = message;
                paramList.Add(objSqlParameter);

                objSqlParameter = new SqlParameter("@cErrorType", SqlDbType.VarChar);
                objSqlParameter.Value = type;
                paramList.Add(objSqlParameter);

                objSqlParameter = new SqlParameter("@cRemarks", SqlDbType.VarChar);
                objSqlParameter.Value = Remarks;
                paramList.Add(objSqlParameter);

                objDataFunctions.executeCommand(_commandText, paramList);
            }
            catch { }
        }

    }
}
