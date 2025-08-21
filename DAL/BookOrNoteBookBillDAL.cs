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
    public class BookOrNoteBookBillDAL
    {
        #region
        DataSet objDataSet = null;
        static string _commandText = string.Empty;
        private DataFunctions objDataFunctions;
        Messages objMessages = null;
        #endregion
        public BookOrNoteBookBillDAL()
        {
            objDataFunctions = new DataFunctions();
            objMessages = new Messages();
        }
        public List<BookOrNotebookDetail> GetBooksOrNoteBooks(string Type,string ClassName)
        {
            _commandText = "SMS.USP_GetBookOrNoteBookBillDetails";
            var para = new SqlParameter[2];
            para[0] = new SqlParameter("@Type", SqlDbType.VarChar) { Value = Type };
            para[1] = new SqlParameter("@ClassName", SqlDbType.VarChar) { Value = ClassName };
            DataSet ds = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, para.ToList());

            List<BookOrNotebookDetail> _BookOrNoteBookData = new List<BookOrNotebookDetail>();
            _BookOrNoteBookData = ds.Tables[0].AsEnumerable().Select(dr => new BookOrNotebookDetail()
            {
                PK_BookId = dr.Field<int>("PK_BookId"),
                BookName = dr.Field<string>("BookName"),
                NotebookPage = dr.Field<int>("NoteBookPageCount"),
                Price = dr.Field<decimal>("Price")
            }).ToList();
            return _BookOrNoteBookData;
        }
        public bool GetBookOrNoteBookBillData(out List<BookOrNoteBookBillMDL> billList, out BasicPagingMDL objBasicPagingMDL, int id, int fk_companyid, int rowPerPage, int currentPage, string searchBy, string searchValue, string fromDate, string toDate)
        {
            billList = new List<BookOrNoteBookBillMDL>();
            objBasicPagingMDL = new BasicPagingMDL();
            bool result = false;
            Messages objMessages = new Messages();
            _commandText = "SMS.usp_GetStudentBillList";  // your proc name

            List<SqlParameter> parms = new List<SqlParameter>
            {
                new SqlParameter("@iRowperPage", rowPerPage),
                new SqlParameter("@iCurrentPage", currentPage),
                new SqlParameter("@Fk_CompanyId", fk_companyid),
                new SqlParameter("@SearchBy", searchBy),
                new SqlParameter("@SearchValue", searchValue),
                new SqlParameter("@PK_BorNBillID", id),
                new SqlParameter("@FromDate", string.IsNullOrEmpty(fromDate) ? (object)DBNull.Value : fromDate),
                new SqlParameter("@ToDate", string.IsNullOrEmpty(toDate) ? (object)DBNull.Value : toDate)
            };

            try
            {
                CheckParameters.ConvertNullToDBNull(parms);
                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);

                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        billList = objDataSet.Tables[1].AsEnumerable().Select(dr => new BookOrNoteBookBillMDL()
                        {
                            PK_BorNBillID = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_BorNBillID")),
                            BILLNo = dr.Field<string>("BILLNo"),
                            StudentName = dr.Field<string>("StudentName"),
                            FatherName = dr.Field<string>("FatherName"),
                            ClassName = dr.Field<string>("ClassName"),
                            Subtotal = WrapDbNull.WrapDbNullValue<decimal>(dr.Field<decimal?>("Subtotal")),
                            GrandTotal = WrapDbNull.WrapDbNullValue<decimal>(dr.Field<decimal?>("GrandTotal")),
                            PaymentMode = dr.Field<string>("PaymentMode"),
                            PaymentDate = dr.Field<string>("PaymentDate"),
                            IsActive = dr.Field<bool>("IsActive"),
                            IsDeleted = dr.Field<bool>("IsDeleted"),
                            CompanyId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("CompanyId")),
                            DueAmount = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("DueAmount")),
                            Discount = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("Discount"))
                        }).ToList();

                        objBasicPagingMDL = new BasicPagingMDL()
                        {
                            TotalItem = WrapDbNull.WrapDbNullValue<int>(objDataSet.Tables[2].Rows[0].Field<int?>("TotalItem")),
                            RowParPage = rowPerPage,
                            CurrentPage = currentPage
                        };

                        if (objBasicPagingMDL.TotalItem % objBasicPagingMDL.RowParPage == 0)
                        {
                            objBasicPagingMDL.TotalPage = objBasicPagingMDL.TotalItem / objBasicPagingMDL.RowParPage;
                        }
                        else
                        {
                            objBasicPagingMDL.TotalPage = (objBasicPagingMDL.TotalItem / objBasicPagingMDL.RowParPage) + 1;
                        }

                        objDataSet.Dispose();
                        result = true;
                    }
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        public Messages PostBookOrNoteBookBill(BookOrNoteBookBillMDL obj,out BookPaymentDetails _BookPaymentDetails)
        {
            Messages objMessages = new Messages();
            _BookPaymentDetails = new BookPaymentDetails();
            _commandText = "SMS.USP_InsertOrUpdateBookOrNoteBookBill"; // ✅ Updated stored procedure name

            List<SqlParameter> parms = new List<SqlParameter>
            {
                new SqlParameter("@PK_BorNBillID", obj.PK_BorNBillID),  // ✅ 0 for insert, BookId for update
                new SqlParameter("@StudentName", obj.StudentName),
                new SqlParameter("@ClassName", obj.ClassName),
                new SqlParameter("@Subtotal", SqlDbType.Decimal) { Value = obj.Subtotal },
                new SqlParameter("@GST", SqlDbType.Decimal) { Value = obj.GST },
                new SqlParameter("@GrandTotal", SqlDbType.Decimal) { Value = obj.GrandTotal },
                new SqlParameter("@CreatedBy", obj.CreatedBy), // ✅ CreatedBy/UpdatedBy user
                new SqlParameter("@CompanyId", obj.CompanyId),
                new SqlParameter("@SelectedBookIds", obj.SelectedBookIds),
                new SqlParameter("@SelectedNotebookIds", obj.SelectedNotebookIds),
                new SqlParameter("@Cash",obj.Cash),
                new SqlParameter("@Online",obj.Online),
                new SqlParameter("@DueAmount",obj.DueAmount),
                new SqlParameter("@Discount",obj.Discount),
                new SqlParameter("@PaymentDate",obj.PaymentDate),
                new SqlParameter("@FK_StudentId",obj.FK_StudentId),
                new SqlParameter("@FK_ClassId",obj.FK_ClassId),
                new SqlParameter("@Other",obj.Other)

            };

            try
            {
                CheckParameters.ConvertNullToDBNull(parms);
                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);

                if (objDataSet.Tables.Count > 0 && objDataSet.Tables[0].Rows.Count > 0)
                {
                    objMessages.Message_Id = objDataSet.Tables[0].Rows[0].Field<int>("Message_Id"); // ✅ Fixed column name
                    objMessages.Message = objDataSet.Tables[0].Rows[0].Field<string>("Message"); // ✅ Fixed column name
                }

                if(objMessages.Message_Id==1)
                {
                    if (objDataSet.Tables.Count>1 && objDataSet.Tables[1].Rows.Count > 0)
                    {
                        _BookPaymentDetails.PK_BillId = objDataSet.Tables[1].Rows[0].Field<int>("PK_BorNBillID");
                        _BookPaymentDetails.StudentName = objDataSet.Tables[1].Rows[0].Field<string>("StudentName");
                        _BookPaymentDetails.BillNo = objDataSet.Tables[1].Rows[0].Field<string>("BillNo");
                        _BookPaymentDetails.ClassName = objDataSet.Tables[1].Rows[0].Field<string>("ClassName");
                        _BookPaymentDetails.PaymentDate = objDataSet.Tables[1].Rows[0].Field<string>("BillDate");
                        _BookPaymentDetails.TotalFee = WrapDbNull.WrapDbNullValue<decimal>(objDataSet.Tables[1].Rows[0].Field<decimal?>("Subtotal"));
                        _BookPaymentDetails.Cash = objDataSet.Tables[1].Rows[0].Field<int>("Cash");
                        _BookPaymentDetails.Online = objDataSet.Tables[1].Rows[0].Field<int>("Online");
                        _BookPaymentDetails.Other = objDataSet.Tables[1].Rows[0].Field<int>("Other");
                        _BookPaymentDetails.DueAmount = objDataSet.Tables[1].Rows[0].Field<int>("DueAmount");
                        _BookPaymentDetails.Discount = objDataSet.Tables[1].Rows[0].Field<int>("Discount");
                        _BookPaymentDetails.PaidAmount = objDataSet.Tables[1].Rows[0].Field<int>("PaidAmount");
                    }

                    if (objDataSet.Tables.Count > 1 && objDataSet.Tables[2].Rows.Count > 0)
                    {
                        _BookPaymentDetails.ItemList = objDataSet.Tables[2].AsEnumerable().Select(row => new ItemDetails
                        {
                            ItemName = row.Field<string>("Item Name"),
                            ItemType = row.Field<string>("Type"),
                            PageCount = WrapDbNull.WrapDbNullValue<int>(row.Field<int?>("Pages")),
                            Quantity = WrapDbNull.WrapDbNullValue<int>(row.Field<int?>("Quantity")),
                            Price = WrapDbNull.WrapDbNullValue<decimal>(row.Field<decimal?>("Price")),
                            TotalAmount = WrapDbNull.WrapDbNullValue<decimal>(row.Field<decimal?>("Total"))
                        }).ToList();
                    }

                }
                else
                {
                    objMessages.Message_Id = 0;
                    objMessages.Message = "Operation failed!";
                }
            }
            catch (Exception ex)
            {
                objMessages.Message_Id = 0;
                objMessages.Message = "Error: " + ex.Message;
            }

            return objMessages;

        }
    }
}
