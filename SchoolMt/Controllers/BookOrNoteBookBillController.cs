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
            objBookOrNoteBookBillMDL.PaymentDate = DateTime.Today.ToString("dd-MM-yyyy"); 
            return View(objBookOrNoteBookBillMDL);
        }
        [HttpPost]
        public ActionResult PostBookOrNoteBookBill(BookOrNoteBookBillMDL obj)
        {
            BookPaymentDetails _BookPaymentDetails = new BookPaymentDetails();
            if (ModelState.IsValid)
            {
                try
                {
                    obj.CreatedBy = SessionInfo.User.userid;
                    obj.CompanyId = SessionInfo.User.fk_companyid;
                    Messages msg = objBookOrNoteBookBillBAL.PostBookOrNoteBookBill(obj,out _BookPaymentDetails);

                    if (msg != null)
                    {
                        msg.Message = msg.Message;
                    }
                    if (msg.Message_Id == 1)
                    {
                        return GenerateBookOrNoteBookPaymentReceiptAfterPayment(_BookPaymentDetails);
                    }
                    TempData["Message"] = msg;
                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error saving bill: " + ex.Message);
                }
            }

            // If we got this far, something failed; redisplay form
            return View(obj);
        }

        private ActionResult GenerateBookOrNoteBookPaymentReceiptAfterPayment(BookPaymentDetails _BookPaymentDetails)
        {
            Messages msg = new Messages();
            ViewData["ClassList"] = CommonBAL.FillClass();

            try
            {
                // Define the base folder path
                string baseFolderPath = Server.MapPath("~/Public_Doc/BooKOrNoteBookPaymentReceipt/");

                // Ensure the directory exists
                if (!Directory.Exists(baseFolderPath))
                {
                    Directory.CreateDirectory(baseFolderPath);
                }

                // Define the file path (without creating a directory at file level)
                string paymentReceiptPath = baseFolderPath + "BookFeePayment_" + _BookPaymentDetails.PK_BillId.ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";

                // Define the HTML template file path
                string templatePath = Server.MapPath("~/App_Data/BooKOrNoteBookPaymentReceipt.html");
                if (!System.IO.File.Exists(templatePath))
                {
                    throw new FileNotFoundException("Template file not found: " + templatePath);
                }

                // Read the HTML template file (UTF-8 Encoding to prevent missing data issue)
                string htmlContent = System.IO.File.ReadAllText(templatePath, Encoding.UTF8);
                StringBuilder strHtml = new StringBuilder(htmlContent);

                // Replace placeholders with actual values (Check for null values)
                strHtml.Replace("{BillNo}", _BookPaymentDetails.BillNo ?? "N/A");
                strHtml.Replace("{StudentName}", _BookPaymentDetails.StudentName ?? "N/A");
                strHtml.Replace("{PaymentDate}", _BookPaymentDetails.PaymentDate ?? "N/A");
                strHtml.Replace("{FatherName}", _BookPaymentDetails.FatherName ?? "N/A");
                strHtml.Replace("{Class}", _BookPaymentDetails.ClassName ?? "N/A");
                strHtml.Replace("{TotalAmount}", _BookPaymentDetails.TotalFee != 0 ? _BookPaymentDetails.TotalFee.ToString() : "0");
                strHtml.Replace("{Discount}", _BookPaymentDetails.Discount != 0 ? _BookPaymentDetails.Discount.ToString() : "0");
                strHtml.Replace("{DueAmount}", _BookPaymentDetails.DueAmount != 0 ? _BookPaymentDetails.DueAmount.ToString() : "0");
                strHtml.Replace("{PaidAmount}", _BookPaymentDetails.PaidAmount != 0 ? _BookPaymentDetails.PaidAmount.ToString() : "0");

                // ✅ **Dynamically generate item details table**
                StringBuilder itemDetailsHtml = new StringBuilder();
                if (_BookPaymentDetails.ItemList != null && _BookPaymentDetails.ItemList.Count > 0)
                {
                    foreach (var item in _BookPaymentDetails.ItemList)
                    {
                        itemDetailsHtml.Append("<tr>");
                        itemDetailsHtml.Append($"<td>{item.ItemName}</td>");
                        itemDetailsHtml.Append($"<td>{item.ItemType}</td>");
                        itemDetailsHtml.Append($"<td>{item.PageCount}</td>");
                        itemDetailsHtml.Append($"<td>{item.Quantity}</td>");
                        itemDetailsHtml.Append($"<td>{item.Price}</td>");
                        itemDetailsHtml.Append($"<td>{item.TotalAmount}</td>");
                        itemDetailsHtml.Append("</tr>");
                    }
                }
                else
                {
                    itemDetailsHtml.Append("<tr><td colspan='6' style='text-align:center;'>No Items Found</td></tr>");
                }

                // Replace items table in HTML
                strHtml.Replace("<!--ITEM_DETAILS_PLACEHOLDER-->", itemDetailsHtml.ToString());

                // Convert HTML to PDF
                _BookPaymentDetails.PdfContent = strHtml.ToString();
                ExportHelper objExportHelper = new ExportHelper();
                byte[] pdfData = objExportHelper.ExportPDF_ByteAarray(_BookPaymentDetails.PdfContent, pageSize: "A4");

                // Save PDF to the server
                System.IO.File.WriteAllBytes(paymentReceiptPath, pdfData);

                // Return file as a downloadable response
                return File(paymentReceiptPath, "application/pdf", "BookFeePaymentReceipt.pdf");
            }
            catch (Exception ex)
            {
                msg.Message_Id = -1;
                msg.Message = "Error: " + ex.Message;
                return Content(msg.Message);
            }
        }

        public JsonResult GetStudentNames(string term)
        {
            return Json(CommonBAL.GetStudent(term), JsonRequestBehavior.AllowGet);

        }
        //public JsonResult GetFatherNames(string term)
        //{
        //    return Json(CommonBAL.GetStudent(term), JsonRequestBehavior.AllowGet);

        //}
        [HttpGet]
        public JsonResult GetBooksOrNoteBooks(string Type,string ClassName)
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