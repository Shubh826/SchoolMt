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
    public class FormMasterBAL
    {
        FormMasterDAL objFormMasterDal = null;
        private DataSet _dataSet;
        public FormMasterBAL()
        {
            objFormMasterDal = new FormMasterDAL();
        }

        public Messages AddEditForm(FormMasterMDL FormMasterMDL)
        {
            return objFormMasterDal.AddEditForm(FormMasterMDL);
        }

        public bool getFormsDetails(out List<FormMasterMDL> _FormMasterlist, out BasicPagingMDL objBasicPagingMDL, int id, int RowPerpage = 10, int CurrentPage = 1, string SearchBy = "", string SearchValue = "")
        {
            _FormMasterlist = new List<FormMasterMDL>();
            objBasicPagingMDL = new BasicPagingMDL();
            return objFormMasterDal.getFormsDetails(out _FormMasterlist, out objBasicPagingMDL, id, RowPerpage, CurrentPage, SearchBy, SearchValue);
        }
        public List<FormMasterMDL> getMenu()
        {
            _dataSet = objFormMasterDal.getMenu();
            List<FormMasterMDL> forms = null;
            if (_dataSet != null)
            {
                forms = _dataSet.Tables[0].AsEnumerable().Select(r => new FormMasterMDL()
                {
                    Pk_FormId = r.Field<int>("PK_FormId"),
                    FormName = r.Field<string>("FormName")
                }).ToList();
                _dataSet.Dispose();
            }
            else
            {
                forms = new List<FormMasterMDL>();
            }
            return forms;
        }



    }
}
