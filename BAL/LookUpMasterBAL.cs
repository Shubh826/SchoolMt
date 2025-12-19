using DAL;
using DAL.DataUtility;
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
    public class LookUpMasterBAL
    {
        LookUpMasterDAL objDAL = null;
        public LookUpMasterBAL()
        {
            objDAL = new LookUpMasterDAL();
        }
        public Messages AddEditLookUp(LookUpMDL Obj)
        {
            return objDAL.AddEditLookUp(Obj);
        }
        public bool GetLookUpData(out List<LookUpMDL> _List, int Id, string SearchBy, string SearchValue)
        {
            return objDAL.GetLookUpData(out _List, Id, SearchBy, SearchValue);
        }
        public bool GetLookUpDetailData(out List<LookUpDetailMDL> _List, out BasicPagingMDL objBasicPagingMDL, int Id, int CurrentPage, int RowPerpage, string SearchBy, string SearchValue)
        {
            return objDAL.GetLookUpDetailData(out _List,out objBasicPagingMDL, Id, CurrentPage, RowPerpage, SearchBy, SearchValue);
        }

        public Messages AddEditLookUpDetail(LookUpDetailMDL Obj)
        {
            return objDAL.AddEditLookUpDetail(Obj);
        }

        public int GetMaxId(int Id)
        {
            return objDAL.GetMaxId(Id);
        }
    }
}
