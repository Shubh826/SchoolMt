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
    public class GenerateInvoiceBAL
    {
        GenerateInvoiceDAL objGenerateInvoiceDAL = null;
        public GenerateInvoiceBAL()
        {
            objGenerateInvoiceDAL = new GenerateInvoiceDAL();
        }
        public bool GetGenerateInvoice(out List<GenerateInvoiceMDL> _GenerateInvoicelist, out BasicPagingMDL objBasicPagingMDL, int Id, int Fk_companyid, int UserId, int RowPerpage, int CurrentPage = 1, string SearchBy = "", string SearchValue = "")
        {
            return objGenerateInvoiceDAL.GetGenerateInvoice(out _GenerateInvoicelist, out objBasicPagingMDL, Id, Fk_companyid, UserId, RowPerpage, CurrentPage, SearchBy, SearchValue);
        }

        public Messages AddEditGenerateInvoice(GenerateInvoiceMDL objGenerateInvoiceMDL)
        {
            return objGenerateInvoiceDAL.AddEditGenerateInvoice(objGenerateInvoiceMDL);
        }

        public bool GetfeeHead(out List<feehead> _feeHeadlist, int Fk_companyid, int UserId)
        {
            return objGenerateInvoiceDAL.GetfeeHead(out _feeHeadlist, Fk_companyid, UserId);
        }
    }
}
