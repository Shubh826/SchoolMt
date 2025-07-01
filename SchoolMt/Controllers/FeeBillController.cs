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
        public PartialViewResult GetFeeBillData(int CurrentPage = 1, string SearchBy = "", string SearchValue = "",string ClassName = "", string Section = "")
        {
            objFeeBillBal.GetFeeBillData(out _FeeBillList, out objBasicPagingMDL, 0, SessionInfo.User.fk_companyid, Convert.ToInt32(20), CurrentPage, SearchBy, SearchValue,ClassName,Section);
            
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
                objFeeBillBal.GetFeeBillData(out _FeeBillList, out objBasicPagingMDL, id, SessionInfo.User.fk_companyid, Convert.ToInt32(20)); return View("AddEditFeeBill", _FeeBillList[0]);
            }
            else
            {
                objFeeBillMDL = new FeeBillMDL();

                return View("AddEditFeeBill", objFeeBillMDL);
            }

        }
        [HttpPost]
        public ActionResult AddEditFeeBill(FeeBillMDL objFeeBillMDL)
        {
            objFeeBillMDL.CreatedBy = SessionInfo.User.userid;
            objFeeBillMDL.FK_CompanyId = SessionInfo.User.fk_companyid;
            List<FeeBillMDL> _StudentFeeBillMDL = new List<FeeBillMDL>();
            PaymentDetails _PaymentDetails = new PaymentDetails();
            if (ModelState.IsValid)
            {
                Messages msg = objFeeBillBal.AddEditFeeBill(objFeeBillMDL, out _PaymentDetails);
                TempData["Message"] = msg;
                //return RedirectToAction("Index");
                if(msg.Message_Id == 1)
                {
                    return GeneratePaymentReceiptAfterPayment(_PaymentDetails);
                }
                //else
                //{
                //    return RedirectToAction("Index");
                //}
            }
            return View("AddEditFeeBill");
            //return GeneratePaymentReceiptAfterPayment(_PaymentDetails);


        }
        private ActionResult GeneratePaymentReceiptAfterPayment(PaymentDetails _dataList)
        {
            Messages msg = new Messages();

            try
            {
                // Define the base folder path
                string baseFolderPath = Server.MapPath("~/Public_Doc/FeePaymentReceipt/");

                // Ensure the directory exists
                if (!Directory.Exists(baseFolderPath))
                {
                    Directory.CreateDirectory(baseFolderPath);
                }

                // Define the file path (without creating a directory at file level)
                string paymentReceiptPath = baseFolderPath + "FeePayment_" + _dataList.PK_BillId.ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";

                // Define the HTML template file path
                string templatePath = Server.MapPath("~/App_Data/Payment Receipt.html");
                if (!System.IO.File.Exists(templatePath))
                {
                    throw new FileNotFoundException("Template file not found: " + templatePath);
                }

                // Read the HTML template file (UTF-8 Encoding to prevent missing data issue)
                string htmlContent = System.IO.File.ReadAllText(templatePath, Encoding.UTF8);
                StringBuilder strHtml = new StringBuilder(htmlContent);

                // Replace placeholders with actual values (Check for null values)
                strHtml.Replace("{BillNo}", !string.IsNullOrEmpty(_dataList.BillNo) ? _dataList.BillNo : "");
                strHtml.Replace("{StudentName}", !string.IsNullOrEmpty(_dataList.StudentName) ? _dataList.StudentName : "");
                strHtml.Replace("{PaymentDate}", !string.IsNullOrEmpty(_dataList.PaymentDate) ? _dataList.PaymentDate : "");
                strHtml.Replace("{FatherName}", !string.IsNullOrEmpty(_dataList.FatherName) ? _dataList.FatherName : "");
                strHtml.Replace("{Class}", !string.IsNullOrEmpty(_dataList.ClassName) ? _dataList.ClassName : "");
                //strHtml.Replace("{PrevDue}", _dataList.PreDue != 0 ? _dataList.PreDue.ToString() : "");
                strHtml.Replace("{MonthsName}", !string.IsNullOrEmpty(_dataList.Months) ? _dataList.Months : "");
                strHtml.Replace("{MonthsFee}", _dataList.MonthFee != 0 ? _dataList.MonthFee.ToString() : "");
                strHtml.Replace("{TranFee}", _dataList.TransFee != 0 ? _dataList.TransFee.ToString() : "");
                strHtml.Replace("{ExamFee}", _dataList.ExamFee != 0 ? _dataList.ExamFee.ToString() : "");
                strHtml.Replace("{DueAmount}", _dataList.DueAmount != 0 ? _dataList.DueAmount.ToString() : "");
                strHtml.Replace("{totalAmount}", _dataList.TotalFee != 0 ? _dataList.TotalFee.ToString() : "");
                // Convert HTML to PDF
                _dataList.PdfContent = strHtml.ToString();
                ExportHelper objExportHelper = new ExportHelper();
                byte[] pdfData = objExportHelper.ExportPDF_ByteAarray(_dataList.PdfContent, pageSize: "A4");

                // Save PDF to the server
                System.IO.File.WriteAllBytes(paymentReceiptPath, pdfData);

                // Return file as a downloadable response
                return File(paymentReceiptPath, "application/pdf", "FeePaymentReceipt.pdf");
            }
            catch (Exception ex)
            {
                msg.Message_Id = -1;
                msg.Message = "Error: " + ex.Message;
                return Content(msg.Message);
            }
        }

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