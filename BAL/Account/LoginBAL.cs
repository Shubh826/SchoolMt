using DAL.Account;
using MDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Account
{
    public class LoginBAL
    {
        #region
        LoginDAL objLoginDAL = null;
        #endregion
        public LoginBAL()
        {
            objLoginDAL = new LoginDAL();
        }

        public Messages AuthenticateUser(LoginMDL ObjLoginMDL, out UserMDL _User, out List<FormMDL> _formlist)
        {
            return objLoginDAL.AuthenticateUser(ObjLoginMDL, out _User, out _formlist);
        }
        public Messages signup(signupMDL ObjsignupMDL)
        {
            return objLoginDAL.signup(ObjsignupMDL);
        }
        public LoginMDL GetUserDetails(string EmailID)
        {


            return objLoginDAL.GetUserDetails(EmailID);
        }
        public Messages InsertResetcode(string resetcode,string account)
        {
            return objLoginDAL.InsertResetcode(resetcode, account);
        }
        public Messages Updatepassword(ResetPassword modal)
        {
            return objLoginDAL.Updatepassword(modal);
        }
        

    }
}
