using BAL;
using BAL.Common;
using FRGMBSystem.Controllers;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml.html;
using MDL;
using MDL.Common;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using SchoolMt.Common;
using SendGrid.Helpers.Mail.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Windows.Interop;

namespace SchoolMt.Controllers
{
    public class FeeBillController : BasicController
    {
        BasicPagingMDL objBasicPagingMDL = null;
        private FeeBillBAL objFeeBillBal;
        List<FeeBillMDL> _FeeBillList;
        private FeeBillMDL objFeeBillMDL;
        public static string STMDomainPath = ConfigurationManager.AppSettings["STMDomainPath"].ToString();
        public static string SMTDomainUrl = ConfigurationManager.AppSettings["SMTDomainUrl"].ToString();

        public FeeBillController()
        {
            objFeeBillBal = new FeeBillBAL();
            objFeeBillMDL = new FeeBillMDL();

        }
        // GET: FeeBill
        public ActionResult Index()
        {
            ViewData["Classlist"] = CommonBAL.FillClass();
            ViewData["ClassCodelist"] = CommonBAL.FillClassCode();
            if (TempData["Message"] != null)
            {
                ViewBag.Msg = (Messages)TempData["Message"];
                TempData["Message"] = null;
            }
            return View();
        }
        public PartialViewResult GetFeeBillData(int CurrentPage = 1, string SearchBy = "", string SearchValue = "", string ClassName = "", string Section = "")
        {
            objFeeBillBal.GetFeeBillData(out _FeeBillList, out objBasicPagingMDL, 0, SessionInfo.User.fk_companyid, Convert.ToInt32(20), CurrentPage, SearchBy, SearchValue, ClassName, Section);

            ViewBag.paging = objBasicPagingMDL;
            TempData["FeeBillList"] = _FeeBillList;
            return PartialView("_FeeBillGrid", _FeeBillList);
        }
        [HttpGet]
        public ActionResult AddEditFeeBill(int id = 0)
        {
            objFeeBillMDL = new FeeBillMDL();
            if (id != 0)
            {
                objFeeBillBal.GetFeeBillData(out _FeeBillList, out objBasicPagingMDL, id, SessionInfo.User.fk_companyid, Convert.ToInt32(20)); 
                return View("AddEditFeeBill", _FeeBillList[0]);
            }
            else
            {
                objFeeBillMDL = new FeeBillMDL();

                return View("AddEditFeeBill", objFeeBillMDL);
            }

        }
        //[HttpPost]
        //public ActionResult AddEditFeeBill(FeeBillMDL objFeeBillMDL)
        //{
        //    objFeeBillMDL.CreatedBy = SessionInfo.User.userid;
        //    objFeeBillMDL.FK_CompanyId = SessionInfo.User.fk_companyid;
        //    List<FeeBillMDL> _StudentFeeBillMDL = new List<FeeBillMDL>();
        //    PaymentDetails _PaymentDetails = new PaymentDetails();
        //    if (ModelState.IsValid)
        //    {
        //        Messages msg = objFeeBillBal.AddEditFeeBill(objFeeBillMDL, out _PaymentDetails);
        //        TempData["PaymentDetails"] = _PaymentDetails;
        //        TempData["Message"] = msg;
        //        //return RedirectToAction("Index");
        //        if(msg.Message_Id == 1)
        //        {
        //            return GeneratePaymentReceiptAfterPayment(_PaymentDetails);
        //        }
        //        //else
        //        //{
        //        //    return RedirectToAction("Index");
        //        //}
        //    }
        //    return View("AddEditFeeBill");
        //    //return GeneratePaymentReceiptAfterPayment(_PaymentDetails);


        //}

        [HttpPost]
        public JsonResult AddEditFeeBill(FeeBillMDL objFeeBillMDL)
        {
            objFeeBillMDL.CreatedBy = SessionInfo.User.userid;
            objFeeBillMDL.FK_CompanyId = SessionInfo.User.fk_companyid;
            List<FeeBillMDL> _StudentFeeBillMDL = new List<FeeBillMDL>();
            PaymentDetails _PaymentDetails = new PaymentDetails();
            Messages msg = objFeeBillBal.AddEditFeeBill(objFeeBillMDL, out _PaymentDetails);
            TempData["predueamounchecked"] = objFeeBillMDL.predueamounchecked;
            TempData["PaymentDetails"] = _PaymentDetails;
            TempData["Message"] = msg;
            return Json(msg, JsonRequestBehavior.AllowGet);
            //return GeneratePaymentReceiptAfterPayment(_PaymentDetails);


        }

        #region PDF RECEIPT
        [HttpGet]
        public string GeneratePaymentReceiptAfterPayment()
        {
            string htmlForPdf = "";
            try
            {
                TempData.Keep();
                int predueamounchecked = 0;
                PaymentDetails data = TempData["PaymentDetails"] as PaymentDetails;
                if (TempData["predueamounchecked"] != null)
                {
                     predueamounchecked = (int)TempData["predueamounchecked"];
                }
                else
                {
                     predueamounchecked = 0;
                }
                if (data == null)
                    throw new Exception("Payment details not found.");

                StringBuilder html = new StringBuilder();

                html.AppendLine("<!DOCTYPE html>");
                html.AppendLine("<html lang='en'>");
                html.AppendLine("<head>");
                html.AppendLine("    <meta charset='UTF-8' />");
                html.AppendLine("    <meta name='viewport' content='width=device-width, initial-scale=1.0' />");
                html.AppendLine("    <title>Fee Bill</title>");
                html.AppendLine("    <style>");
                html.AppendLine("        body { font-family: Arial, sans-serif; margin: 20px; }");
                html.AppendLine("        .bill-header .school-name { font-size: 18px; text-decoration: underline; }");
                html.AppendLine("        .bill-header .address { font-size: 14px; }");
                html.AppendLine("        .bill-details, .fee-details { width: 100%; margin-top: 20px; border-collapse: collapse; }");
                html.AppendLine("        .fee-details th, .fee-details td { border: 1px solid black; padding: 8px; text-align: left; }");
                html.AppendLine("        .fee-details th { text-align: center; background-color: #bf1e2e; color: white; }");
                html.AppendLine("        .total-amount { color: red; font-weight: bold; text-align: right; }");
                html.AppendLine("        .signature, .date { margin-top: 20px; margin-right: 20px; text-align: left; }");
                html.AppendLine("        .bill-container { width: 100%; height: 100%; display: flex; flex-direction: column; justify-content: center; align-items: center; background: white; }");
                html.AppendLine("        .bill-header { text-align: center; font-weight: bold; }");
                html.AppendLine("        @media print {");
                html.AppendLine("            th {");
                html.AppendLine("                -webkit-print-color-adjust: exact;");
                html.AppendLine("                print-color-adjust: exact;");
                html.AppendLine("            }");
                html.AppendLine("        }");
                html.AppendLine("    </style>");
                html.AppendLine("</head>");
                html.AppendLine("<body>");
                html.AppendLine("    <div class='pdf-container'>");
                html.AppendLine("        <div class='bill-container'>");
                html.AppendLine("            <div class='bill-header'>");
                html.AppendLine($"                <div class='BillNo'>Bill No.: {data.BillNo}</div>");
                html.AppendLine("                <div class='demand'>Fee Bill Receipt</div>");
                html.AppendLine("                <div class='school-name'>True Sunshine Academy</div>");
                html.AppendLine("                <div class='address'>Jahanabad Saifabad, Patti, Pratapgarh (U.P.)</div>");
                html.AppendLine("            </div>");

                html.AppendLine("            <table class='bill-details'>");
                html.AppendLine("                <tr>");
                html.AppendLine($"                    <td><strong>Student Name:</strong> {data.StudentName}</td>");
                html.AppendLine($"                    <td><strong>Date:</strong> {data.PaymentDate}</td>");
                html.AppendLine("                </tr>");
                html.AppendLine("                <tr><td colspan='2' style='height: 10px;'></td></tr>");
                html.AppendLine("                <tr>");
                html.AppendLine($"                    <td><strong>Father Name:</strong> {data.FatherName}</td>");
                html.AppendLine($"                    <td><strong>Class:</strong> {data.ClassName}</td>");
                html.AppendLine("                </tr>");
                html.AppendLine("            </table>");

                html.AppendLine("            <table class='fee-details'>");
                html.AppendLine("                <thead>");
                html.AppendLine("                    <tr>");
                html.AppendLine("                        <th>Details</th>");
                html.AppendLine("                        <th>Amount (₹)</th>");
                html.AppendLine("                    </tr>");
                html.AppendLine("                </thead>");
                html.AppendLine("                <tbody>");
                html.AppendLine($"                    <tr><td>Months</td><td>{data.Months}</td></tr>");
                html.AppendLine($"                    <tr><td>Months Fee</td><td>{data.MonthFee:0.00}</td></tr>");
                html.AppendLine($"                    <tr><td>Months Transport Fee</td><td>{data.TransFee:0.00}</td></tr>");
                html.AppendLine($"                    <tr><td>Exam Fee</td><td>{data.ExamFee:0.00}</td></tr>");
                html.AppendLine($"                    <tr><td>Due Amount</td><td>{data.DueAmount:0.00}</td></tr>");
                html.AppendLine($"                    <tr><td>Previous Due Amount</td><td>{predueamounchecked:0.00}</td></tr>");
                html.AppendLine("                </tbody>");
                html.AppendLine("                <tfoot>");
                html.AppendLine("                    <tr>");
                html.AppendLine("                        <td class='total-amount'>Total Amount</td>");
                html.AppendLine($"                        <td class='total-amount'>{data.TotalFee:0.00}</td>");
                html.AppendLine("                    </tr>");
                html.AppendLine("                </tfoot>");
                html.AppendLine("            </table>");
                html.AppendLine("            <table class='bill-details'>");
                html.AppendLine("                <tr>");
                html.AppendLine($"                    <td><strong>Date :</strong> {data.PaymentDate}</td>");
                html.AppendLine($"                    <td></td>");
                html.AppendLine("                </tr>");
                html.AppendLine("                <tr><td colspan='2' style='height: 10px;'></td></tr>");
                html.AppendLine("                <tr>");
                html.AppendLine($"                    <td><strong>Signature :</strong></td>");
                html.AppendLine($"                    <td></td>");
                html.AppendLine("                </tr>");
                html.AppendLine("            </table>");
                //html.AppendLine($"            <div class='date' style='text-align: left;'><strong>Date :</strong> {data.PaymentDate}</div>");
                //html.AppendLine("            <div class='signature' style='text-align: left;'><strong>Signature :</strong></div>");
                html.AppendLine("        </div>");
                html.AppendLine("    </div>");
                html.AppendLine("</body>");
                html.AppendLine("</html>");

                htmlForPdf = html.ToString();
                data.PdfContent = htmlForPdf;
            }
            catch (Exception ex)
            {
                htmlForPdf = "<div style='color:red'>Error: " + ex.Message + "</div>";
            }

            return htmlForPdf;
        }
        #endregion



        //private ActionResult GeneratePaymentReceiptAfterPayment(PaymentDetails _dataList)
        //{
        //    Messages msg = new Messages();

        //    try
        //    {
        //        // Prepare file paths and URLs
        //        string paymentReceiptPath = Path.Combine(STMDomainPath, "Public_Doc", "FeePaymentReceipt", _dataList.PK_BillId.ToString(), "FeePayment", DateTime.Now.ToString("ddMMyyyy"));
        //        string paymentReceiptUrl = Path.Combine(SMTDomainUrl, "Public_Doc", "FeePaymentReceipt", _dataList.PK_BillId.ToString(), "FeePayment", DateTime.Now.ToString("ddMMyyyy"));
        //        string fileFormat = "\\App_Data\\Payment Receipt.html";
        //        StringBuilder strHtml = new StringBuilder();
        //        var pathn = STMDomainPath + fileFormat;

        //        // Read and append the HTML template
        //        var textRead = System.IO.File.ReadAllText(pathn);
        //        strHtml.Append(System.IO.File.ReadAllText(pathn));

        //        // Add CSS for A6 size
        //        strHtml.Insert(0, "<style>@page { size: A6; margin: 10mm; } body { font-family: Arial, sans-serif; font-size: 12px; }</style>");

        //        // Replace placeholders with actual data
        //        strHtml.Replace("{BillNo}", !string.IsNullOrEmpty(_dataList.BillNo) ? _dataList.BillNo : "");
        //        strHtml.Replace("{StudentName}", !string.IsNullOrEmpty(_dataList.StudentName) ? _dataList.StudentName : "");
        //        strHtml.Replace("{PaymentDate}", !string.IsNullOrEmpty(_dataList.PaymentDate) ? _dataList.PaymentDate : "");
        //        strHtml.Replace("{FatherName}", !string.IsNullOrEmpty(_dataList.FatherName) ? _dataList.FatherName : "");
        //        strHtml.Replace("{Class}", !string.IsNullOrEmpty(_dataList.ClassName) ? _dataList.ClassName : "");
        //        //strHtml.Replace("{PrevDue}", _dataList.PreDue != 0 ? _dataList.PreDue.ToString() : "");
        //        strHtml.Replace("{MonthsName}", !string.IsNullOrEmpty(_dataList.Months) ? _dataList.Months : "");
        //        strHtml.Replace("{MonthsFee}", _dataList.MonthFee != 0 ? _dataList.MonthFee.ToString() : "");
        //        strHtml.Replace("{TranFee}", _dataList.TransFee != 0 ? _dataList.TransFee.ToString() : "");
        //        strHtml.Replace("{ExamFee}", _dataList.ExamFee != 0 ? _dataList.ExamFee.ToString() : "");
        //        strHtml.Replace("{DueAmount}", _dataList.DueAmount != 0 ? _dataList.DueAmount.ToString() : "");
        //        strHtml.Replace("{totalAmount}", _dataList.TotalFee != 0 ? _dataList.TotalFee.ToString() : "");

        //        // Set the PDF content
        //        _dataList.PdfContent = strHtml.ToString();

        //        // Generate the PDF from the HTML content
        //        ExportHelper objExportHelper = new ExportHelper();
        //        byte[] data = objExportHelper.ExportPDF_ByteAarray(_dataList.PdfContent, pageSize: "A6"); // Ensure A6 page size is passed

        //        // Ensure the directory exists
        //        if (!Directory.Exists(paymentReceiptPath))
        //        {
        //            Directory.CreateDirectory(paymentReceiptPath);
        //        }

        //        // Generate the file name and path
        //        string fileName = "FeePaymentReceipt_" + _dataList.PK_BillId + "_" + DateTime.UtcNow.ToString("yyyy_MM_dd_HH_mm_ss_fff") + ".pdf";
        //        string filePath = Path.Combine(paymentReceiptPath, fileName);
        //        string paymentReceiptFilePath = Path.Combine(paymentReceiptUrl, fileName);

        //        // Save the PDF to the specified path
        //        System.IO.File.WriteAllBytes(filePath, data);
        //        string url = paymentReceiptFilePath;
        //        // Set success message
        //        msg.Message_Id = 1;
        //        msg.Message = "Success";
        //        //Process.Start(filePath);
        //        Process.Start(new ProcessStartInfo
        //        {
        //            FileName = url,
        //            UseShellExecute = true
        //        });
        //        // Return success response
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle the exception and log it if necessary
        //        var methodBase = System.Reflection.MethodBase.GetCurrentMethod();
        //        msg.Message_Id = -1;
        //        msg.Message = "Error: " + ex.Message;

        //        // Return failure response
        //        return Content(msg.Message);
        //    }
        //}
    }
}