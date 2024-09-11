using DAL;
using MDL;
using MDL.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BAL
{
    public class UserMstBAL
    {
        private UserMstDAL objUserDal;
        private DataSet _dataSet;
        public UserMstBAL()
        {
            objUserDal = new UserMstDAL();
        }
        public bool GetUserData(out List<UserMDL> _Userlist, out BasicPagingMDL objBasicPagingMDL, int id, int user, int cmpId, int RowPerpage = 20, int CurrentPage = 1, string SearchBy = "", string SearchValue = "")
        {
            _Userlist = new List<UserMDL>();
            objBasicPagingMDL = new BasicPagingMDL();
            return objUserDal.GetUserData(out _Userlist, out objBasicPagingMDL, id, user, cmpId, RowPerpage, CurrentPage, SearchBy, SearchValue);

        }
        public Messages AddEditUser(UserMDL userMDL)
        {
            return objUserDal.AddEditUser(userMDL);
        }
        public Messages DeleteUser(int id, int CreatedBy)
        {
            return objUserDal.DeleteUser(id, CreatedBy);
        }
    }
}
