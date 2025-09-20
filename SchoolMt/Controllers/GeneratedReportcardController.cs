using BAL;
using MDL.Common;
using MDL;
using SchoolMt.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using DocumentFormat.OpenXml.Drawing.Charts;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.IO;
using DocumentFormat.OpenXml.Office2010.Excel;
using iTextSharp.text;
using iTextSharp.tool.xml;

namespace SchoolMt.Controllers
{
    public class GeneratedReportcardController : Controller
    {


        // GET: StudentMaster
        private List<StudentMasterMDL> _Studentlist;
        List<StudentMasterMDL> _stuLst;
        BasicPagingMDL objBasicPagingMDL = null;
        private GeneratedReportcardBAL objBal;
        StoppageMstBAL objStoppageMstBAL = null;
        public GeneratedReportcardController()
        {
            objBal = new GeneratedReportcardBAL();
        }
        // GET: GeneratedReportcard
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult GetStudentData(int CurrentPage = 1, string SearchBy = "", string SearchValue = "")
        {
            objBal.GetStudentData(out _Studentlist, out objBasicPagingMDL, 0, 20, CurrentPage, SessionInfo.User.fk_companyid,SearchBy, SearchValue);
            ViewBag.paging = objBasicPagingMDL;
            TempData["studentlist"] = _Studentlist;
            return PartialView("_StudentGrid", _Studentlist);
        }
        public ActionResult StudentReportCardDetails(int id)
        {
            ReportCardViewMDL obj=new ReportCardViewMDL();
            obj= objBal.GetStudentDataForReportCard(id);
            return View(obj);
        }

        [HttpGet]
        public string PrintReportcard(int id)
        {
            string htmlForPdf = "";
            try
            {
                ReportCardViewMDL obj = new ReportCardViewMDL();
                obj = objBal.GetStudentDataForReportCard(id);
                htmlForPdf = GenerateHtml(obj);
            }
            catch (Exception ex)
            {
                htmlForPdf = "<div style='color:red'>Error: " + ex.Message + "</div>";
            }

            return htmlForPdf;
        }

        [HttpGet]
        public ActionResult DownloadReportCard(int id)
        {
            ReportCardViewMDL obj = objBal.GetStudentDataForReportCard(id);
            if (obj == null)
                return Content("Report card not found");

            string htmlContent = GenerateHtml(obj);

            // Convert relative image URLs to absolute
            string baseUrl = $"{Request.Url.Scheme}://{Request.Url.Authority}";
            htmlContent = htmlContent.Replace("/assets/", baseUrl + "/assets/");

            using (MemoryStream ms = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4, 20f, 20f, 20f, 20f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, ms);
                pdfDoc.Open();

                using (var srHtml = new StringReader(htmlContent))
                {
                    // Correct ParseXHtml usage
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, srHtml);
                }

                pdfDoc.Close();
                byte[] pdfBytes = ms.ToArray();

                return File(pdfBytes, "application/pdf", $"ReportCard_{obj.StudentId}.pdf");
            }
        }

        [NonAction]
        public static string GenerateHtml(ReportCardViewMDL model)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<html>");
            sb.AppendLine("<body style='background-color:#f8f9fa; font-family:Arial,sans-serif; margin:0; padding:0;'>");

            // Wrapper
            sb.AppendLine("<div style='max-width:900px; margin:16px auto; border:1px solid #ddd; padding:18px; border-radius:8px; background:#fff;'>");

            // Hidden student ID
            sb.AppendLine("<div style='display:none;'>" + HttpUtility.HtmlEncode(model.StudentId.ToString()) + "</div>");

            // School Header
            sb.AppendLine("<div style='display:flex; align-items:center; gap:16px;'>");
            sb.AppendLine("<img src='" + HttpUtility.HtmlEncode(model.SchoolLogoUrl ?? "") + "' alt='Logo' style='width:90px; height:90px; object-fit:contain;' />");
            sb.AppendLine("<div style='flex:1;'>");
            sb.AppendLine("<h4 style='margin:0;'>" + HttpUtility.HtmlEncode(model.SchoolName ?? "-") + "</h4>");
            sb.AppendLine("<small style='color:#6c757d;'>" + HttpUtility.HtmlEncode(model.SchoolAddress ?? "-") + "</small>");
            sb.AppendLine("<div><strong>Academic Year:</strong> " + HttpUtility.HtmlEncode(model.AcademicYear ?? "-") + "</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("<div style='text-align:right;'>");
            sb.AppendLine("<h5 style='margin:0;'>Progress Report</h5>");
            sb.AppendLine("<small style='color:#6c757d;'>CBSE</small>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>"); // school-header

            sb.AppendLine("<hr />");

            // Student info
            sb.AppendLine("<div style='display:flex; justify-content:space-between; margin-bottom:16px;'>");
            sb.AppendLine("<div style='flex:2;'>");
            sb.AppendLine("<table style='border-collapse:collapse;'>");
            sb.AppendLine("<tr><td><strong>Student Name</strong></td><td>" + HttpUtility.HtmlEncode(model.StudentName ?? "-") + "</td></tr>");
            sb.AppendLine("<tr><td><strong>Admission No.</strong></td><td>" + HttpUtility.HtmlEncode(model.AdmissionNo ?? "-") + "</td></tr>");
            sb.AppendLine("<tr><td><strong>Class</strong></td><td>" + HttpUtility.HtmlEncode(model.Class ?? "-") + "</td></tr>");
            sb.AppendLine("<tr><td><strong>Section</strong></td><td>" + HttpUtility.HtmlEncode(model.Section ?? "-") + "</td></tr>");
            sb.AppendLine("<tr><td><strong>Roll No.</strong></td><td>" + HttpUtility.HtmlEncode(model.RollNo ?? "-") + "</td></tr>");
            sb.AppendLine("</table>");
            sb.AppendLine("</div>");
            sb.AppendLine("<div style='flex:1; text-align:right;'>");
            sb.AppendLine("<div><strong>DOB:</strong> " + HttpUtility.HtmlEncode(model.DOB ?? "-") + "</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");

            // Subjects Table
            sb.AppendLine("<div style='overflow-x:auto;'>");
            sb.AppendLine("<table style='width:100%; border-collapse:collapse; border:1px solid #ddd;'>");

            // Table Header
            sb.AppendLine("<thead>");
            sb.AppendLine("<tr style='background-color:#d9edf7; color:#000; -webkit-print-color-adjust: exact;'>");
            sb.AppendLine("<th style='border:1px solid #ddd; padding:8px; text-align:center;'>Subject</th>");
            sb.AppendLine("<th style='border:1px solid #ddd; padding:8px; text-align:center;'><div>Periodic Test</div><div>(20)</div></th>");
            sb.AppendLine("<th style='border:1px solid #ddd; padding:8px; text-align:center;'><div>Notebook</div><div>(5)</div></th>");
            sb.AppendLine("<th style='border:1px solid #ddd; padding:8px; text-align:center;'><div>Subject Enrichment</div><div>(5)</div></th>");
            sb.AppendLine("<th style='border:1px solid #ddd; padding:8px; text-align:center;'><div>Annual Exam</div><div>(70)</div></th>");
            sb.AppendLine("<th style='border:1px solid #ddd; padding:8px; text-align:center;'><div>Total</div><div>(100)</div></th>");
            sb.AppendLine("<th style='border:1px solid #ddd; padding:8px; text-align:center;'>Grade</th>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</thead>");

            // Table Body
            sb.AppendLine("<tbody>");
            bool isAlternate = false;
            foreach (var s in model.SubjectGrades)
            {
                // Alternate row background
                string rowBg = isAlternate ? "#f9f9f9" : "#ffffff";
                sb.AppendLine("<tr style='background-color:" + rowBg + "; -webkit-print-color-adjust: exact;'>");
                sb.AppendLine("<td style='border:1px solid #ddd; padding:8px; text-align:left;'>" + HttpUtility.HtmlEncode(s.SubjectName ?? "-") + "</td>");
                sb.AppendLine("<td style='border:1px solid #ddd; padding:8px; text-align:center;'>" + (s.PeriodicTest.HasValue ? s.PeriodicTest.Value.ToString() : "-") + "</td>");
                sb.AppendLine("<td style='border:1px solid #ddd; padding:8px; text-align:center;'>" + (s.Notebook.HasValue ? s.Notebook.Value.ToString() : "-") + "</td>");
                sb.AppendLine("<td style='border:1px solid #ddd; padding:8px; text-align:center;'>" + (s.SubjectEnrichment.HasValue ? s.SubjectEnrichment.Value.ToString() : "-") + "</td>");
                sb.AppendLine("<td style='border:1px solid #ddd; padding:8px; text-align:center;'>" + (s.AnnualExam.HasValue ? s.AnnualExam.Value.ToString() : "-") + "</td>");
                sb.AppendLine("<td style='border:1px solid #ddd; padding:8px; text-align:center;'>" + s.Total + "</td>");
                sb.AppendLine("<td style='border:1px solid #ddd; padding:8px; text-align:center;'>" + HttpUtility.HtmlEncode(s.Grade ?? "-") + "</td>");
                sb.AppendLine("</tr>");
                isAlternate = !isAlternate;
            }
            sb.AppendLine("</tbody>");

            // Table Footer
            sb.AppendLine("<tfoot>");
            sb.AppendLine("<tr style='background-color:#d9edf7; color:#000; font-weight:bold; -webkit-print-color-adjust: exact;'>");
            sb.AppendLine("<td colspan='5' style='border:1px solid #ddd; padding:8px; text-align:right;'>Grand Total</td>");
            sb.AppendLine("<td style='border:1px solid #ddd; padding:8px; text-align:center;'>" + model.TotalMarks + " / " + model.MaxMarks + "</td>");
            sb.AppendLine("<td style='border:1px solid #ddd; padding:8px; text-align:center;'>" + string.Format("{0:0.00}%", model.Percentage) + "</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</tfoot>");

            sb.AppendLine("</table>");
            sb.AppendLine("</div>");

            // Remarks & Signatures
            sb.AppendLine("<div style='display:flex; margin-top:16px;'>");
            sb.AppendLine("<div style='flex:2;'>");
            sb.AppendLine("<p><strong>Remark:</strong> " + HttpUtility.HtmlEncode(model.ResultRemark ?? "-") + "</p>");
            sb.AppendLine("<p><small>Note: This is a computer-generated report card based on recorded marks.</small></p>");
            sb.AppendLine("</div>");
            sb.AppendLine("<div style='flex:1; text-align:center;'>");
            sb.AppendLine("<div style='height:60px;'>&nbsp;</div>");
            sb.AppendLine("<div><strong>" + HttpUtility.HtmlEncode(model.ClassTeacherName ?? "-") + "</strong><br/><small>Class Teacher</small></div>");
            sb.AppendLine("<div style='margin-top:12px;'><strong>" + HttpUtility.HtmlEncode(model.PrincipalName ?? "-") + "</strong><br/><small>Principal</small></div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");

            sb.AppendLine("</div>"); // wrapper
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }

    }

}

 