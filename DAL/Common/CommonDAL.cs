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
    public class CommonDAL
    {
        static DataFunctions objDataFunctions = new DataFunctions();
        static string CommandText;
        public static List<DropDownMDL> FillCompany(int companyid)
        {
            CommandText = "USP_GetAllCompany";
            var para = new SqlParameter[1];
            para[0] = new SqlParameter("@iCompanyId", SqlDbType.Int) { Value = Convert.ToInt32(companyid) };
            DataSet Client = (DataSet)objDataFunctions.getQueryResult(CommandText, DataReturnType.DataSet, para.ToList());
            List<DropDownMDL> CompanyList = new List<DropDownMDL>();
            CompanyList = Client.Tables[0].AsEnumerable().Select(dr => new DropDownMDL()
            {
                ID = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_CompanyId")),
                Value = dr.Field<string>("CompanyName"),
            }).ToList();
            return CompanyList;
        }
        public static List<DropDownMDL> FillClassName(int companyid)
        {
            CommandText = "USP_GetAllClassName";
            var para = new SqlParameter[1];
            para[0] = new SqlParameter("@iCompanyId", SqlDbType.Int) { Value = Convert.ToInt32(companyid) };
            DataSet Client = (DataSet)objDataFunctions.getQueryResult(CommandText, DataReturnType.DataSet, para.ToList());
            List<DropDownMDL> CompanyList = new List<DropDownMDL>();
            CompanyList = Client.Tables[0].AsEnumerable().Select(dr => new DropDownMDL()
            {
                ID = 0,
                Value = dr.Field<string>("ClassName"),
            }).ToList();
            return CompanyList;
        }


        public static List<RoleMapping> FillFormModal(int companyId)
        {
            CommandText = "USP_GetAllForm";
            var para = new SqlParameter[1];
            para[0] = new SqlParameter("@iCompanyId", SqlDbType.Int) { Value = Convert.ToInt32(companyId) };
            DataSet Form = (DataSet)objDataFunctions.getQueryResult(CommandText, DataReturnType.DataSet, para.ToList());
            List<RoleMapping> FormList = new List<RoleMapping>();
            FormList = Form.Tables[0].AsEnumerable().Select(dr => new RoleMapping()
            {
                FK_FormId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_FormId")),
                FormName = dr.Field<string>("FormName"),
            }).ToList();
            return FormList;
        }

        public static List<CompanywiseDataMDL> FillCompanyById(int companyid)
        {
            CommandText = "USP_GetAllCompanyById";
            var para = new SqlParameter[1];
            para[0] = new SqlParameter("@iCompanyId", SqlDbType.Int) { Value = Convert.ToInt32(companyid) };
            DataSet Client = (DataSet)objDataFunctions.getQueryResult(CommandText, DataReturnType.DataSet, para.ToList());
            List<CompanywiseDataMDL> CompanyList = new List<CompanywiseDataMDL>();
            CompanyList = Client.Tables[0].AsEnumerable().Select(dr => new CompanywiseDataMDL()
            {
                ID = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_CompanyId")),
                Value = dr.Field<string>("CompanyName"),
                type = dr.Field<string>("AccountType"),
            }).ToList();
            return CompanyList;
        }
        public static List<DropDownMDL> FillForm(int companyId)
        {
            CommandText = "USP_GetAllForm";
            var para = new SqlParameter[1];
            para[0] = new SqlParameter("@iCompanyId", SqlDbType.Int) { Value = Convert.ToInt32(companyId) };
            DataSet Form = (DataSet)objDataFunctions.getQueryResult(CommandText, DataReturnType.DataSet, para.ToList());
            List<DropDownMDL> FormList = new List<DropDownMDL>();
            FormList = Form.Tables[0].AsEnumerable().Select(dr => new DropDownMDL()
            {
                ID = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_FormId")),
                Value = dr.Field<string>("FormName"),
            }).ToList();
            return FormList;
        }

        public static List<DropDownMDL> FillFormByType(out List<DropDownMDL> FormListAgency, out List<DropDownMDL> FormListClient, out List<DropDownMDL> Both, int companyId)
        {
            CommandText = "USP_GetAllFormByType";
            var para = new SqlParameter[1];
            para[0] = new SqlParameter("@iCompanyId", SqlDbType.Int) { Value = Convert.ToInt32(companyId) };
            DataSet Form = (DataSet)objDataFunctions.getQueryResult(CommandText, DataReturnType.DataSet, para.ToList());
            List<DropDownMDL> FormList = new List<DropDownMDL>();

            FormList = Form.Tables[0].AsEnumerable().Select(dr => new DropDownMDL()
            {
                ID = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_FormId")),
                Value = dr.Field<string>("FormName"),
            }).ToList();
            FormListAgency = Form.Tables[1].AsEnumerable().Select(dr => new DropDownMDL()
            {
                ID = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_FormId")),
                Value = dr.Field<string>("FormNameAgency"),
            }).ToList();
            FormListClient = Form.Tables[2].AsEnumerable().Select(dr => new DropDownMDL()
            {
                ID = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_FormId")),
                Value = dr.Field<string>("FormNameClient"),
            }).ToList();
            Both = Form.Tables[3].AsEnumerable().Select(dr => new DropDownMDL()
            {
                ID = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_FormId")),
                Value = dr.Field<string>("FormNameBoth"),
            }).ToList();
            return FormList;
        }
        public static List<DropDownMDL> FillRole(int companyId)
        {
            DataFunctions objDataFunctions = new DataFunctions();
            CommandText = "usp_GetRolelist";
            var para = new SqlParameter[1];
            para[0] = new SqlParameter("@iCompanyId", SqlDbType.Int) { Value = Convert.ToInt32(companyId) };
            DataSet Role = (DataSet)objDataFunctions.getQueryResult(CommandText, DataReturnType.DataSet, para.ToList());
            List<DropDownMDL> RoleList = new List<DropDownMDL>();
            RoleList = Role.Tables[0].AsEnumerable().Select(dr => new DropDownMDL()
            {
                ID = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_RoleId")),
                Value = dr.Field<string>("RoleName"),

            }).ToList();
            return RoleList;
        }
        public static List<FormMappingViewModel> GetMappingCompanyWise(int CompanyId)
        {
            List<SqlParameter> parms = new List<SqlParameter>()
                {
                    new SqlParameter("@iCompanyID",CompanyId),
                };
            CommandText = "[dbo].[USP_GetMappingCompanyWise]";
            DataSet Mapping_ds = (DataSet)objDataFunctions.getQueryResult(CommandText, DataReturnType.DataSet, parms.ToList());
            List<FormMappingViewModel> MappingList = new List<FormMappingViewModel>();
            MappingList = Mapping_ds.Tables[0].AsEnumerable().Select(dr => new FormMappingViewModel()
            {
                MappingFor = dr.Field<string>("AppName"),

            }).ToList();
            return MappingList;
        }

        public static List<DropDownMDL> FillResourceAvailability()
        {
            CommandText = "USP_GetResourceAvailability";
            DataSet Client = (DataSet)objDataFunctions.getQueryResult(CommandText, DataReturnType.DataSet);
            List<DropDownMDL> CompanyList = new List<DropDownMDL>();
            CompanyList = Client.Tables[0].AsEnumerable().Select(dr => new DropDownMDL()
            {
                ID = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("Pk_AvailabilityId")),
                Value = dr.Field<string>("AvailabilityDuration"),
            }).ToList();
            return CompanyList;
        }
        public static List<DropDownMDL> FillResourceSkills()
        {
            CommandText = "USP_GetResourceSkills";
            DataSet Client = (DataSet)objDataFunctions.getQueryResult(CommandText, DataReturnType.DataSet);
            List<DropDownMDL> CompanyList = new List<DropDownMDL>();
            CompanyList = Client.Tables[0].AsEnumerable().Select(dr => new DropDownMDL()
            {
                ID = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("Pk_SkillsId")),
                Value = dr.Field<string>("Skills"),
            }).ToList();
            return CompanyList;
        }

        public static List<DropDownMDL> FillResourceCommunicationSkills()
        {
            CommandText = "USP_GetResourceCommunicationSkills";
            DataSet Client = (DataSet)objDataFunctions.getQueryResult(CommandText, DataReturnType.DataSet);
            List<DropDownMDL> CompanyList = new List<DropDownMDL>();
            CompanyList = Client.Tables[0].AsEnumerable().Select(dr => new DropDownMDL()
            {
                ID = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("Pk_CommunicationSkillId")),
                Value = dr.Field<string>("Level"),
            }).ToList();
            return CompanyList;
        }
        public static List<DropDownMDL> FillLocation()
        {
            CommandText = "[dbo].[USP_GetLocation]";
            DataSet Client = (DataSet)objDataFunctions.getQueryResult(CommandText, DataReturnType.DataSet);
            List<DropDownMDL> LocationList = new List<DropDownMDL>();
            LocationList = Client.Tables[0].AsEnumerable().Select(dr => new DropDownMDL()
            {
                ID = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_LocationId")),
                Value = dr.Field<string>("Location"),
            }).ToList();
            return LocationList;
        }
        public static List<DropDownMDL> FillIndustriesworked()
        {
            CommandText = "[dbo].[USP_GetIndustriesworked]";
            DataSet Industriesworked = (DataSet)objDataFunctions.getQueryResult(CommandText, DataReturnType.DataSet);
            List<DropDownMDL> IndustriesworkedList = new List<DropDownMDL>();
            IndustriesworkedList = Industriesworked.Tables[0].AsEnumerable().Select(dr => new DropDownMDL()
            {
                ID = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_IndustriesworkedId")),
                Value = dr.Field<string>("Industriesworked"),
            }).ToList();
            return IndustriesworkedList;
        }
        public static List<DropDownMDL> BindLocationlist()
        {
            CommandText = "GetLocation";
            DataSet Industriesworked = (DataSet)objDataFunctions.getQueryResult(CommandText, DataReturnType.DataSet);
            List<DropDownMDL> IndustriesworkedList = new List<DropDownMDL>();
            IndustriesworkedList = Industriesworked.Tables[0].AsEnumerable().Select(dr => new DropDownMDL()
            {
                ID = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_LocationId")),
                Value = dr.Field<string>("Loc"),
            }).ToList();
            return IndustriesworkedList;
        }
        public static List<StateMDL> FillState(int CountryId)
        {
            CommandText = "usp_GetStatelist";
            var para = new SqlParameter[1];
            para[0] = new SqlParameter("@iCountryId", SqlDbType.Int) { Value = Convert.ToInt32(CountryId) };
            DataSet State = (DataSet)objDataFunctions.getQueryResult(CommandText, DataReturnType.DataSet, para.ToList());
            List<StateMDL> StateList = new List<StateMDL>();
            StateList = State.Tables[1].AsEnumerable().Select(dr => new StateMDL()
            {
                StateId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_StateId")),
                StateName = dr.Field<string>("StateName"),
                StateAbbrevations = dr.Field<string>("StateAbbr")
            }).ToList();
            return StateList;
        }
        public static List<CityMDL> FillCity(int StateId)
        {
            CommandText = "usp_GetCitylist";
            var para = new SqlParameter[1];
            para[0] = new SqlParameter("@iStateId", SqlDbType.Int) { Value = Convert.ToInt32(StateId) };
            DataSet city = (DataSet)objDataFunctions.getQueryResult(CommandText, DataReturnType.DataSet, para.ToList());
            List<CityMDL> CityList = new List<CityMDL>();
            CityList = city.Tables[1].AsEnumerable().Select(dr => new CityMDL()
            {
                CityId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_CityId")),
                CityName = dr.Field<string>("CityName"),

            }).ToList();
            return CityList;
        }
        public static List<DropDownMDL> BindCity(int StateId)
        {
            CommandText = "usp_GetCitylist";
            var para = new SqlParameter[1];
            para[0] = new SqlParameter("@iStateId", SqlDbType.Int) { Value = Convert.ToInt32(StateId) };
            DataSet city = (DataSet)objDataFunctions.getQueryResult(CommandText, DataReturnType.DataSet, para.ToList());
            List<DropDownMDL> CityList = new List<DropDownMDL>();
            CityList = city.Tables[1].AsEnumerable().Select(dr => new DropDownMDL()
            {
                ID = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_CityId")),
                Value = dr.Field<string>("CityName"),

            }).ToList();
            return CityList;
        }
        public static List<CountryMDL> FillCountry()
        {
            CommandText = "usp_GetCountrylist";
            var para = new SqlParameter[1];

            DataSet country = (DataSet)objDataFunctions.getQueryResult(CommandText, DataReturnType.DataSet);
            List<CountryMDL> CountryList = new List<CountryMDL>();
            CountryList = country.Tables[1].AsEnumerable().Select(dr => new CountryMDL()
            {
                CountryId = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_CountryId")),
                CountryName = dr.Field<string>("CountryName"),

            }).ToList();
            return CountryList;
        }
        public static List<DropDownMDL> BindShift(int CompanyId)
        {
            CommandText = "[SBTMS].[usp_GetAllShiftByCompany]";
            var para = new SqlParameter[1];
            para[0] = new SqlParameter("@FK_CompanyId", SqlDbType.Int) { Value = Convert.ToInt32(CompanyId) };
            DataSet ds = (DataSet)objDataFunctions.getQueryResult(CommandText, DataReturnType.DataSet, para.ToList());

            List<DropDownMDL> _dropdownlist = new List<DropDownMDL>();
            _dropdownlist = ds.Tables[0].AsEnumerable().Select(dr => new DropDownMDL()
            {
                ID = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_Shift_Id")),
                Value = dr.Field<string>("Shift_Name"),

            }).ToList();
            return _dropdownlist;
        }
        public static List<DropDownMDL> FillClass()
        {
            CommandText = "USP_GetAllClass";
            DataSet Client = (DataSet)objDataFunctions.getQueryResult(CommandText, DataReturnType.DataSet);
            List<DropDownMDL> ClassList = new List<DropDownMDL>();
            ClassList = Client.Tables[0].AsEnumerable().Select(dr => new DropDownMDL()
            {
                Value = dr.Field<string>("VALUE"),
            }).ToList();
            return ClassList;
        }
        public static List<DropDownMDL> FillClassCode()
        {
            CommandText = "USP_GetAllClassCode";
            
            DataSet Client = (DataSet)objDataFunctions.getQueryResult(CommandText, DataReturnType.DataSet);
            List<DropDownMDL> ClassCodeList = new List<DropDownMDL>();
            ClassCodeList = Client.Tables[0].AsEnumerable().Select(dr => new DropDownMDL()
            {
                Value = dr.Field<string>("VALUE"),
            }).ToList();
            return ClassCodeList;
        }
        public static List<DropDownMDL> FillRelation()
        {
            CommandText = "USP_GetAllRelation";
            
            DataSet Client = (DataSet)objDataFunctions.getQueryResult(CommandText, DataReturnType.DataSet);
            List<DropDownMDL> RelationList = new List<DropDownMDL>();
            RelationList = Client.Tables[0].AsEnumerable().Select(dr => new DropDownMDL()
            {
                Value = dr.Field<string>("VALUE"),
            }).ToList();
            return RelationList;
        }
        public static List<DropDownMDL> FillAcYear()
        {
            CommandText = "USP_GetAllAcYear";
            DataSet Client = (DataSet)objDataFunctions.getQueryResult(CommandText, DataReturnType.DataSet);
            List<DropDownMDL> ClassList = new List<DropDownMDL>();
            ClassList = Client.Tables[0].AsEnumerable().Select(dr => new DropDownMDL()
            {
                ID = dr.Field<int>("ID"),
                Value = dr.Field<string>("VALUE"),
            }).ToList();
            return ClassList;
        }
        public static List<DropDownMDL> FillMonth()
        {
            CommandText = "USP_GetAllMonth";
            DataSet Client = (DataSet)objDataFunctions.getQueryResult(CommandText, DataReturnType.DataSet);
            List<DropDownMDL> ClassList = new List<DropDownMDL>();
            ClassList = Client.Tables[0].AsEnumerable().Select(dr => new DropDownMDL()
            {
                ID = dr.Field<int>("ID"),
                Value = dr.Field<string>("VALUE"),
            }).ToList();
            return ClassList;
        }
        public static List<DropDownMDL> BindStudent(string className)
        {
            CommandText = "USP_GetAllStusent";
            var para = new SqlParameter[1];
            para[0] = new SqlParameter("@className", SqlDbType.VarChar) { Value = className };
            DataSet ds = (DataSet)objDataFunctions.getQueryResult(CommandText, DataReturnType.DataSet, para.ToList());

            List<DropDownMDL> _dropdownlist = new List<DropDownMDL>();
            _dropdownlist = ds.Tables[0].AsEnumerable().Select(dr => new DropDownMDL()
            {
                ID = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("ID")),
                Value = dr.Field<string>("VALUE"),
            }).ToList();
            return _dropdownlist;
        }
        public static List<DropDownMDL> CheckStudentMonth(int Id)
        {
            CommandText = "USP_GetMonthByStudentId";
            var para = new SqlParameter[1];
            para[0] = new SqlParameter("@iFK_StudentId", SqlDbType.VarChar) { Value = Id };
            DataSet ds = (DataSet)objDataFunctions.getQueryResult(CommandText, DataReturnType.DataSet, para.ToList());

            List<DropDownMDL> _dropdownlist = new List<DropDownMDL>();
            _dropdownlist = ds.Tables[0].AsEnumerable().Select(dr => new DropDownMDL()
            {
                Value = dr.Field<string>("VALUE"),
            }).ToList();
            return _dropdownlist;
        }
        public static List<DropDownMDL> FillIdCard()
        {
            CommandText = "USP_GetAllIDProof";
            DataSet ds = (DataSet)objDataFunctions.getQueryResult(CommandText, DataReturnType.DataSet);
            List<DropDownMDL> _dropdownlist = new List<DropDownMDL>();
            _dropdownlist = ds.Tables[0].AsEnumerable().Select(dr => new DropDownMDL()
            {
                ID = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("ID")),
                Value = dr.Field<string>("VALUE"),
            }).ToList();
            return _dropdownlist;
        }
        public static List<DropDownMDL> BindArea(int CompanyId)
        {
            CommandText = "[usp_GetAreasByCompanyId]";
            var para = new SqlParameter[1];
            para[0] = new SqlParameter("@CompanyId", SqlDbType.Int) { Value = Convert.ToInt32(CompanyId) };
            DataSet ds = (DataSet)objDataFunctions.getQueryResult(CommandText, DataReturnType.DataSet, para.ToList());

            List<DropDownMDL> _dropdownlist = new List<DropDownMDL>();
            _dropdownlist = ds.Tables[0].AsEnumerable().Select(dr => new DropDownMDL()
            {
                ID = WrapDbNull.WrapDbNullValue<int>(dr.Field<int?>("PK_AreaId")),
                Value = dr.Field<string>("AreaName"),

            }).ToList();
            return _dropdownlist;
        }
    }
}
