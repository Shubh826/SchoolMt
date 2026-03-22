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
using SendGrid.Helpers.Mail.Model;
using System.Net.Http;
using BAL.Common;
using Microsoft.Ajax.Utilities;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

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
        private SubjectMarkMappingBAL objSubjectMarkMappingBAL;
        public GeneratedReportcardController()
        {
            objBal = new GeneratedReportcardBAL();
            objSubjectMarkMappingBAL = new SubjectMarkMappingBAL();
        }
        // GET: GeneratedReportcard
        public ActionResult Index()
        {
            ViewData["Classlist"] = CommonBAL.FillClass();
            ViewData["ClassCodelist"] = CommonBAL.FillClassCode();
            return View();
        }
        public PartialViewResult GetStudentData(int CurrentPage = 1, string SearchBy = "", string SearchValue = "", string ClassName = "", string Section = "")
        {
            objBal.GetStudentData(out _Studentlist, out objBasicPagingMDL, 0, 20, CurrentPage, SessionInfo.User.fk_companyid, SearchBy, SearchValue, ClassName, Section);
            ViewBag.paging = objBasicPagingMDL;
            TempData["studentlist"] = _Studentlist;
            return PartialView("_StudentGrid", _Studentlist);
        }
        public ActionResult StudentReportCardDetails(int id)
        {
            MarksTableViewModel obj = new MarksTableViewModel();
            obj = objSubjectMarkMappingBAL.StudentReportCardDetails_New(id);
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

        public ActionResult GenerateAllStudentsHtml()
        {
            try
            {
                TempData.Keep();
                // ✅ Safe TempData handling
                if (TempData["studentlist"] == null)
                {
                    return Content("Session expired. Please search again.");
                }

                List<StudentMasterMDL> _list = TempData["studentlist"] as List<StudentMasterMDL>;
                TempData.Keep("studentlist"); // keep for reuse

                if (_list == null || _list.Count == 0)
                {
                    return Content("Report card not found");
                }

                // ✅ Unique file name
                string ReportName = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                var sb = new StringBuilder();

                // ✅ Global CSS
                sb.Append(@"
        <style>
        body { font-family: Helvetica,Arial,sans-serif; font-size:8pt; }
        .borLeft { border-left:1px solid #000; }
        .borRight { border-right:1px solid #000; }
        .borTop { border-top:1px solid #000; }
        .borBottom { border-bottom:1px solid #000; }
        table { border-collapse: collapse; width:100%; }
        td, th { padding:5px; }

        .page-break {
            page-break-after: always;
        }
        </style>
        ");

                // ✅ Loop students
                foreach (var item in _list)
                {
                    var obj = objSubjectMarkMappingBAL
                              .StudentReportCardDetails_New(Convert.ToInt32(item.PK_SudentId));

                    sb.Append(GenerateSingleStudentHtml(obj));

                    // Page break after each student
                    sb.Append("<div class='page-break'></div>");
                }

                string htmlContent = sb.ToString(); // ✅ moved outside loop

                // ✅ PDF generation
                using (MemoryStream ms = new MemoryStream())
                {
                    Document pdfDoc = new Document(PageSize.A4, 20f, 20f, 20f, 20f);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, ms);
                    pdfDoc.Open();

                    using (var srHtml = new StringReader(htmlContent))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, srHtml);
                    }

                    pdfDoc.Close();

                    byte[] pdfBytes = ms.ToArray();

                    return File(pdfBytes, "application/pdf", $"ReportCard_{ReportName}.pdf");
                }
            }
            catch (Exception ex)
            {
                // Optional: log error
                return Content("Error generating report card");
            }
        }
       
        public string GenerateSingleStudentHtml(MarksTableViewModel Model)
        {
            var html = new StringBuilder();

           
            // Start Table
            html.Append(@"<table width='100%' border='0' cellspacing='0' cellpadding='0'>
        <tbody>
            <tr>
                <td colspan='4' align='center' style='font-size:12pt;'><strong>REPORT CARD " + Model.Student.AcademicYear + @"</strong></td>
            </tr>
            <tr><td colspan='4'>&nbsp;</td></tr>
            <tr>
                <td class='borLeft borTop' colspan='2'>
                    <table width='100%' cellspacing='0' cellpadding='3'>
                        <tr><td>Student's Name: <strong>" + Model.Student.StudentName + @"</strong></td></tr>
                        <tr><td>Father's Name: <strong>" + (Model.Student.FatherName ?? "-") + @"</strong></td></tr>
                        <tr><td>Mother's Name: <strong>" + (Model.Student.MotherName ?? "-") + @"</strong></td></tr>
                        <tr><td>Date Of Birth: <strong>" + (Model.Student.DOB ?? "-") + @"</strong></td></tr>
                        <tr>
                            <td>
                                <table width='70%' cellspacing='0' cellpadding='0'>
                                    <tr>
                                        <td width='45%'>Class: <strong>" + (Model.Student.Class ?? "-") + @"</strong></td>
                                        <td width='25%'>Sec: <strong>" + (Model.Student.Section ?? "-") + @"</strong></td>
                                        <td width='30%'>Roll No: <strong>" + (Model.Student.RollNo ?? "-") + @"</strong></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td align='right' class='borRight borTop' colspan='2'>
                    <img src='" + Model.Student.StudentImgUrl + @"' alt='Student Photo' class='borBottom borLeft borRight borTop' width='80'>
                </td>
            </tr>
            <tr><td colspan='4' class='borTop borLeft borRight'>&nbsp;</td></tr>
            <tr>
                <td colspan='4' class='borTop borLeft borRight'>
                    <table width='100%' cellspacing='0' cellpadding='5'>
                        <thead>
                            <tr>
                                <th class='borBottom borRight'>Scholastic Areas:</th>");
            // Exam Categories Header
            foreach (var cat in Model.ExamCategories)
            {
                html.Append("<th class='borBottom borRight' colspan='6'>" + cat.ExamCategoryType + " (100 marks)</th>");
            }
            html.Append("<th class='borBottom' colspan='2'>OVERALL</th></tr>");

            // Exam Types Header
            html.Append("<tr><th class='borBottom borRight' rowspan='2'>Subject Name</th>");
            foreach (var cat in Model.ExamCategories)
            {
                var examTypes = Model.ExamTypes.AsEnumerable();
                if (cat.ExamCategoryType.Contains("Term-1") || cat.ExamCategoryType.Contains("Term 1")) examTypes = examTypes.Where(e => e.ExamType != "Y");
                if (cat.ExamCategoryType.Contains("Term-2") || cat.ExamCategoryType.Contains("Term 2")) examTypes = examTypes.Where(e => e.ExamType != "HY");

                foreach (var examType in examTypes)
                {
                    html.Append("<th class='borBottom borRight'>" + examType.ExamType + "</th>");
                }
                html.Append("<th class='borBottom borRight'>TOTAL</th><th rowspan='2' class='borBottom borRight'>Grade</th>");
            }
            html.Append("<th class='borBottom borRight'>GRAND TOTAL</th><th rowspan='2' class='borBottom'>Grade</th></tr>");

            // Sub-header for marks
            html.Append("<tr>");
            foreach (var cat in Model.ExamCategories)
            {
                var examTypes = Model.ExamTypes.AsEnumerable();
                if (cat.ExamCategoryType.Contains("Term-1") || cat.ExamCategoryType.Contains("Term 1")) examTypes = examTypes.Where(e => e.ExamType != "Y");
                if (cat.ExamCategoryType.Contains("Term-2") || cat.ExamCategoryType.Contains("Term 2")) examTypes = examTypes.Where(e => e.ExamType != "HY");
                foreach (var examType in examTypes) html.Append("<th class='borBottom borRight'>(in " + examType.TotalMark + ")</th>");
                html.Append("<th class='borBottom borRight'>(in 100)</th>");
            }
            html.Append("<th class='borBottom borRight'>T1(50)+T2(50)</th></tr>");
            html.Append("</thead><tbody>");

            // Subject Rows
            foreach (var subject in Model.Subjects)
            {
                html.Append("<tr><td class='borBottom borRight'><strong>" + subject.SubjectName + "</strong></td>");
                double term1Total = 0, term2Total = 0;
                foreach (var cat in Model.ExamCategories)
                {
                    int termTotal = 0;
                    var examTypes = Model.ExamTypes.AsEnumerable();
                    if (cat.ExamCategoryType.Contains("Term-1") || cat.ExamCategoryType.Contains("1")) examTypes = examTypes.Where(e => e.ExamType != "Y");
                    if (cat.ExamCategoryType.Contains("Term-2") || cat.ExamCategoryType.Contains("2")) examTypes = examTypes.Where(e => e.ExamType != "HY");

                    foreach (var examType in examTypes)
                    {
                        var mark = Model.Marks.FirstOrDefault(m => m.SubjectId == subject.Id && m.ExamCategoryId == cat.Id && m.ExamTypeId == examType.Id);
                        if (mark != null) { html.Append("<td class='borBottom borRight'>" + mark.ObtainMarks + "</td>"); termTotal += mark.ObtainMarks; }
                        else html.Append("<td class='borBottom borRight'>-</td>");
                    }

                    termTotal = termTotal > 100 ? 100 : termTotal;
                    html.Append("<td class='borBottom borRight'>" + termTotal + "</td>");
                    double termPercentage = (termTotal * 100.0) / 100;
                    var grade = Model.Grades.FirstOrDefault(g => termPercentage >= g.Min && termPercentage <= g.Max);
                    html.Append("<td class='borBottom borRight'>" + (grade?.GradeName ?? "-") + "</td>");

                    if (cat.ExamCategoryType.Contains("Term-1") || cat.ExamCategoryType.Contains("1")) term1Total = termTotal;
                    if (cat.ExamCategoryType.Contains("Term-2") || cat.ExamCategoryType.Contains("2")) term2Total = termTotal;
                }

                double overallTotal = (term1Total * 0.5) + (term2Total * 0.5);
                var overallGrade = Model.Grades.FirstOrDefault(g => overallTotal >= g.Min && overallTotal <= g.Max);
                html.Append("<td class='borBottom borRight'>" + overallTotal.ToString("F0") + "</td>");
                html.Append("<td class='borBottom'>" + (overallGrade?.GradeName ?? "-") + "</td>");
                html.Append("</tr>");
            }

            html.Append("</tbody></table></td></tr>");

            // 8 Point Grading Scale
            html.Append(@"
    <tr>
        <td colspan='4' class='borTop borLeft borRight'>
            <table width='100%' cellspacing='5' cellpadding='0'>
                <tr>
                    <td width='25%'><strong>8 Point Grading Scale:</strong></td>
                    <td width='75%'><strong>");
            var gradesList = Model.Grades.OrderByDescending(g => g.Min).ToList();
            for (int i = 0; i < gradesList.Count; i++)
            {
                var g = gradesList[i];
                html.Append(g.GradeName + " (" + g.Min + "% - " + g.Max + "%)");
                if (i < gradesList.Count - 1) html.Append(" ");
            }
            html.Append(@"</strong></td></tr></table></td></tr>");

            // Abbreviations
            html.Append(@"
    <tr>
        <td colspan='4' class='borTop borLeft borRight'>
            <table width='100%' cellspacing='5' cellpadding='0'>
                <tr>
                    <td width='25%'><strong>Abbreviations:</strong></td>
                    <td width='75%'><strong>" + Model.Student.AbbreviationText + @"</strong></td>
                </tr>
            </table>
        </td>
    </tr>");

            // Overall Marks
            html.Append(@"
    <tr>
        <td colspan='4' class='borTop borLeft borRight'>
            <table width='100%' cellspacing='5' cellpadding='0'>
                <tr>
                    <td width='25%'><strong>Overall:</strong></td>
                    <td width='25%'><strong>Marks: " + Model.Student.ObtainMarks + "/" + Model.Student.TotalMarks + @"</strong></td>
                    <td width='25%'><strong>Percentage: " + Model.Student.Percentages + @"</strong></td>
                    <td width='25%'><strong>Grade: " + Model.Student.Grade + @"</strong></td>
                </tr>
            </table>
        </td>
    </tr>"
);

            // Co-Scholastic Areas
            html.Append(@"
      <tr>
        <td colspan='4' style='border-top:1px solid #000; border-left:1px solid #000; border-right:1px solid #000; font-family:Helvetica,Arial,sans-serif; font-size:8pt; line-height:10pt;'>
            <table style='width:100%; border-collapse:collapse;' cellspacing='0' cellpadding='5'>
                <thead>
                    <tr align='center'>
                        <th style='border-bottom:1px solid #000; border-right:1px solid #000; width:40%;'>Co-Scholastic Areas: Term-1<br>[on a 3-point (A-C) grading scale]</th>
                        <th style='border-bottom:1px solid #000; border-right:1px solid #000; width:10%;'>Grade</th>
                        <th style='border-bottom:1px solid #000; border-right:1px solid #000; width:40%;'>Co-Scholastic Areas: Term-2<br>[on a 3-point (A-C) grading scale]</th>
                        <th style='border-bottom:1px solid #000; width:10%;'>Grade</th>
                     </tr>
                </thead>
                <tbody>");

            // Data rows
            if (Model.CoScholasticArea != null && Model.CoScholasticArea.Any())
            {
                var areas = Model.CoScholasticArea.Select(x => x.AreaName).Distinct();
                foreach (var area in areas)
                {
                    var term1 = Model.CoScholasticArea.FirstOrDefault(x => x.AreaName == area && x.TermName == "Term 1");
                    var term2 = Model.CoScholasticArea.FirstOrDefault(x => x.AreaName == area && x.TermName == "Term 2");

                    html.Append(@"<tr>");
                    html.Append("<td style='border-bottom:1px solid #000; border-right:1px solid #000;' align='center'>" + area + "</td>");
                    html.Append("<td style='border-bottom:1px solid #000; border-right:1px solid #000;' align='center'>" + (term1?.TermGrade ?? "-") + "</td>");
                    html.Append("<td style='border-bottom:1px solid #000; border-right:1px solid #000;' align='center'>" + area + "</td>");
                    html.Append("<td style='border-bottom:1px solid #000;' align='center'>" + (term2?.TermGrade ?? "-") + "</td>");
                    html.Append("</tr>");
                }
            }

            html.Append(@"</tbody>
            </table>
        </td>
     </tr>");

            // Attendance, Remarks, Promoted Class
            html.Append(@"
    <tr>
        <td colspan='4' class='borTop borLeft borRight'>
            <table width='100%' cellspacing='5' cellpadding='0'>
                <tr>
                    <td><strong>Attendance:</strong> " + Model.Student.Attendancecount + @"</td>
                    <td><strong>Remarks/Status:</strong> " + Model.Student.Remarks + @"</td>
                    <td><strong>Promoted to Class:</strong> " + Model.Student.PromotedToClass + @"</td>
                </tr>
            </table>
        </td>
    </tr>");

            // Signatures
            html.Append(@"
    <tr>
        <td colspan='4' class='borTop borLeft borRight'>
            <table width='100%' cellspacing='5' cellpadding='0'>
                <tr>
                    <td><strong>Date: " + DateTime.Now.ToString("dd-MM-yyyy") + @"</strong></td>
                    <td align='center'><strong>Signature of Class Teacher</strong></td>
                    <td align='right'><strong>Principal's Signature</strong></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr><td colspan='4' class='borTop borLeft borRight borBottom'>&nbsp;</td></tr>");

            html.Append("</tbody></table>");

            return html.ToString();
        }
        [HttpGet]
        public string GetReportcardHtmlForPDF()
        {
            try
            {
                var list = TempData["studentlist"] as List<StudentMasterMDL>;
                TempData.Keep("studentlist");

                if (list == null || !list.Any())
                    return "<h3>No data found</h3>";

                var sb = new StringBuilder();

                // Start HTML for PDF
                sb.Append(@"
<html>
<head>
<style>
body { font-family: Helvetica,Arial,sans-serif; font-size:8pt; }
table { border-collapse: collapse; width:100%; }
td, th { padding:5px; }

.borLeft { border-left:1px solid #000; }
.borRight { border-right:1px solid #000; }
.borTop { border-top:1px solid #000; }
.borBottom { border-bottom:1px solid #000; }

.page-break { page-break-after: always; }
</style>
</head>
<body>
");

                for (int i = 0; i < list.Count; i++)
                {
                    var obj = objSubjectMarkMappingBAL
                        .StudentReportCardDetails_New(Convert.ToInt32(list[i].PK_SudentId));

                    sb.Append(GenerateSingleStudentHtml(obj));

                    // Avoid last blank page
                    if (i < list.Count - 1)
                        sb.Append("<div class='page-break'></div>");
                }

                sb.Append("</body></html>");

                return sb.ToString();
            }
            catch (Exception ex)
            {
                return $"<h3>Error: {ex.Message}</h3>";
            }
        }
    }

}

