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
    public class FeeHeadMasterBAL
    {
        private FeeHeadMasterDAL objFeeHeadMasterDAL;
        private DataSet _dataSet;
        public FeeHeadMasterBAL()
        {
            objFeeHeadMasterDAL = new FeeHeadMasterDAL();
        }
        public bool GetAllFeeHead(out List<FeeHeadMasterMDL> _FeeHeadMasterList, out BasicPagingMDL objBasicPagingMDL, int Id, int Userid, int Fk_companyid, int RowPerpage = 20, int CurrentPage = 1, string SearchBy = "", string SearchValue = "")
        {
            _FeeHeadMasterList = new List<FeeHeadMasterMDL>();
            objBasicPagingMDL = new BasicPagingMDL();
            return objFeeHeadMasterDAL.GetAllFeeHead(out _FeeHeadMasterList, out objBasicPagingMDL, Id, Userid, Fk_companyid, RowPerpage, CurrentPage, SearchBy, SearchValue);
        }
        public Messages AddEditFeeHead(FeeHeadMasterMDL ObjFeeHeadMasterMDL)
        {
            return objFeeHeadMasterDAL.AddEditFeeHead(ObjFeeHeadMasterMDL);
        }

    }
}
