using BAL;
using MDL.Common;
using MDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolMt.Common;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
//using ExcelDataReader;
using Excel;
using SchoolMt.Common;
using Newtonsoft.Json;
using Microsoft.Ajax.Utilities;
using Org.BouncyCastle.Asn1.X509;
using BAL.Common;
using SchoolMt.Controllers;
using FRGMBSystem.Controllers;

namespace SchoolMt.Controllers
{
    public class StudentFeeDetailsController : BasicController
    {
        // GET: StudentFeeDetails
        private List<StudentFeeDetailsMDL> _Studentlist;
        List<StudentFeeDetailsMDL> _stuLst;
        BasicPagingMDL objBasicPagingMDL = null;
        private StudentFeeDetailsBAL objStudentFeeDetailsBAL;
        StudentFeeDetailsMDL objStudentFeeDetailsMDL = null;
        public StudentFeeDetailsController()
        {
            objStudentFeeDetailsBAL = new StudentFeeDetailsBAL();
            objStudentFeeDetailsMDL = new StudentFeeDetailsMDL();
        }
        public ActionResult Index()
        {
            ViewBag.InvalidLink = false;
            if (TempData["Message"] != null)
            {
                ViewBag.Msg = (Messages)TempData["Message"];
                TempData["Message"] = null;
            }
            return View();
        }
        public PartialViewResult GetStudentFeeData(int CurrentPage = 1, string SearchBy = "", string SearchValue = "")
        {
            objStudentFeeDetailsBAL.GetStudentFeeData(out _Studentlist, out objBasicPagingMDL, 0, SessionInfo.User.fk_companyid, Convert.ToInt32(20), CurrentPage, SearchBy, SearchValue);
            ViewBag.paging = objBasicPagingMDL;
            TempData["studentlist"] = _Studentlist;
            return PartialView("_StudentFeeDetailsGrid", _Studentlist);
        }

        [HttpGet]
        public ActionResult AddBulkStudentDetails(int id = 0)
        {
            return View("AddBulkStudentDetails", objStudentFeeDetailsMDL);
        }
        public FileResult DownloadFile()
        {
            return new FilePathResult("~//App_Data//Student Fee Details.xlsx", "application/vnd.ms-excel")
            {
                FileDownloadName = "Student Fee Details.xlsx"
            };
        }
        public ActionResult ValidateExcelData(StudentFeeDetailsMDL objStudentFeeDetailsMDL, int id = 0)
        {

            List<StudentFeeDetailsMDL> _StudentNames = new List<StudentFeeDetailsMDL>();

            HttpPostedFileBase Upload = objStudentFeeDetailsMDL.ExcelFile;
            List<StudentFeeDetailsMDL> InValidRecords = new List<StudentFeeDetailsMDL>();

            List<StudentFeeDetailsValidExcelDataMDL> ValidExcel = new List<StudentFeeDetailsValidExcelDataMDL>();


            DataColumn _col = new DataColumn();

            string validateMesg = string.Empty;

            if (Upload != null && Upload.ContentLength > 0)
            {
                Stream stream = Upload.InputStream;

                IExcelDataReader reader = null;

                if (Upload.FileName.EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else if (Upload.FileName.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                else
                {
                    ModelState.AddModelError("File", "This file format not supported.");

                    ViewBag.InvalidLink = true;
                    Messages msg = new Messages();
                    msg.Message_Id = 0;
                    msg.Message = "This file format not supported";
                    TempData["Message"] = msg;

                    return RedirectToAction("Index");
                }

                reader.IsFirstRowAsColumnNames = true;
                DataSet ds = reader.AsDataSet();
                DataTable RosterData = null;
                if (ds.Tables.Count > 0)
                {
                    RosterData = ds.Tables[0];
                }


                if (RosterData != null && RosterData.Rows.Count > 0)
                {
                    RosterData = RosterData.Rows
                        .Cast<DataRow>()
                        .Where(row => !row.ItemArray.All(field => field is DBNull ||
                                 string.IsNullOrWhiteSpace(field as string)))
                        .CopyToDataTable();

                    StringBuilder sb = new StringBuilder();
                    //StringBuilder sbCol = new StringBuilder();
                    List<string> _Col = new List<string>()
                        {
                         "Student Name",
                         "Father Name",
                         "Mobile No.",
                         "Address",
                         "Admission Fee",
                         "Admission Date",
                         "April Fee",
                         "April Trans Fee",
                         "May Fee",
                         "May Trans Fee",
                         "June Fee",
                         "June Trans Fee",
                         "July Fee",
                         "July Trans Fee",
                         "August Fee",
                         "August Trans Fee",
                         "September Fee",
                         "September Trans Fee",
                         "October Fee",
                         "October Trans Fee",
                         "November Fee",
                         "November Trans Fee",
                         "December Fee",
                         "December Trans Fee",
                         "January Fee",
                         "January Trans Fee",
                         "February Fee",
                         "February Trans Fee",
                         "March Fee",
                         "March Trans Fee",
                         "Examination Fee 1",
                         "Examination Fee 2",
                         "Class Name",
                         "Applicable Month Fee",
                         "Applicable Trans Fee",
                         "Applicable Month",
                         "Previous Due Amount"
                         //"Session"
                        };

                    int x = 0;
                    DataColumnCollection columns = RosterData.Columns;
                    foreach (var item in _Col)
                    {

                        if (columns.Contains(item))
                        {
                            //RosterData.Columns[x].ColumnName = item.Trim();
                        }
                        else
                        {
                            sb.Append(item + " column not available. ");
                        }
                        x++;
                    }

                    if (sb.Length == 0)
                    {

                        DataTable dtUpdate = new DataTable();
                        dtUpdate.Columns.Add("Student Name", typeof(string));
                        dtUpdate.Columns.Add("Father Name", typeof(string));
                        dtUpdate.Columns.Add("Mobile No.", typeof(string));
                        dtUpdate.Columns.Add("Address", typeof(string));
                        dtUpdate.Columns.Add("Admission Fee", typeof(int));
                        dtUpdate.Columns.Add("Admission Date", typeof(string));
                        dtUpdate.Columns.Add("April Fee", typeof(int));
                        dtUpdate.Columns.Add("April Trans Fee", typeof(int));
                        dtUpdate.Columns.Add("May Fee", typeof(int));
                        dtUpdate.Columns.Add("May Trans Fee", typeof(int));
                        dtUpdate.Columns.Add("June Fee", typeof(int));
                        dtUpdate.Columns.Add("June Trans Fee", typeof(int));
                        dtUpdate.Columns.Add("July Fee", typeof(int));
                        dtUpdate.Columns.Add("July Trans Fee", typeof(int));
                        dtUpdate.Columns.Add("August Fee", typeof(int));
                        dtUpdate.Columns.Add("August Trans Fee", typeof(int));
                        dtUpdate.Columns.Add("September Fee", typeof(int));
                        dtUpdate.Columns.Add("September Trans Fee", typeof(int));
                        dtUpdate.Columns.Add("October Fee", typeof(int));
                        dtUpdate.Columns.Add("October Trans Fee", typeof(int));
                        dtUpdate.Columns.Add("November Fee", typeof(int));
                        dtUpdate.Columns.Add("November Trans Fee", typeof(int));
                        dtUpdate.Columns.Add("December Fee", typeof(int));
                        dtUpdate.Columns.Add("December Trans Fee", typeof(int));
                        dtUpdate.Columns.Add("January Fee", typeof(int));
                        dtUpdate.Columns.Add("January Trans Fee", typeof(int));
                        dtUpdate.Columns.Add("February Fee", typeof(int));
                        dtUpdate.Columns.Add("February Trans Fee", typeof(int));
                        dtUpdate.Columns.Add("March Fee", typeof(int));
                        dtUpdate.Columns.Add("March Trans Fee", typeof(int));
                        dtUpdate.Columns.Add("Examination Fee 1", typeof(int));
                        dtUpdate.Columns.Add("Examination Fee 2", typeof(int));
                        dtUpdate.Columns.Add("Class Name", typeof(string));
                        dtUpdate.Columns.Add("Applicable Month Fee", typeof(int));
                        dtUpdate.Columns.Add("Applicable Trans Fee", typeof(int));
                        dtUpdate.Columns.Add("Applicable Month", typeof(string));
                        dtUpdate.Columns.Add("Previous Due Amount", typeof(int));


                        foreach (DataRow dr in RosterData.Rows)
                        {
                            DataRow vdr = dtUpdate.NewRow();
                            // Assuming 'dr' is your DataRow from the original DataTable and 'vdr' is the DataRow for the updated DataTable

                            vdr["Student Name"] = Convert.ToString(dr["Student Name"]).Trim();
                            vdr["Father Name"] = Convert.ToString(dr["Father Name"]).Trim();
                            vdr["Mobile No."] = Convert.ToString(dr["Mobile No."]).Trim();
                            vdr["Address"] = Convert.ToString(dr["Address"]).Trim();
                            vdr["Admission Fee"] = Convert.ToInt32(dr["Admission Fee"]);
                            vdr["Admission Date"] = dr["Admission Date"];

                            // Assuming date formats are handled elsewhere
                            vdr["April Fee"] = Convert.ToInt32(dr["April Fee"]);
                            vdr["April Trans Fee"] = Convert.ToInt32(dr["April Trans Fee"]);
                            vdr["May Fee"] = Convert.ToInt32(dr["May Fee"]);
                            vdr["May Trans Fee"] = Convert.ToInt32(dr["May Trans Fee"]);
                            vdr["June Fee"] = Convert.ToInt32(dr["June Fee"]);
                            vdr["June Trans Fee"] = Convert.ToInt32(dr["June Trans Fee"]);
                            vdr["July Fee"] = Convert.ToInt32(dr["July Fee"]);
                            vdr["July Trans Fee"] = Convert.ToInt32(dr["July Trans Fee"]);
                            vdr["August Fee"] = Convert.ToInt32(dr["August Fee"]);
                            vdr["August Trans Fee"] = Convert.ToInt32(dr["August Trans Fee"]);
                            vdr["September Fee"] = Convert.ToInt32(dr["September Fee"]);
                            vdr["September Trans Fee"] = Convert.ToInt32(dr["September Trans Fee"]);
                            vdr["October Fee"] = Convert.ToInt32(dr["October Fee"]);
                            vdr["October Trans Fee"] = Convert.ToInt32(dr["October Trans Fee"]);
                            vdr["November Fee"] = Convert.ToInt32(dr["November Fee"]);
                            vdr["November Trans Fee"] = Convert.ToInt32(dr["November Trans Fee"]);
                            vdr["December Fee"] = Convert.ToInt32(dr["December Fee"]);
                            vdr["December Trans Fee"] = Convert.ToInt32(dr["December Trans Fee"]);
                            vdr["January Fee"] = Convert.ToInt32(dr["January Fee"]);
                            vdr["January Trans Fee"] = Convert.ToInt32(dr["January Trans Fee"]);
                            vdr["February Fee"] = Convert.ToInt32(dr["February Fee"]);
                            vdr["February Trans Fee"] = Convert.ToInt32(dr["February Trans Fee"]);
                            vdr["March Fee"] = Convert.ToInt32(dr["March Fee"]);
                            vdr["March Trans Fee"] = Convert.ToInt32(dr["March Trans Fee"]);
                            vdr["Examination Fee 1"] = Convert.ToInt32(dr["Examination Fee 1"]);
                            vdr["Examination Fee 2"] = Convert.ToInt32(dr["Examination Fee 2"]);
                            vdr["Class Name"] = Convert.ToString(dr["Class Name"]);
                            vdr["Applicable Month Fee"] = Convert.ToInt32(dr["Applicable Month Fee"]);
                            vdr["Applicable Trans Fee"] = Convert.ToInt32(dr["Applicable Trans Fee"]);
                            vdr["Applicable Month"] = Convert.ToString(dr["Applicable Month"]);
                            vdr["Previous Due Amount"] = Convert.ToInt32(dr["Previous Due Amount"]);


                            dtUpdate.Rows.Add(vdr);


                        }
                        DataRow EmptyMandatoryFieldCount = null;
                        //DataRow EmptyMandatoryFieldCount = (from DataRow r in (dtUpdate).Rows
                        //                                    where Convert.ToString(r["Student Name"]).Equals(string.Empty)
                        //                                    || Convert.ToString(r["Father Name"]).Equals(string.Empty)
                        //                                    || Convert.ToString(r["Mobile No."]).Equals(string.Empty)
                        //                                    || Convert.ToString(r["Address"]).Equals(string.Empty)
                        //                                    || (r["Admission Fee"] == DBNull.Value || Convert.ToInt32(r["Admission Fee"]) == 0)
                        //                                    || Convert.ToString(r["Admission Date"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["April Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["April Trans Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["May Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["May Trans Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["June Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["June Trans Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["July Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["July Trans Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["August Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["August Trans Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["September Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["September Trans Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["October Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["October Trans Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["November Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["November Trans Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["December Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["December Trans Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["January Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["January Trans Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["February Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["February Trans Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["March Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["March Trans Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["Examination Fee 1"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["Examination Fee 2"]).Equals(string.Empty)
                        //                                    || Convert.ToString(r["Class Name"]).Equals(string.Empty)


                        //                                    select r
                        //                                     ).FirstOrDefault();


                        //InValidRecords = dtUpdate.AsEnumerable().Where(r => Convert.ToString(r["Student Name"]).Equals(string.Empty)
                        //                                    || Convert.ToString(r["Father Name"]).Equals(string.Empty)
                        //                                    || Convert.ToString(r["Mobile No."]).Equals(string.Empty)
                        //                                    || Convert.ToString(r["Address"]).Equals(string.Empty)
                        //                                    || (r["Admission Fee"] == DBNull.Value || Convert.ToInt32(r["Admission Fee"]) == 0)
                        //                                    || Convert.ToString(r["Admission Date"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["April Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["April Trans Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["May Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["May Trans Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["June Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["June Trans Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["July Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["July Trans Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["August Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["August Trans Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["September Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["September Trans Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["October Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["October Trans Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["November Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["November Trans Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["December Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["December Trans Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["January Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["January Trans Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["February Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["February Trans Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["March Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["March Trans Fee"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["Examination Fee 1"]).Equals(string.Empty)
                        //                                    //|| Convert.ToInt32(r["Examination Fee 2"]).Equals(string.Empty)
                        //                                    || Convert.ToString(r["Class Name"]).Equals(string.Empty)



                        //      )
                        //      .Select(dr => new StudentFeeDetailsMDL
                        //      {
                        //          StudentName = Convert.ToString(dr["Student Name"]),
                        //          FatherName = Convert.ToString(dr["Father Name"]),
                        //          MobileNo = Convert.ToString(dr["Mobile No."]),
                        //          Address = Convert.ToString(dr["Address"]),
                        //          AdmissionFee = Convert.ToInt32(dr["Admission Fee"]),
                        //          AdmissionDate = Convert.ToString(dr["Admission Date"]),
                        //          //AprilFee = Convert.ToInt32(dr["April Fee"]),
                        //          //AprilTrnsFee = Convert.ToInt32(dr["April Trans Fee"]),
                        //          //MayFee = Convert.ToInt32(dr["May Fee"]),
                        //          //MayTrnsFee = Convert.ToInt32(dr["May Trans Fee"]),
                        //          //JuneFee = Convert.ToInt32(dr["June Fee"]),
                        //          //JuneTrnsFee = Convert.ToInt32(dr["June Trans Fee"]),
                        //          //JulyFee = Convert.ToInt32(dr["July Fee"]),
                        //          //JulyTrnsFee = Convert.ToInt32(dr["July Trans Fee"]),
                        //          //AugustFee = Convert.ToInt32(dr["August Fee"]),
                        //          //AugustTrnsFee = Convert.ToInt32(dr["August Trans Fee"]),
                        //          //SeptemberFee = Convert.ToInt32(dr["September Fee"]),
                        //          //SeptemberTrnsFee = Convert.ToInt32(dr["September Trans Fee"]),
                        //          //OctoberFee = Convert.ToInt32(dr["October Fee"]),
                        //          //OctoberTrnsFee = Convert.ToInt32(dr["October Trans Fee"]),
                        //          //NovemberFee = Convert.ToInt32(dr["November Fee"]),
                        //          //NovemberTrnsFee = Convert.ToInt32(dr["November Trans Fee"]),
                        //          //DecemberFee = Convert.ToInt32(dr["December Fee"]),
                        //          //DecemberTrnsFee = Convert.ToInt32(dr["December Trans Fee"]),
                        //          //JanuaryFee = Convert.ToInt32(dr["January Fee"]),
                        //          //JanuaryTrnsFee = Convert.ToInt32(dr["January Trans Fee"]),
                        //          //FebruaryFee = Convert.ToInt32(dr["February Fee"]),
                        //          //FebruaryTrnsFee = Convert.ToInt32(dr["February Trans Fee"]),
                        //          //MarchFee = Convert.ToInt32(dr["March Fee"]),
                        //          //MarchTrnsFee = Convert.ToInt32(dr["March Trans Fee"]),
                        //          //ExaminationFee1 = Convert.ToInt32(dr["Examination Fee 1"]),
                        //          //ExaminationFee2 = Convert.ToInt32(dr["Examination Fee 2"]),
                        //          ClassName = Convert.ToString(dr["Class Name"]),

                        //          // Updated Remark string
                        //          Remark = "Student Name, Father Name, Mobile No., Address, Admission Fee, Admission Date, Class Name Are Mandatory."

                        //      })
                        //      .ToList();

                        if (EmptyMandatoryFieldCount == null)
                        {
                            if (dtUpdate != null && dtUpdate.Rows.Count > 0)
                            {
                                //DataTable dtData = ValidateDuplicateInExcel(dtUpdate, InValidRecords);
                                //if (dtData != null && dtData.Rows.Count > 0)
                                //{
                                    foreach (DataRow row in dtUpdate.Rows)
                                    {

                                        if (String.IsNullOrEmpty(Convert.ToString(row["Student Name"]).Trim()))
                                        {
                                            sb.AppendLine("Please Enter Student Name. ");
                                        }


                                        if (String.IsNullOrEmpty(Convert.ToString(row["Father Name"]).Trim()))
                                        {
                                            sb.AppendLine("Please Enter Father Name. ");
                                        }


                                        if (Convert.ToString(row["Mobile No."]).Trim() == "")
                                        {
                                            //if (!IsValidateNumeric(Convert.ToString(row["Mobile No."]).Trim()))
                                            //{
                                            sb.Append("Please Enter Mobile No.");
                                            //}
                                        }

                                        if (sb.Length != 0)
                                        {
                                            StudentFeeDetailsMDL StudentInvalid = new StudentFeeDetailsMDL();
                                            StudentInvalid.StudentName = Convert.ToString(row["Student Name"]);
                                            StudentInvalid.FatherName = Convert.ToString(row["Father Name"]);
                                            StudentInvalid.MobileNo = Convert.ToString(row["Mobile No."]);
                                            StudentInvalid.ClassName = Convert.ToString(row["Class Name"]);
                                            StudentInvalid.Address = Convert.ToString(row["Address"]);
                                            StudentInvalid.AdmissionFee = Convert.ToInt32(row["Admission Fee"]);
                                            StudentInvalid.AdmissionDate = Convert.ToString(row["Admission Date"]);
                                            StudentInvalid.AprilFee = Convert.ToInt32(row["April Fee"]);
                                            StudentInvalid.AprilTrnsFee = Convert.ToInt32(row["April Trans Fee"]);
                                            StudentInvalid.MayFee = Convert.ToInt32(row["May Fee"]);
                                            StudentInvalid.MayTrnsFee = Convert.ToInt32(row["May Trans Fee"]);
                                            StudentInvalid.JuneFee = Convert.ToInt32(row["June Fee"]);
                                            StudentInvalid.JuneTrnsFee = Convert.ToInt32(row["June Trans Fee"]);
                                            StudentInvalid.JulyFee = Convert.ToInt32(row["July Fee"]);
                                            StudentInvalid.JulyTrnsFee = Convert.ToInt32(row["July Trans Fee"]);
                                            StudentInvalid.AugustFee = Convert.ToInt32(row["August Fee"]);
                                            StudentInvalid.AugustTrnsFee = Convert.ToInt32(row["August Trans Fee"]);
                                            StudentInvalid.SeptemberFee = Convert.ToInt32(row["September Fee"]);
                                            StudentInvalid.SeptemberTrnsFee = Convert.ToInt32(row["September Trans Fee"]);
                                            StudentInvalid.OctoberFee = Convert.ToInt32(row["October Fee"]);
                                            StudentInvalid.OctoberTrnsFee = Convert.ToInt32(row["October Trans Fee"]);
                                            StudentInvalid.NovemberFee = Convert.ToInt32(row["November Fee"]);
                                            StudentInvalid.NovemberTrnsFee = Convert.ToInt32(row["November Trans Fee"]);
                                            StudentInvalid.DecemberFee = Convert.ToInt32(row["December Fee"]);
                                            StudentInvalid.DecemberTrnsFee = Convert.ToInt32(row["December Trans Fee"]);
                                            StudentInvalid.JanuaryFee = Convert.ToInt32(row["January Fee"]);
                                            StudentInvalid.JanuaryTrnsFee = Convert.ToInt32(row["January Trans Fee"]);
                                            StudentInvalid.FebruaryFee = Convert.ToInt32(row["February Fee"]);
                                            StudentInvalid.FebruaryTrnsFee = Convert.ToInt32(row["February Trans Fee"]);
                                            StudentInvalid.MarchFee = Convert.ToInt32(row["March Fee"]);
                                            StudentInvalid.MarchTrnsFee = Convert.ToInt32(row["March Trans Fee"]);
                                            StudentInvalid.ExaminationFee1 = Convert.ToInt32(row["Examination Fee 1"]);
                                            StudentInvalid.ExaminationFee2 = Convert.ToInt32(row["Examination Fee 2"]);
                                            StudentInvalid.ClassName = Convert.ToString(row["Class Name"]);
                                            StudentInvalid.ApplicableMonthFee = Convert.ToInt32(row["Applicable Month Fee"]);
                                            StudentInvalid.ApplicableTrnsFee = Convert.ToInt32(row["Applicable Trans Fee"]);
                                            StudentInvalid.ApplicableMonth = Convert.ToString(row["Applicable Month"]);
                                            StudentInvalid.PreviousDueAmount = Convert.ToInt32(row["Previous Due Amount"]);
                                            StudentInvalid.Remark = Convert.ToString(sb);
                                            InValidRecords.Add(StudentInvalid);
                                            sb.Clear();
                                        }
                                        else
                                        {
                                            StudentFeeDetailsValidExcelDataMDL Studenvalid = new StudentFeeDetailsValidExcelDataMDL();
                                            DateTime admissionDate = Convert.ToDateTime(row["Admission Date"]);

                                            Studenvalid.StudentName = Convert.ToString(row["Student Name"]);
                                            Studenvalid.FatherName = Convert.ToString(row["Father Name"]);
                                            Studenvalid.MobileNo = Convert.ToString(row["Mobile No."]);
                                            Studenvalid.Address = Convert.ToString(row["Address"]);
                                            Studenvalid.AdmissionFee = Convert.ToInt32(row["Admission Fee"]);
                                            Studenvalid.AdmissionDate = admissionDate.ToString("dd/MM/yyyy"); 
                                            Studenvalid.AprilFee = Convert.ToInt32(row["April Fee"]);
                                            Studenvalid.AprilTrnsFee = Convert.ToInt32(row["April Trans Fee"]);
                                            Studenvalid.MayFee = Convert.ToInt32(row["May Fee"]);
                                            Studenvalid.MayTrnsFee = Convert.ToInt32(row["May Trans Fee"]);
                                            Studenvalid.JuneFee = Convert.ToInt32(row["June Fee"]);
                                            Studenvalid.JuneTrnsFee = Convert.ToInt32(row["June Trans Fee"]);
                                            Studenvalid.JulyFee = Convert.ToInt32(row["July Fee"]);
                                            Studenvalid.JulyTrnsFee = Convert.ToInt32(row["July Trans Fee"]);
                                            Studenvalid.AugustFee = Convert.ToInt32(row["August Fee"]);
                                            Studenvalid.AugustTrnsFee = Convert.ToInt32(row["August Trans Fee"]);
                                            Studenvalid.SeptemberFee = Convert.ToInt32(row["September Fee"]);
                                            Studenvalid.SeptemberTrnsFee = Convert.ToInt32(row["September Trans Fee"]);
                                            Studenvalid.OctoberFee = Convert.ToInt32(row["October Fee"]);
                                            Studenvalid.OctoberTrnsFee = Convert.ToInt32(row["October Trans Fee"]);
                                            Studenvalid.NovemberFee = Convert.ToInt32(row["November Fee"]);
                                            Studenvalid.NovemberTrnsFee = Convert.ToInt32(row["November Trans Fee"]);
                                            Studenvalid.DecemberFee = Convert.ToInt32(row["December Fee"]);
                                            Studenvalid.DecemberTrnsFee = Convert.ToInt32(row["December Trans Fee"]);
                                            Studenvalid.JanuaryFee = Convert.ToInt32(row["January Fee"]);
                                            Studenvalid.JanuaryTrnsFee = Convert.ToInt32(row["January Trans Fee"]);
                                            Studenvalid.FebruaryFee = Convert.ToInt32(row["February Fee"]);
                                            Studenvalid.FebruaryTrnsFee = Convert.ToInt32(row["February Trans Fee"]);
                                            Studenvalid.MarchFee = Convert.ToInt32(row["March Fee"]);
                                            Studenvalid.MarchTrnsFee = Convert.ToInt32(row["March Trans Fee"]);
                                            Studenvalid.ExaminationFee1 = Convert.ToInt32(row["Examination Fee 1"]);
                                            Studenvalid.ExaminationFee2 = Convert.ToInt32(row["Examination Fee 2"]);
                                            Studenvalid.ClassName = Convert.ToString(row["Class Name"]);
                                            Studenvalid.ApplicableMonthFee = Convert.ToInt32(row["Applicable Month Fee"]);
                                            Studenvalid.ApplicableTransFee = Convert.ToInt32(row["Applicable Trans Fee"]);
                                            Studenvalid.ApplicableMonth = Convert.ToString(row["Applicable Month"]);
                                            Studenvalid.PreviousDueAmount = Convert.ToInt32(row["Previous Due Amount"]);
                                            Studenvalid.IsActive = true;

                                            ValidExcel.Add(Studenvalid);
                                        }

                                    }

                                //}
                            }

                        }
                        else
                        {
                            Messages msg = new Messages();
                            msg.Message_Id = 2;
                            msg.Message = "Required field error.";
                            TempData["Message"] = msg;
                        }
                    }
                    else
                    {
                        Messages msg = new Messages();
                        msg.Message_Id = 0;
                        msg.Message = sb.ToString();
                        TempData["Message"] = msg;
                    }
                    // TempData["ValidExcelData"] = dtValid;
                    TempData["ValidExcelData"] = ValidExcel;
                    TempData["InValidExcelData"] = InValidRecords;


                    if (ValidExcel.Count > 0 && InValidRecords.Count > 0)
                    {
                        TempData["UploadFromExcel"] = "1";
                        //InValidExportToExcel();
                        UploadValidDataToDB();
                    }
                    if (ValidExcel.Count == 0 && InValidRecords.Count > 0)
                    {
                        TempData["UploadFromExcel"] = "1";
                        Messages msg = new Messages();
                        msg.Message_Id = 0;
                        msg.Message = "Upload Failed As All Records Are Invalid";
                        TempData["Message"] = msg;

                       // InValidExportToExcel();
                    }
                    if (ValidExcel.Count > 0 && InValidRecords.Count == 0)
                    {
                        UploadValidDataToDB();
                    }

                }
                else
                {
                    Messages msg = new Messages();
                    msg.Message_Id = 0;
                    msg.Message = "No record are available in excel file";
                    TempData["Message"] = msg;
                }


            }

            return RedirectToAction("Index");
        }
        public ActionResult UploadValidDataToDB()
        {
            TempData.Keep();
            List<StudentFeeDetailsMDL> EmpDetails = new List<StudentFeeDetailsMDL>();

            List<StudentFeeDetailsValidExcelDataMDL> _studentValidlist = (List<StudentFeeDetailsValidExcelDataMDL>)TempData["ValidExcelData"];
            List<StudentFeeDetailsMDL> InValidRecords = (List<StudentFeeDetailsMDL>)TempData["InValidExcelData"];
            string jsondata = string.Empty;
            if (_studentValidlist.Count > 0)
            {
                jsondata = JsonConvert.SerializeObject(_studentValidlist);
            }
            Messages msg = objStudentFeeDetailsBAL.UploadValidStudentDataToDB(jsondata, SessionInfo.User.userid, SessionInfo.User.fk_companyid);


            string count = Convert.ToString(_studentValidlist.Count);
            int countInvalid = InValidRecords.Count;
            if (countInvalid > 0)
            {
                msg.Message = count + " record added successfully and " + countInvalid + " found invalid.";
            }
            else
            {
                msg.Message = count + " record added successfully";
            }
            TempData["Message"] = msg;

            if (msg.Message_Id == 1)
            {
                return RedirectToAction("Index");
            }
            else
            {

            }
            return RedirectToAction("Index");
        }
        //public FileResult InValidExportToExcel()
        //{
        //    TempData.Keep();
        //    List<StudentFeeDetailsMDL> _EmployeesInvalidList = (List<StudentFeeDetailsMDL>)TempData["InValidExcelData"];
        //    string[] columns = { "First Name", "Last Name", "Role","IMEI No","Staffing Partner","Date Of Birth","Date Of Join","Gender","Email","Mobile No.",
        //                         "Emergency Contact No.","Shift Name","Area","Address","Landmark","PIN Code","Permanent Account Number (PAN)","Partner Employee Code","CTC","Net Take Home","Employee Level","Aadhaar Number","Vaco Referral","Employment Type","Reporting Person","LDAP","Referred By","Remark"
        //                        };
        //    string MDLAttr = "First_Name,Last_Name,RoleName,IMEINo,StaffingPartner,Date_Of_Birth,Date_Of_Join,Gender,Email_Id,Mobile_No,Emergency_Contact_No,Shift_Name,AreaName,Address,Landmark,Pin_Code,Pan_No,PartnerEmployeeCode,CTC,NetTakeHome,EmployeeLevels,AadharCard,VacoReferral,EmploymentTypeName,ReportingPersonName,LDAP,ReferredBy,Remark";
        //    ViewBag.InvalidLink = true;

        //    ExcelExportHelper obj = new ExcelExportHelper();

        //    return obj.ExportExcel(_EmployeesInvalidList, "Invalid Employee List", ".xls", MDLAttr, columns);
        //}

        public DataTable ValidateDuplicateInExcel(DataTable dtAllData, List<StudentFeeDetailsMDL> InValid)
        {
            DataTable Roster1 = new DataTable();
            DataTable Roster2 = new DataTable();
            DataTable Roster3 = new DataTable();
            DataTable Roster4 = new DataTable();
            //RosterData = dtAllData.Clone();

            DataTable dtAlready = new DataTable();
            DataTable AlreadyFather = new DataTable();
            DataTable AlreadyMob = new DataTable();
            dtAlready = dtAllData.Clone();

            var _StudentName = dtAllData.AsEnumerable()
           .GroupBy(r => new
           {
               Col1 = r.Field<string>("Student Name")
           }).Where(gr => gr.Count() > 1)
          .SelectMany(g => g);

            DataRow drStudentName = (_StudentName).FirstOrDefault();
            if (drStudentName != null)
            {
                dtAlready = _StudentName.CopyToDataTable();
            }

            if (dtAllData.Rows.Count != dtAlready.Rows.Count)
            {
                Roster1 = (from r in dtAllData.AsEnumerable()
                           join b in dtAlready.AsEnumerable()
                           on r["Student Name"].ToString() equals b["Student Name"].ToString()
                           into g
                           where g.Count() == 0
                           select r).CopyToDataTable();
            }
            else
            {
                Roster1 = null;
            }
            if (Roster1 != null)
            {
                var _FatherName = Roster1.AsEnumerable()
                                         .GroupBy(r => new
                                         {
                                             Col1 = r.Field<string>("Father Name")
                                         }).Where(gr => gr.Count() > 1)
                                        .SelectMany(g => g);
                DataRow drFatherName = (_FatherName).FirstOrDefault();
                if (drFatherName != null)
                {
                    AlreadyFather = _FatherName.CopyToDataTable();
                }
                if (AlreadyFather.Rows.Count != Roster1.Rows.Count)
                {
                    var Excel2 = (from r in Roster1.AsEnumerable()
                                  join b in AlreadyFather.AsEnumerable()
                                  on r["Father Name"].ToString() equals b["Father Name"].ToString()
                                  into g
                                  where g.Count() == 0
                                  select r);
                    if (Excel2.Any())
                    {
                        Roster2 = Excel2.CopyToDataTable();
                    }

                    else
                    {
                        Roster2 = null;
                    }
                }
                else
                {
                    Roster2 = null;
                }
                if (Roster2 != null)
                {
                    var _MobileNo = Roster2.AsEnumerable()
                                             .GroupBy(r => new
                                             {
                                                 Col1 = r.Field<string>("Mobile No.")
                                             }).Where(gr => gr.Count() > 1)
                                            .SelectMany(g => g);
                    DataRow drMobileNo = (_MobileNo).FirstOrDefault();
                    if (drMobileNo != null)
                    {
                        AlreadyMob = _MobileNo.CopyToDataTable();
                    }

                    if (AlreadyMob.Rows.Count > 0)
                    {
                        var Excel3 = (from r in Roster2.AsEnumerable()
                                      join b in AlreadyMob.AsEnumerable()
                                      on r["Mobile No."].ToString() equals b["Mobile No."].ToString()
                                      into g
                                      where g.Count() == 0
                                      select r);
                        if (Excel3.Any())
                        {
                            Roster3 = Excel3.CopyToDataTable();
                        }
                        else
                        {
                            Roster3 = null;
                        }
                    }
                    else
                    {
                        Roster3 = null;
                        Roster3 = Roster2.Copy();
                    }




                }
                else
                {
                    Roster3 = null;
                }
            }
            else
            {
                Roster3 = null;
            }


            if (dtAlready.Rows.Count > 0 && AlreadyFather.Rows.Count > 0 && AlreadyMob.Rows.Count > 0)
            {

                List<StudentFeeDetailsMDL> InvalidDevice = new List<StudentFeeDetailsMDL>();

                InvalidDevice = (from DataRow dr in dtAlready.Rows
                                 select new StudentFeeDetailsMDL()
                                 {
                                     StudentName = Convert.ToString(dr["Student Name"]),
                                     FatherName = Convert.ToString(dr["Father Name"]),
                                     MobileNo = Convert.ToString(dr["Mobile No."]),
                                     Address = Convert.ToString(dr["Address"]),
                                     AdmissionFee = Convert.ToInt32(dr["Admission Fee"]),
                                     AdmissionDate = Convert.ToString(dr["Admission Date"]),
                                     AprilFee = Convert.ToInt32(dr["April Fee"]),
                                     AprilTrnsFee = Convert.ToInt32(dr["April Trans Fee"]),
                                     MayFee = Convert.ToInt32(dr["May Fee"]),
                                     MayTrnsFee = Convert.ToInt32(dr["May Trans Fee"]),
                                     JuneFee = Convert.ToInt32(dr["June Fee"]),
                                     JuneTrnsFee = Convert.ToInt32(dr["June Trans Fee"]),
                                     JulyFee = Convert.ToInt32(dr["July Fee"]),
                                     JulyTrnsFee = Convert.ToInt32(dr["July Trans Fee"]),
                                     AugustFee = Convert.ToInt32(dr["August Fee"]),
                                     AugustTrnsFee = Convert.ToInt32(dr["August Trans Fee"]),
                                     SeptemberFee = Convert.ToInt32(dr["September Fee"]),
                                     SeptemberTrnsFee = Convert.ToInt32(dr["September Trans Fee"]),
                                     OctoberFee = Convert.ToInt32(dr["October Fee"]),
                                     OctoberTrnsFee = Convert.ToInt32(dr["October Trans Fee"]),
                                     NovemberFee = Convert.ToInt32(dr["November Fee"]),
                                     NovemberTrnsFee = Convert.ToInt32(dr["November Trans Fee"]),
                                     DecemberFee = Convert.ToInt32(dr["December Fee"]),
                                     DecemberTrnsFee = Convert.ToInt32(dr["December Trans Fee"]),
                                     JanuaryFee = Convert.ToInt32(dr["January Fee"]),
                                     JanuaryTrnsFee = Convert.ToInt32(dr["January Trans Fee"]),
                                     FebruaryFee = Convert.ToInt32(dr["February Fee"]),
                                     FebruaryTrnsFee = Convert.ToInt32(dr["February Trans Fee"]),
                                     MarchFee = Convert.ToInt32(dr["March Fee"]),
                                     MarchTrnsFee = Convert.ToInt32(dr["March Trans Fee"]),
                                     ExaminationFee1 = Convert.ToInt32(dr["Examination Fee 1"]),
                                     ExaminationFee2 = Convert.ToInt32(dr["Examination Fee 2"]),
                                     ClassName = Convert.ToString(dr["Class Name"]),
                                     ApplicableMonthFee = Convert.ToInt32(dr["Applicable Month Fee"]),
                                     ApplicableTrnsFee = Convert.ToInt32(dr["Applicable Trans Fee"]),
                                     ApplicableMonth = Convert.ToString(dr["Applicable Month"]),
                                     PreviousDueAmount = Convert.ToInt32(dr["Previous Due Amount"]),
                                     Remark = "This Student Details is Already Exist in Excel so Please Remove then Upload !"



                                 }).ToList();
                if (InvalidDevice.Count > 0)
                {
                    InValid.AddRange(InvalidDevice);
                }

            }

            return Roster4;
        }

        #region Excel Data Field Validation Functions
        public bool IsValidateNumeric(string mobileno)
        {
            if (Regex.IsMatch(mobileno.Trim(), "^([6-9]{1})([0-9]{9})"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsValidatePinCode(string PinCode)
        {
            if (Regex.IsMatch(PinCode.Trim(), "^([1-9]{1})([0-9]{5})"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsValidatePAN(string PanNo)
        {
            if (Regex.IsMatch(PanNo.Trim(), "^[a-zA-Z0-9 ]+$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsValidateNumber(string number)
        {
            if (Regex.IsMatch(number.Trim(), @"^[0-9]+$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string ReturnDateWithZeroOfMonthAndDay(string Date)
        {
            string validateDt = string.Empty;
            if (Date != "" && Date != string.Empty)
            {
                DateTime dt = Convert.ToDateTime(Date, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                validateDt = String.Format("{0:dd/MM/yyyy}", dt);
            }
            return validateDt;
        }
        public bool CheckDate(string Date)
        {
            try
            {
                DateTime dob;
                //DateTime dt = DateTime.ParseExact(Date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime dt = Convert.ToDateTime(Date, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                String dateTime = String.Format("{0:dd/MM/yyyy}", dt);
                return true;
            }
            catch (FormatException ex)
            {
                return false;
            }
        }
        public bool ValidateDateofBirthAndDateofJoin(string strDateofbirth, string strDateofjoin)
        {
            try
            {
                var DateOfBirth = ConvertToDateTime(strDateofbirth);
                var DateOfJoin = ConvertToDateTime(strDateofjoin);
                var yeardiff = DateOfJoin.Year - DateOfBirth.Year;
                if (yeardiff > 18)
                {
                    return true;
                }
                else
                    return false;

            }
            catch (Exception)
            {
                return false;
            }
        }
        private DateTime ConvertToDateTime(object strValue)
        {
            DateTime dt = Convert.ToDateTime(strValue, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
            String dateTime = String.Format("{0:dd/MM/yyyy}", dt);
            if (dateTime != "" && dateTime != string.Empty)
            {
                try
                {
                    return Convert.ToDateTime(dateTime, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                }
                catch (Exception ex)
                {
                    return DateTime.MinValue;
                }
            }
            else
            {
                return DateTime.MinValue;
            }
        }
        public bool IsValidateEmail(string email)
        {
            if (Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool IsvalidName(string Name)
        {
            if (Regex.IsMatch(Name, @"^[a-zA-Z0-9 ]+$"))
            {
                return true;

            }
            else
            {
                return false;
            }
        }
        public bool IsvalidCTCAndNTH(string Name)
        {
            if (Regex.IsMatch(Name, @"^[a-zA-Z0-9,./? ]+$"))
            {
                return true;

            }
            else
            {
                return false;
            }
        }
        public bool IsLessOrEqualToCurrentDateTime(string Date)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Date) && Date != "")
                {
                    Regex regex = new Regex(@"(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$");
                    Date = string.Format("{0:MM/dd/yyyy}", Date);
                    //Verify whether date entered in dd/MM/yyyy format.
                    bool isValid = regex.IsMatch(Date.Trim());

                    if (isValid)
                    {
                        DateTime InputDT = DateTime.ParseExact(Date, "dd/MM/yyyy", CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);

                        DateTime CompareToDT = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);

                        if (InputDT <= CompareToDT)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    return false;
                }
            }
            catch (FormatException ex)
            {
                return false;
            }

        }
        #endregion
    }
}