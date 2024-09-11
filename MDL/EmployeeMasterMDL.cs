using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MDL
{
    public class EmployeeMasterMDL
    {
        public int PK_Employee_ID { get; set; }

        public int FK_CompanyId { get; set; }
        public string CompanyName { get; set; }
        //public string Company_Name { get; set; }

        public string Employee_Name { get; set; }
        public string Gender { get; set; }
        public string Employee_Code { get; set; }
        public string Mobile_No { get; set; }
        public string Emergency_Contact_No { get; set; }
        public HttpPostedFileBase EmpImage { get; set; }
        public string ImageName { get; set; }
        public string EMPImageUrl { get; set; }
        public string profileImage { get; set; }
        public int FK_EMPProofId { get; set; }
        public HttpPostedFileBase EmpProofImage { get; set; }
        public string EMPImageName { get; set; }
        public string EMPProofImageUrl { get; set; }
        public string EmpProofprofileImage { get; set; }
        public string EMPIdProofName { get; set; }
        public int FK_Shift_ID { get; set; }
        public string Shift_Name { get; set; }
        public string FK_ShiftID { get; set; }
        public string Shift_Value { get; set; }
        public string Shift_Start_Time { get; set; }
        public string Shift_End_Time { get; set; }

        public int FK_Region_ID { get; set; }
        public string Region_Name { get; set; }

        public int FK_Area_ID { get; set; }
        public string Area_Name { get; set; }

        public string Landmark { get; set; }
        public string Landmark_Lat { get; set; }
        public string Landmark_Long { get; set; }
        public string Address { get; set; }
        public string Address_Lat { get; set; }
        public string Address_Long { get; set; }
        public int FK_Geofence_ID { get; set; }
        public string Geofence_Name { get; set; }
        public string Color { get; set; }
        public string Center { get; set; }
        public string Geofence_Center_Lat { get; set; }
        public string Geofence_Center_Long { get; set; }
        public string Geofence_Radius { get; set; }
        public bool IsOnRoute { get; set; }
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public string Icon { get; set; }
        public string Icon_Value { get; set; }
        public string From_Location { get; set; }
        public string To_Location { get; set; }
        public string From_Pic_Loc { get; set; }
        public string To_Pic_Loc { get; set; }
        public string Distance { get; set; }
        public string Total_Time { get; set; }
        public string Employee_Mob_No { get; set; }

        public bool IsOnPickRoute { get; set; }
        public bool IsOnDropRoute { get; set; }

        public HttpPostedFileBase ExcelFile { get; set; }

        public string Remarks { get; set; }
        public string Pick_Up_Addres { get; set; }
        public string Pick_Lat { get; set; }
        public string Pick_Long { get; set; }
        public string Drop_Address { get; set; }
        public string Drop_Lat { get; set; }
        public string Drop_Long { get; set; }
        public string Trip_No { get; set; }
        public string Route_Name { get; set; }
        public string RegistrationNo { get; set; }

        public string Time { get; set; }
        public string Date { get; set; }

        public int Department_Id { get; set; }
        public string Department { get; set; }
        public int Designation_Id { get; set; }
        public string Designation { get; set; }
        public string Alternate_ContactNo { get; set; }
        public string IsWorking { get; set; }
        public string Status { get; set; }
        public string Client_Code { get; set; }
        public string Shift { get; set; }
        public string Reporting_Person { get; set; }
        public string Remark { get; set; }
        public string Alternate_Pickup_Address { get; set; }
        public string Alternate_Drop_Address { get; set; }

        public string Pickup_Address_Lat { get; set; }
        public string Pickup_Address_Long { get; set; }
        public string Drop_Address_Lat { get; set; }
        public string Drop_Address_Long { get; set; }
        public string Alternate_Pickup_Address_Lat { get; set; }
        public string Alternate_Pickup_Address_Long { get; set; }
        public string Alternate_Drop_Address_Lat { get; set; }
        public string Alternate_Drop_Address_Long { get; set; }
        public string Created_Datetime { get; set; }
        public string Updated_Datetime { get; set; }
        public string TripType { get; set; }
        public string OTP { get; set; }
        public int Sequence { get; set; } 
        public string TravelDate { get; set; } 
    }
}
