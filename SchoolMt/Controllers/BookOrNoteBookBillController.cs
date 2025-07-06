using BAL;
using BAL.Common;
using MDL.Common;
using MDL;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Web.Configuration;
using SchoolMt.Common;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Configuration;
using System.Security.Policy;
using iTextSharp.tool.xml.html;

namespace SchoolMt.Controllers
{
    public class BookOrNoteBookBillController : Controller
    {
        // GET: BookOrNoteBookBill
        BasicPagingMDL objBasicPagingMDL = null;
        BookOrNoteBookBillMDL objBookOrNoteBookBillMDL = new BookOrNoteBookBillMDL();
        BookOrNoteBookBillBAL objBookOrNoteBookBillBAL = new BookOrNoteBookBillBAL();
        public static string STMDomainPath = ConfigurationManager.AppSettings["STMDomainPath"].ToString();
        public static string SMTDomainUrl = ConfigurationManager.AppSettings["SMTDomainUrl"].ToString();
        public BookOrNoteBookBillController()
        {
            objBookOrNoteBookBillBAL = new BookOrNoteBookBillBAL();
        }
        public ActionResult Index()
        {
            ViewData["ClassList"] = CommonBAL.FillClass();
            List<DropDownMDL> _StudentList = new List<DropDownMDL>();
            TempData["StudentList"] = _StudentList = CommonBAL.GetStudentByClassName("");
            // objBookOrNoteBookBillMDL.PaymentDate = DateTime.Today.ToString("dd-MM-yyyy");
             objBookOrNoteBookBillMDL.PaymentDate = "";
             objBookOrNoteBookBillMDL.HdnPaymentDate = "";
            return View(objBookOrNoteBookBillMDL);
        }
        [HttpPost]
        public JsonResult PostBookOrNoteBookBill(BookOrNoteBookBillMDL obj)
        {
            BookPaymentDetails _BookPaymentDetails = new BookPaymentDetails();
            
            if(!string.IsNullOrEmpty(obj.HdnPaymentDate))
            {
                obj.PaymentDate = obj.HdnPaymentDate;
            }
              
                    obj.CreatedBy = SessionInfo.User.userid;
                    obj.CompanyId = SessionInfo.User.fk_companyid;
                    Messages msg = objBookOrNoteBookBillBAL.PostBookOrNoteBookBill(obj, out _BookPaymentDetails);
                    TempData["Message"] = msg;
                    TempData["BookPaymentDetails"] = _BookPaymentDetails;
                    return Json(msg, JsonRequestBehavior.AllowGet);
        }
        #region PDF RECEIPT

        [HttpGet]
        public string GenerateBookOrNoteBookPaymentReceiptAfterPayment()
        {
            string htmlForPdf = "";
            try
            {
                TempData.Keep();
                BookPaymentDetails data = TempData["BookPaymentDetails"] as BookPaymentDetails;
                if (data == null)
                    throw new Exception("Payment details not found.");

                StringBuilder strHtml = new StringBuilder();

                // Start HTML
                strHtml.AppendLine("<!DOCTYPE html>");
                strHtml.AppendLine("<html lang='en'>");
                strHtml.AppendLine("<head>");
                strHtml.AppendLine("    <meta charset='UTF-8' />");
                strHtml.AppendLine("    <meta name='viewport' content='width=device-width, initial-scale=1.0' />");
                strHtml.AppendLine("    <title>Book/Notebook Bill</title>");
                strHtml.AppendLine("    <style>");
                strHtml.AppendLine("        html, body { height: 100%; margin: 0; display: flex; justify-content: center; align-items: center; font-family: Arial, sans-serif; background-color: #f8f8f8; }");
                strHtml.AppendLine("        .pdf-container { width: 100%; display: flex; justify-content: center; align-items: center; }");
                strHtml.AppendLine("        .bill-header { text-align: center; font-weight: bold; }");
                strHtml.AppendLine("        .bill-header .school-name { font-size: 18px; text-decoration: underline; }");
                strHtml.AppendLine("        .bill-header .address { font-size: 14px; }");
                strHtml.AppendLine("        .bill-details, .item-details { width: 100%; margin-top: 20px; border-collapse: collapse; }");
                strHtml.AppendLine("        .item-details th, .item-details td { border: 1px solid black; padding: 8px; text-align: left; }");
                strHtml.AppendLine("        .item-details th { text-align: center; background-color: #bf1e2e; color: white; }");
                strHtml.AppendLine("        .total-amount { color: red; font-weight: bold; text-align: right; }");
                strHtml.AppendLine("        .signature, .date { margin-top: 20px; text-align: left; }");
                strHtml.AppendLine("        .bill-details tr:nth-child(2) { height: 15px; }");
                strHtml.AppendLine("        @media print {");
                strHtml.AppendLine("            th {");
                strHtml.AppendLine("                -webkit-print-color-adjust: exact;");
                strHtml.AppendLine("                print-color-adjust: exact;");
                strHtml.AppendLine("            }");
                strHtml.AppendLine("        }");
                strHtml.AppendLine("    </style>");
                strHtml.AppendLine("</head>");
                strHtml.AppendLine("<body>");
                strHtml.AppendLine("    <div class='pdf-container'>");
                strHtml.AppendLine("        <div class='bill-container'>");

                // Header
                strHtml.AppendLine("            <div class='bill-header'>");
                strHtml.AppendLine($"                <div class='BillNo'>Bill No.: {data.BillNo ?? "N/A"}</div>");
                strHtml.AppendLine("                <div class='demand'>Book Or NoteBook Bill Receipt</div>");
                strHtml.AppendLine("                <div class='school-name'>True Sunshine Academy</div>");
                strHtml.AppendLine("                <div class='address'>Jahanabad Saifabad, Patti, Pratapgarh (U.P.)</div>");
                strHtml.AppendLine("            </div>");

                // Student Info
                strHtml.AppendLine("            <table class='bill-details'>");
                strHtml.AppendLine("                <tr>");
                strHtml.AppendLine($"                    <td><strong>Student Name:</strong> {data.StudentName ?? "N/A"}</td>");
                strHtml.AppendLine($"                    <td><strong>Date:</strong> {data.PaymentDate ?? "N/A"}</td>");
                strHtml.AppendLine("                </tr>");
                strHtml.AppendLine("                <tr><td colspan='2' style='height: 10px;'></td></tr>");
                strHtml.AppendLine("                <tr>");
                strHtml.AppendLine($"                    <td><strong>Father Name:</strong> {data.FatherName ?? "N/A"}</td>");
                strHtml.AppendLine($"                    <td><strong>Class:</strong> {data.ClassName ?? "N/A"}</td>");
                strHtml.AppendLine("                </tr>");
                strHtml.AppendLine("            </table>");

                // Item Table
                strHtml.AppendLine("            <table class='item-details'>");
                strHtml.AppendLine("                <thead>");
                strHtml.AppendLine("                    <tr>");
                strHtml.AppendLine("                        <th>Item Name</th>");
                strHtml.AppendLine("                        <th>Type</th>");
                strHtml.AppendLine("                        <th>Pages</th>");
                strHtml.AppendLine("                        <th>Quantity</th>");
                strHtml.AppendLine("                        <th>Price (₹)</th>");
                strHtml.AppendLine("                        <th>Total (₹)</th>");
                strHtml.AppendLine("                    </tr>");
                strHtml.AppendLine("                </thead>");
                strHtml.AppendLine("                <tbody>");

                if (data.ItemList != null && data.ItemList.Count > 0)
                {
                    foreach (var item in data.ItemList)
                    {
                        strHtml.AppendLine("                <tr>");
                        strHtml.AppendLine($"                    <td>{item.ItemName}</td>");
                        strHtml.AppendLine($"                    <td>{item.ItemType}</td>");
                        strHtml.AppendLine($"                    <td>{item.PageCount}</td>");
                        strHtml.AppendLine($"                    <td>{item.Quantity}</td>");
                        strHtml.AppendLine($"                    <td>{item.Price:0.00}</td>");
                        strHtml.AppendLine($"                    <td>{item.TotalAmount:0.00}</td>");
                        strHtml.AppendLine("                </tr>");
                    }
                }
                else
                {
                    strHtml.AppendLine("<tr><td colspan='6' style='text-align:center;'>No Items Found</td></tr>");
                }

                strHtml.AppendLine("                </tbody>");
                strHtml.AppendLine("                <tfoot>");
                strHtml.AppendLine($"                    <tr><td colspan='5' class='total-amount'>Total Amount</td><td class='total-amount'>{(data.TotalFee != 0 ? data.TotalFee.ToString("0.00") : "0.00")}</td></tr>");
                strHtml.AppendLine($"                    <tr><td colspan='5' class='total-amount'>Discount</td><td class='total-amount'>{(data.Discount != 0 ? data.Discount.ToString("0.00") : "0.00")}</td></tr>");
                strHtml.AppendLine($"                    <tr><td colspan='5' class='total-amount'>Due Amount</td><td class='total-amount'>{(data.DueAmount != 0 ? data.DueAmount.ToString("0.00") : "0.00")}</td></tr>");
                strHtml.AppendLine($"                    <tr><td colspan='5' class='total-amount'>Paid Amount</td><td class='total-amount'>{(data.PaidAmount != 0 ? data.PaidAmount.ToString("0.00") : "0.00")}</td></tr>");
                strHtml.AppendLine("                </tfoot>");
                strHtml.AppendLine("            </table>");

                // Footer
                strHtml.AppendLine($"            <div class='date'><strong>Date :</strong> {data.PaymentDate ?? "N/A"}</div>");
                strHtml.AppendLine("            <div class='signature'><strong>Signature :</strong></div>");

                strHtml.AppendLine("        </div>");
                strHtml.AppendLine("    </div>");
                strHtml.AppendLine("</body>");
                strHtml.AppendLine("</html>");

                htmlForPdf = strHtml.ToString();
                data.PdfContent = htmlForPdf;
            }
            catch (Exception ex)
            {
                htmlForPdf = $"<div style='color:red'>Error: {ex.Message}</div>";
            }

            return htmlForPdf;
        }

        #endregion
        //[HttpPost]
        //public ActionResult PostBookOrNoteBookBill(BookOrNoteBookBillMDL obj)
        //{
        //    BookPaymentDetails _BookPaymentDetails = new BookPaymentDetails();
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            obj.CreatedBy = SessionInfo.User.userid;
        //            obj.CompanyId = SessionInfo.User.fk_companyid;
        //            Messages msg = objBookOrNoteBookBillBAL.PostBookOrNoteBookBill(obj, out _BookPaymentDetails);

        //            if (msg != null)
        //            {
        //                msg.Message = msg.Message;
        //            }
        //            if (msg.Message_Id == 1)
        //            {
        //                return GenerateBookOrNoteBookPaymentReceiptAfterPayment(_BookPaymentDetails);
        //            }
        //            TempData["Message"] = msg;
        //            return RedirectToAction("Index");

        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError("", "Error saving bill: " + ex.Message);
        //        }
        //    }

        //    // If we got this far, something failed; redisplay form
        //    return View(obj);
        //}

        //private ActionResult GenerateBookOrNoteBookPaymentReceiptAfterPayment(BookPaymentDetails _BookPaymentDetails)
        //{
        //    Messages msg = new Messages();
        //    ViewData["ClassList"] = CommonBAL.FillClass();

        //    try
        //    {
        //        // Define the base folder path
        //        string baseFolderPath = Server.MapPath("~/Public_Doc/BooKOrNoteBookPaymentReceipt/");

        //        // Ensure the directory exists
        //        if (!Directory.Exists(baseFolderPath))
        //        {
        //            Directory.CreateDirectory(baseFolderPath);
        //        }

        //        // Define the file path (without creating a directory at file level)
        //        string paymentReceiptPath = baseFolderPath + "BookFeePayment_" + _BookPaymentDetails.PK_BillId.ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";

        //        // Define the HTML template file path
        //        string templatePath = Server.MapPath("~/App_Data/BooKOrNoteBookPaymentReceipt.html");
        //        if (!System.IO.File.Exists(templatePath))
        //        {
        //            throw new FileNotFoundException("Template file not found: " + templatePath);
        //        }

        //        // Read the HTML template file (UTF-8 Encoding to prevent missing data issue)
        //        string htmlContent = System.IO.File.ReadAllText(templatePath, Encoding.UTF8);
        //        StringBuilder strHtml = new StringBuilder(htmlContent);

        //        // Replace placeholders with actual values (Check for null values)
        //        strHtml.Replace("{BillNo}", _BookPaymentDetails.BillNo ?? "N/A");
        //        strHtml.Replace("{StudentName}", _BookPaymentDetails.StudentName ?? "N/A");
        //        strHtml.Replace("{PaymentDate}", _BookPaymentDetails.PaymentDate ?? "N/A");
        //        strHtml.Replace("{FatherName}", _BookPaymentDetails.FatherName ?? "N/A");
        //        strHtml.Replace("{Class}", _BookPaymentDetails.ClassName ?? "N/A");
        //        strHtml.Replace("{TotalAmount}", _BookPaymentDetails.TotalFee != 0 ? _BookPaymentDetails.TotalFee.ToString() : "0");
        //        strHtml.Replace("{Discount}", _BookPaymentDetails.Discount != 0 ? _BookPaymentDetails.Discount.ToString() : "0");
        //        strHtml.Replace("{DueAmount}", _BookPaymentDetails.DueAmount != 0 ? _BookPaymentDetails.DueAmount.ToString() : "0");
        //        strHtml.Replace("{PaidAmount}", _BookPaymentDetails.PaidAmount != 0 ? _BookPaymentDetails.PaidAmount.ToString() : "0");

        //        // ✅ **Dynamically generate item details table**
        //        StringBuilder itemDetailsHtml = new StringBuilder();
        //        if (_BookPaymentDetails.ItemList != null && _BookPaymentDetails.ItemList.Count > 0)
        //        {
        //            foreach (var item in _BookPaymentDetails.ItemList)
        //            {
        //                itemDetailsHtml.Append("<tr>");
        //                itemDetailsHtml.Append($"<td>{item.ItemName}</td>");
        //                itemDetailsHtml.Append($"<td>{item.ItemType}</td>");
        //                itemDetailsHtml.Append($"<td>{item.PageCount}</td>");
        //                itemDetailsHtml.Append($"<td>{item.Quantity}</td>");
        //                itemDetailsHtml.Append($"<td>{item.Price}</td>");
        //                itemDetailsHtml.Append($"<td>{item.TotalAmount}</td>");
        //                itemDetailsHtml.Append("</tr>");
        //            }
        //        }
        //        else
        //        {
        //            itemDetailsHtml.Append("<tr><td colspan='6' style='text-align:center;'>No Items Found</td></tr>");
        //        }

        //        // Replace items table in HTML
        //        strHtml.Replace("<!--ITEM_DETAILS_PLACEHOLDER-->", itemDetailsHtml.ToString());

        //        // Convert HTML to PDF
        //        _BookPaymentDetails.PdfContent = strHtml.ToString();
        //        ExportHelper objExportHelper = new ExportHelper();
        //        byte[] pdfData = objExportHelper.ExportPDF_ByteAarray(_BookPaymentDetails.PdfContent, pageSize: "A4");

        //        // Save PDF to the server
        //        System.IO.File.WriteAllBytes(paymentReceiptPath, pdfData);

        //        // Return file as a downloadable response
        //        return File(paymentReceiptPath, "application/pdf", "BookFeePaymentReceipt.pdf");
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Message_Id = -1;
        //        msg.Message = "Error: " + ex.Message;
        //        return Content(msg.Message);
        //    }
        //}

        public JsonResult GetStudentNameByClassName(string className = "")
        {
           // className = "";
            TempData.Keep();
            List<DropDownMDL> _StudentList = new List<DropDownMDL>();
            _StudentList = CommonBAL.GetStudentByClassName(className);
            TempData["StudentList"] = _StudentList;
            return Json(1, JsonRequestBehavior.AllowGet);
            //return Json(CommonBAL.GetStudent(term), JsonRequestBehavior.AllowGet);

        }



        public JsonResult GetStudentNames(string term = "")
        {
            TempData.Keep();
            List<DropDownMDL> _StudentList = new List<DropDownMDL>();
            List<DropDownMDL> _NewStudentList = new List<DropDownMDL>();
            _StudentList = (List<DropDownMDL>)TempData["StudentList"];
            if (!string.IsNullOrEmpty(term))
            {
                _NewStudentList = _StudentList
     .Where(x => !string.IsNullOrEmpty(x.Value) &&
                 x.Value.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
     .ToList();
            }
            else
            {
                _NewStudentList = _StudentList;
            }
            return Json(_NewStudentList, JsonRequestBehavior.AllowGet);
            //return Json(CommonBAL.GetStudent(term), JsonRequestBehavior.AllowGet);

        }
        //public JsonResult GetFatherNames(string term)
        //{
        //    return Json(CommonBAL.GetStudent(term), JsonRequestBehavior.AllowGet);

        //}
        [HttpGet]
        public JsonResult GetBooksOrNoteBooks(string Type, string ClassName)
        {
            try
            {
                List<BookOrNotebookDetail> _BookOrNoteBookData = new List<BookOrNotebookDetail>();
                _BookOrNoteBookData = objBookOrNoteBookBillBAL.GetBooksOrNoteBooks(Type, ClassName);
                return Json(_BookOrNoteBookData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}