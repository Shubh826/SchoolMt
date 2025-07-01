using DAL.DataUtility;
using MDL;
using MDL.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class BookOrNoteBookMstDAL
    {
        #region
        static DataFunctions objDataFunctions = null;
        DataSet objDataSet = null;
        static string _commandText = string.Empty;
        Messages objMessages = null;
        #endregion
        public BookOrNoteBookMstDAL()
        {
            objDataFunctions = new DataFunctions();
            objMessages = new Messages();
        }

        public bool GetBookOrNoteBookData(out List<BookOrNoteBookMst> _booklist, out BasicPagingMDL objBasicPagingMDL, int Userid, int RowPerpage, int CurrentPage, int PK_BookId, string SearchBy,string SearchValue,int Fk_companyid)
        {
            bool result = false;
            _booklist = new List<BookOrNoteBookMst>();
            objBasicPagingMDL = new BasicPagingMDL(); 
            List<SqlParameter> parms = new List<SqlParameter>()
                {
                     new SqlParameter("@iPK_BookId",PK_BookId),
                     new SqlParameter("@cSearchBy",SearchBy),
                     new SqlParameter("@cSearchValue",SearchValue),
                     new SqlParameter("@iCompanyId",Fk_companyid),
                     new SqlParameter("@iUser",Userid),
                     new SqlParameter("@iRowperPage",RowPerpage),
                     new SqlParameter("@iCurrentPage",CurrentPage)

                };

            try
            {

                _commandText = "SMS.USP_Get_BookOrNoteBook";

                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        _booklist = objDataSet.Tables[1].AsEnumerable().Select(dr => new BookOrNoteBookMst()
                        {
                            PK_BookId = dr.Field<int>("PK_BookId"),
                            Type = dr.Field<string>("Type"),
                            //FK_ClassId = dr.Field<int>("FK_ClassId"),
                            ClassName = dr.Field<string>("ClassName"),
                            BookName = dr.Field<string>("BookName"),
                            NoteBookPageCount = dr.Field<int>("NoteBookPageCount"),
                            Price = dr.Field<decimal>("Price"),
                            IsActive = dr.Field<bool>("IsActive"),
                            IsDeleted = dr.Field<bool>("IsDeleted"),
                            CreatedBy = dr.Field<int>("CreatedBy")
                            //CreatedDateTime = dr.Field<string>("CreatedDateTime"),
                            //UpdatedBy = dr.Field<int?>("UpdatedBy"),  // ✅ Nullable field
                            //UpdatedDateTime = dr.Field<string?>("UpdatedDateTime")  // ✅ Nullable field
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

        public Messages AddEditBookOrNoteBook(BookOrNoteBookMst ObjBookOrNoteBookMstMDL)
        {
            Messages objMessages = new Messages();
            _commandText = "SMS.USP_Save_BookOrNoteBook"; // ✅ Updated stored procedure name

            List<SqlParameter> parms = new List<SqlParameter>
            {
                new SqlParameter("@PK_BookId", ObjBookOrNoteBookMstMDL.PK_BookId),  // ✅ 0 for insert, BookId for update
                new SqlParameter("@Type", ObjBookOrNoteBookMstMDL.Type.Trim()),
                //new SqlParameter("@FK_ClassId", ObjBookOrNoteBookMstMDL.FK_ClassId),
                new SqlParameter("@ClassName", ObjBookOrNoteBookMstMDL.ClassName),
                new SqlParameter("@BookName", ObjBookOrNoteBookMstMDL.BookName),
                new SqlParameter("@NoteBookPageCount", SqlDbType.Int) { Value = ObjBookOrNoteBookMstMDL.NoteBookPageCount },
                new SqlParameter("@Price", SqlDbType.Decimal) { Value = ObjBookOrNoteBookMstMDL.Price },
                new SqlParameter("@IsActive", ObjBookOrNoteBookMstMDL.IsActive),
                new SqlParameter("@UserId", ObjBookOrNoteBookMstMDL.CreatedBy), // ✅ CreatedBy/UpdatedBy user
                new SqlParameter("@CompId", ObjBookOrNoteBookMstMDL.CompID)
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
