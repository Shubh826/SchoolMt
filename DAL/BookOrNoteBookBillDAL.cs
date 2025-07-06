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
                new SqlParameter("@PaymentMode",obj.PaymentMode),
                new SqlParameter("@DueAmount",obj.DueAmount),
                new SqlParameter("@Discount",obj.Discount),
                new SqlParameter("@PaymentDate",obj.PaymentDate),
                new SqlParameter("@FK_StudentId",obj.FK_StudentId),
                new SqlParameter("@FK_ClassId",obj.FK_ClassId)

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
                        _BookPaymentDetails.PaymentMode = objDataSet.Tables[1].Rows[0].Field<string>("PaymentMode");
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
