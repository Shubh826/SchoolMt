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
    public class GenerateInvoiceDAL
    {
        #region
        static DataFunctions objDataFunctions = null;
        System.Data.DataSet objDataSet = null;
        static string _commandText = string.Empty;
        #endregion
        public GenerateInvoiceDAL()
        {
            objDataFunctions = new DataFunctions();
        }
        public bool GetGenerateInvoice(out List<GenerateInvoiceMDL> _GenerateInvoicelist, out BasicPagingMDL objBasicPagingMDL, int Id, int Fk_companyid, int UserId, int RowPerpage, int CurrentPage, string SearchBy, string SearchValue)
        {
            bool result = false;
            _GenerateInvoicelist = new List<GenerateInvoiceMDL>();
            System.Data.DataSet objDataSet = null;
            objBasicPagingMDL = new BasicPagingMDL();
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>()
                {
                    new SqlParameter("@iPK_InvoiceId",Id),
                    new SqlParameter("@iFK_CompanyID",Fk_companyid),
                    new SqlParameter("@iUserId",UserId),
                    new SqlParameter("@iRowperPage",RowPerpage),
                    new SqlParameter("@iCurrentPage",CurrentPage),
                    new SqlParameter("@cSearchBy",SearchBy),
                    new SqlParameter("@cSearchValue",SearchValue)
                };
                _commandText = "[usp_GetInvoice]";
                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        _GenerateInvoicelist = objDataSet.Tables[1].AsEnumerable().Select(dr => new GenerateInvoiceMDL()
                        {
                            PK_InvoiceId = WrapDbNull.WrapDbNullValue<Int64>(dr.Field<Int64?>("PK_InvoiceId")),
                            FK_CompanyId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_Company_ID")),
                            SchoolName = dr.Field<string>("SchoolName"),
                            ClassName = dr.Field<string>("ClassName"),
                            Section = dr.Field<string>("Section"),
                            StudentName = dr.Field<string>("StudentName"),
                            YearName = dr.Field<string>("YearName"),
                            Month = dr.Field<string>("Month"),
                            TotalDue = dr.Field<string>("TotalDue"),
                            GrandTotal = dr.Field<string>("grandtotal"),
                            InCash = dr.Field<string>("inCash"),
                            InAccount = dr.Field<string>("inAccount"),
                            TuitionFee = dr.Field<decimal>("Tuition Fee").ToString("F2"),
                            BusFee = dr.Field<decimal>("Bus Fee").ToString("F2"),
                            OtherCharges = dr.Field<decimal>("Other Charges").ToString("F2"),
                            Miscellaneous = dr.Field<decimal>("Miscellaneous").ToString("F2"),
                            Discount = dr.Field<decimal>("Discount").ToString("F2"),
                            PreviousDue = dr.Field<decimal>("Previous Due").ToString("F2"),
                            AdmissionFee = dr.Field<decimal>("Admission Fee").ToString("F2")


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

        public bool GetfeeHead(out List<feehead> _feeHeadlist, int Fk_companyid, int UserId)
        {
            bool result = false;
            _feeHeadlist = new List<feehead>();
            System.Data.DataSet objDataSet = null;
            
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>()
                {
                    new SqlParameter("@iFK_CompanyID",Fk_companyid),
                    new SqlParameter("@iUserId",UserId)
                };
                _commandText = "[usp_Getfeeheadlist]";
                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        _feeHeadlist = objDataSet.Tables[1].AsEnumerable().Select(dr => new feehead()
                        {
                            PK_feeheadId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_FeeHeadId")),
                            feeheadName = dr.Field<string>("FeeHeadName"),
                        }).ToList();
                       
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

        public Messages AddEditGenerateInvoice(GenerateInvoiceMDL objGenerateInvoiceMDL)
        {
            Messages objMessages = new Messages();
            _commandText = "[usp_AddEditInvoice]";
            List<SqlParameter> parms = new List<SqlParameter>
               {
                new SqlParameter("@iPK_InvoiceId" ,         objGenerateInvoiceMDL.PK_InvoiceId),
                new SqlParameter("@iFK_Company_ID" ,        objGenerateInvoiceMDL.FK_CompanyId),
                new SqlParameter("@cClassName" ,            objGenerateInvoiceMDL.ClassName),
                new SqlParameter("@iFK_StudentId" ,         objGenerateInvoiceMDL.FK_StudentId),
                new SqlParameter("@iYear" ,                 objGenerateInvoiceMDL.Year),
                new SqlParameter("@cMonth" ,                objGenerateInvoiceMDL.Month),
                new SqlParameter("@cInvoiceDetailJson" ,         objGenerateInvoiceMDL.InvoiceDetailJson),
                //new SqlParameter("@iTutionFee" ,            objGenerateInvoiceMDL.TutionFee ),
                //new SqlParameter("@iBusFee" ,               objGenerateInvoiceMDL.BusFee),
                //new SqlParameter("@iOtherCharges" ,         objGenerateInvoiceMDL.OtherCharges),
               
                new SqlParameter("@iGrandTotal" ,            objGenerateInvoiceMDL.GrandTotal),
                new SqlParameter("@iInCash" ,                objGenerateInvoiceMDL.InCash),
                new SqlParameter("@iInAccount" ,             objGenerateInvoiceMDL.InAccount),
                new SqlParameter("@iDueAmount" ,             objGenerateInvoiceMDL.DueAmount),
                new SqlParameter("@bIsActive" ,              objGenerateInvoiceMDL.IsActive),
                new SqlParameter("@iCreatedBy" ,             objGenerateInvoiceMDL.CreatedBy),//IsActive
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

