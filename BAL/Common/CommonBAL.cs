using DAL;
using MDL;
using MDL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Common
{
    public class CommonBAL
    {
        public static List<DropDownMDL> FillCompany(int CompanyId)
        {
            return CommonDAL.FillCompany(CompanyId);
        }
        public static List<CompanywiseDataMDL> FillCompanyByID(int CompanyId)
        {
            return CommonDAL.FillCompanyById(CompanyId);
        }
        public static List<RoleMapping> FillForminModal(int CompanyId)
        {
            return CommonDAL.FillFormModal(CompanyId);
        }

        public static List<DropDownMDL> FillRole(int CompanyId)
        {
            return CommonDAL.FillRole(CompanyId);
        }

        public static List<FormMappingViewModel> GetMappingCompanyWise(int CompanyId)
        {
            return CommonDAL.GetMappingCompanyWise(CompanyId);
        }
        public static List<DropDownMDL> FillForm(int CompanyId)
        {
            return CommonDAL.FillForm(CompanyId);
        }
        public static List<DropDownMDL> FillFormByType(out List<DropDownMDL> FormListAgency, out List<DropDownMDL> FormListClient, out List<DropDownMDL> Both, int CompanyId)
        {
            return CommonDAL.FillFormByType(out FormListAgency, out FormListClient, out Both, CompanyId);
        }


        public static List<DropDownMDL> FillResourceAvailability()
        {
            return CommonDAL.FillResourceAvailability();
        }
        public static List<DropDownMDL> FillResourceSkills()
        {
            return CommonDAL.FillResourceSkills();
        }
        public static List<DropDownMDL> FillResourceCommunicationSkills()
        {
            return CommonDAL.FillResourceCommunicationSkills();
        }

        public static List<DropDownMDL> FillLocation()
        {
            return CommonDAL.FillLocation();
        }
        public static List<DropDownMDL> FillIndustriesworked()
        {
            return CommonDAL.FillIndustriesworked();
        }

        public static List<DropDownMDL> BindLocationlist()
        {
            return CommonDAL.BindLocationlist();
        }
        public static List<StateMDL> FillState(int CountryId)
        {
            return CommonDAL.FillState(CountryId);
        }
        public static List<CityMDL> FillCity(int StateId)
        {
            return CommonDAL.FillCity(StateId);
        }
        public static List<DropDownMDL> BindCity(int StateId)
        {
            return CommonDAL.BindCity(StateId);
        }
        public static List<CountryMDL> FillCountry()
        {
            return CommonDAL.FillCountry();
        }
        public static List<DropDownMDL> BindShift(int companyID)
        {
            return CommonDAL.BindShift(companyID);
        }
        public static List<DropDownMDL> FillClass()
        {
            return CommonDAL.FillClass();
        }
        public static List<DropDownMDL> FillClassCode()
        {
            return CommonDAL.FillClassCode();
        }
        public static List<DropDownMDL> FillRelation()
        {
            return CommonDAL.FillRelation();
        }
        public static List<DropDownMDL> FillAcYear()
        {
            return CommonDAL.FillAcYear();
        }
        public static List<DropDownMDL> FillMonth()
        {
            return CommonDAL.FillMonth();
        }
        public static List<DropDownMDL> BindStudent(string ClassName)
        {
            return CommonDAL.BindStudent(ClassName);
        }
        public static List<DropDownMDL> CheckStudentMonth(int Id)
        {
            return CommonDAL.CheckStudentMonth(Id);
        }
        public static List<DropDownMDL> FillIdCard()
        {
            return CommonDAL.FillIdCard();
        }
    }
}
