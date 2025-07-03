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
    public class ExpenseMasterDAL
    {
        static DataFunctions objDataFunctions = null;
        System.Data.DataSet objDataSet = null;
        static string _commandText = string.Empty;

        public ExpenseMasterDAL()
        {
            objDataFunctions = new DataFunctions();
        }
        public bool GetExpenseList(out List<ExpenseMasterMDL> objExpenseList, out BasicPagingMDL objBasicPagingMDL, int id, int rowPerPage, int currentPage, int FK_CompanyId, string SearchBy, string SearchValue)
        {
            objExpenseList = new List<ExpenseMasterMDL>();
            objBasicPagingMDL = new BasicPagingMDL();
            bool result = false;
            _commandText = "[SMS].[usp_GetExpenseList]";

            List<SqlParameter> parms = new List<SqlParameter>
            {
                new SqlParameter("@iRowPerPage", rowPerPage),
                new SqlParameter("@iCurrentPage", currentPage),
                new SqlParameter("@FK_CompanyId", FK_CompanyId),
                new SqlParameter("@SearchBy", SearchBy ?? string.Empty),
                new SqlParameter("@SearchValue", SearchValue ?? string.Empty),
                new SqlParameter("@Pk_ExpenseId", id)
            };

            try
            {
                CheckParameters.ConvertNullToDBNull(parms);
                DataSet objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);

                if (objDataSet != null && objDataSet.Tables.Count > 0 && objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        objExpenseList = objDataSet.Tables[1].AsEnumerable().Select(dr => new ExpenseMasterMDL()
                        {
                            PK_ExpenseId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("Pk_ExpenseId")),
                            FK_ExpenseHeadId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_ExpenseHeadId")),
                            ExpenseHeadName = dr.Field<string>("ExpenseHeadName"),
                            Date = dr.Field<string>("Date"), // already formatted dd-MM-yyyy in SP
                            Amount = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("Amount")),
                            Remarks = dr.Field<string>("Remarks")
                        }).ToList();

                        objBasicPagingMDL = new BasicPagingMDL()
                        {
                            TotalItem = WrapDbNull.WrapDbNullValue<int>(objDataSet.Tables[2].Rows[0].Field<int?>("TotalItem")),
                            RowParPage = rowPerPage,
                            CurrentPage = currentPage
                        };

                        objBasicPagingMDL.TotalPage = (objBasicPagingMDL.TotalItem + objBasicPagingMDL.RowParPage - 1) / objBasicPagingMDL.RowParPage;

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
                // Ideally log the exception here
                result = false;
            }

            return result;
        }

        public Messages AddEditExpense(ExpenseMasterMDL expenseMasterMDL)
        {
            Messages objMessages = new Messages();
            _commandText = "SMS.usp_AddEditExpense";
            List<SqlParameter> parms = new List<SqlParameter>
            {
                new SqlParameter("@Pk_ExpenseId",expenseMasterMDL.PK_ExpenseId),
                new SqlParameter("@FK_ExpenseHeadId", expenseMasterMDL.FK_ExpenseHeadId), // Assuming this is the unique identifier
                new SqlParameter("@Fk_CompanyId", expenseMasterMDL.FK_CompanyId),
                new SqlParameter("@Date", expenseMasterMDL.Date),
                new SqlParameter("@Amount", expenseMasterMDL.Amount),
                new SqlParameter("@Remarks", expenseMasterMDL.Remarks),
                new SqlParameter("@CreatedBy", expenseMasterMDL.CreatedBy),

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
                objMessages.Message = "Failed: " + ex.Message; // Optional: Log the exception message for debugging
            }

            return objMessages;
        }
    }
}
