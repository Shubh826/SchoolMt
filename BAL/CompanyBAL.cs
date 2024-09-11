using DAL;
using MDL;
using MDL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class CompanyBAL
    {
        CompanyDAL objCompanyDal = null;

        public CompanyBAL()
        {
            objCompanyDal = new CompanyDAL();
        }
        //public bool getCompanyDetails(out List<CompanyMDL> _Companylist, int id = 0)
        //{
        //    _Companylist = new List<CompanyMDL>();
        //    return objCompanyDal.GetCompanyDetails(out _Companylist, id);
        //}

        public Messages AddEditCompany(CompanyMDL companyMDL)
        {
            return objCompanyDal.AddEditCompany(companyMDL);
        }

        public bool getCompanyDetails(out List<CompanyMDL> _Companylist, out BasicPagingMDL objBasicPagingMDL, int cmpId, int id, int RowPerpage = 10, int CurrentPage = 1, string SearchBy = "", string SearchValue = "")
        {
            _Companylist = new List<CompanyMDL>();
            objBasicPagingMDL = new BasicPagingMDL();
            return objCompanyDal.GetCompanyDetails(out _Companylist, out objBasicPagingMDL, cmpId, id, RowPerpage, CurrentPage, SearchBy, SearchValue);

        }
    }
}
