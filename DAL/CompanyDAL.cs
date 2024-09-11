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
    public class CompanyDAL
    {
        #region
        static DataFunctions objDataFunctions = null;
        DataSet objDataSet = null;
        static string _commandText = string.Empty;
        #endregion

        public CompanyDAL()
        {
            objDataFunctions = new DataFunctions();
        }
        public bool GetCompanyDetails(out List<CompanyMDL> _Companylist, out BasicPagingMDL objBasicPagingMDL, int cmpId, int id, int rowPerpage, int currentPage, string searchBy, string searchValue)
        {
            bool result = false;
            _Companylist = new List<CompanyMDL>();
            objBasicPagingMDL = new BasicPagingMDL();
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>()
                {
                    new SqlParameter("@iCmpid",cmpId),
                      new SqlParameter("@iCompanyid",id),
                      new SqlParameter("@iRowperPage",rowPerpage),
                        new SqlParameter("@iCurrentPage",currentPage),
                         new SqlParameter("@cSearchBy",searchBy),
                        new SqlParameter("@cSearchValue",searchValue)
                };
                _commandText = "usp_GetCompanydetails ";

                objDataSet = (DataSet)objDataFunctions.getQueryResult(_commandText, DataReturnType.DataSet, parms);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (objDataSet.Tables[0].Rows[0].Field<int>("Message_Id") == 1)
                    {
                        _Companylist = objDataSet.Tables[1].AsEnumerable().Select(dr => new CompanyMDL()
                        {
                            CompanyId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_CompanyId")),
                            Address = dr.Field<string>("Address"),
                            Billing_Address = dr.Field<string>("Billing_Address"),
                            Billing_FkCityId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("Billing_FkCityId")),
                            Billing_FkCountryId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("Billing_FkCountryId")),
                            Billing_FkStateId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("Billing_FkStateId")),
                            Billing_Phone = dr.Field<string>("Billing_Phone"),
                            Billing_Pin = dr.Field<string>("Billing_Pin"),
                            AltEmailId = dr.Field<string>("AltEmailId"),
                            Company_Phone = dr.Field<string>("Company_Phone"),
                            Company_Pin = dr.Field<string>("Company_Pin"),
                            ContactAltNo = dr.Field<string>("ContactAltNo"),
                            ContactEmailId = dr.Field<string>("ContactEmailId"),
                            ContactNo = dr.Field<string>("ContactNo"),
                            ContactPerson = dr.Field<string>("ContactPerson"),

                            CompanyName = dr.Field<string>("CompanyName"),
                            CompanyParentId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("CompanyParentId")),
                            CompanyParentName = dr.Field<string>("ParentCompanyName"),
                            EmailId = dr.Field<string>("EmailId"),
                            //locationBuildAllow = dr.Field<bool>("LocationBuildAllow"),
                            MobileNo = dr.Field<string>("MobileNo"),
                            //Address = dr.Field<string>("Address"),

                            FK_CountryId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_CountryId")),
                            countryname = dr.Field<string>("CountryName"),
                            FK_StateId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_StateId")),
                            statename = dr.Field<string>("StateName"),
                            FK_CityId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_CityId")),
                            cityname = dr.Field<string>("CityName"),
                            IsActive = dr.Field<bool>("IsActive"),
                            FK_SalesHeadId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_SalesHeadId")),
                            FK_DealerId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("FK_DealerId")),

                            GstNo = dr.Field<string>("GST_No"),
                            PanNo = dr.Field<string>("PAN_No"),
                            CIN = dr.Field<string>("CIN_No"),
                            CompanyLogoName = dr.Field<string>("Company_Logo"),
                            CurrentLogoURL = dr.Field<string>("Company_Logo"),
                            chkIsSameAddress = dr.Field<bool>("IsSameAddress")


                        }).ToList();
                        objBasicPagingMDL = new BasicPagingMDL()
                        {
                            TotalItem = WrapDbNull.WrapDbNullValue<int>(objDataSet.Tables[2].Rows[0].Field<int?>("TotalItem")),
                            RowParPage = rowPerpage,
                            CurrentPage = currentPage
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


        public Messages AddEditCompany(CompanyMDL companyMDL)
        {

            if (!string.IsNullOrEmpty(companyMDL.GstNo))
            {
                companyMDL.GstNo = companyMDL.GstNo.Trim().ToUpper();
            }

            if (!string.IsNullOrEmpty(companyMDL.PanNo))
            {
                companyMDL.PanNo = companyMDL.PanNo.Trim().ToUpper();
            }

            if (!string.IsNullOrEmpty(companyMDL.CIN))
            {
                companyMDL.CIN = companyMDL.CIN.Trim().ToUpper();
            }

            Messages objMessages = new Messages();
            _commandText = "usp_AddEditCompany";
            List<SqlParameter> parms = new List<SqlParameter>()
               {

                 new SqlParameter("@iPK_CompanyId" ,SqlDbType.Int){Value = companyMDL.CompanyId},
                 new SqlParameter("@iCompanyParentId"        ,companyMDL.CompanyParentId    ),
                 new SqlParameter("@cCompanyName"          ,companyMDL.CompanyName       ),
                 new SqlParameter("@cAddress"              ,companyMDL.Address                ),
                 new SqlParameter("@cEmailId"              ,companyMDL.EmailId           ),
                 new SqlParameter("@cAltEmailId"           ,companyMDL.AltEmailId         ),
                 new SqlParameter("@cMobileNo"             ,companyMDL.MobileNo               ),
                 new SqlParameter("@ciFK_CountryId"        ,SqlDbType.Int){Value= companyMDL.FK_CountryId},
                 new SqlParameter("@iFK_StateId"           ,SqlDbType.Int){Value= companyMDL.FK_StateId},
                 new SqlParameter("@ciFK_CityId"           ,SqlDbType.Int){Value= companyMDL.FK_CityId},
                 new SqlParameter("@cCompany_Pin"          ,companyMDL.Company_Pin        ),
                 new SqlParameter("@cCompany_Phone"        ,companyMDL.Company_Phone          ),
                 new SqlParameter("@cContactPerson"        ,companyMDL.ContactPerson         ),
                 new SqlParameter("@cContactNo"            ,companyMDL.ContactNo          ),
                 new SqlParameter("@cContactAltNo"         ,companyMDL.ContactAltNo       ),
                 new SqlParameter("@cContactEmailId"       ,companyMDL.ContactEmailId       ),
                 new SqlParameter("@cBilling_Address"      ,companyMDL.Billing_Address    ),
                 new SqlParameter("@iBilling_FkCountryId"  ,SqlDbType.Int){Value= companyMDL.Billing_FkCountryId},
                 new SqlParameter("@ciBilling_FkStateId"  ,SqlDbType.Int){Value= companyMDL.Billing_FkStateId},
                 new SqlParameter("@iBilling_FkCityId"     ,SqlDbType.Int){Value= companyMDL.Billing_FkCityId},
                 new SqlParameter("@cBilling_Pin"          ,companyMDL.Billing_Pin        ),
                 new SqlParameter("@cBilling_Phone"        ,companyMDL.Billing_Phone          ),
                 new SqlParameter("@bIsActive"             ,companyMDL.IsActive           ),
                 new SqlParameter("@bIsDeleted"            ,companyMDL.IsDeleted          ),
                 new SqlParameter("@iCreatedBy"            ,SqlDbType.Int){Value= companyMDL.CreatedBy},
                 new SqlParameter("@iUpdatedBy"           ,SqlDbType.Int){Value= companyMDL.UpdatedBy},
                 new SqlParameter("@cOperation"           ,companyMDL.Operation       ),
                 //new SqlParameter("@bIsLocationBuildAllow"             ,companyMDL.locationBuildAllow           ),
                  new SqlParameter("@cGST_No"                    ,companyMDL.GstNo),
                 new SqlParameter("@cPAN_No"                    ,companyMDL.PanNo),
                 new SqlParameter("@cCIN_No"                    ,companyMDL.CIN),
                 new SqlParameter("@cCompany_Logo"              ,companyMDL.CompanyLogoName),
                 new SqlParameter("@bchkIsSameAddress"          ,companyMDL.chkIsSameAddress)




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
