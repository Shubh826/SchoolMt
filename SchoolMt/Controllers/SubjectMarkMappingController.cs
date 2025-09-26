using BAL;
using MDL.Common;
using MDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BAL.Common;
using SchoolMt.Common;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.IO;
using System.Text;
using iTextSharp.text;

namespace SchoolMt.Controllers
{
    public class SubjectMarkMappingController : Controller
    {

        private List<SubjectMarkMappingMDL> _list;

        BasicPagingMDL objBasicPagingMDL = null;
        private SubjectMarkMappingBAL objBal;
        public SubjectMarkMappingController()
        {
            objBal = new SubjectMarkMappingBAL();
        }
        // GET: SubjectMarkMapping
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult GetSubjectMarkMappingData(int CurrentPage = 1, string SearchBy = "", string SearchValue = "")
        {
            objBal.GetSubjectMarkMappingData(out _list, out objBasicPagingMDL, 0, SessionInfo.User.fk_companyid, Convert.ToInt32(20), CurrentPage, SearchBy, SearchValue);
            ViewBag.paging = objBasicPagingMDL;
            // TempData["studentlist"] = _list;
            return PartialView("_SubjectMarkMappingnGrid", _list);
        }

        [HttpGet]
        public ActionResult AddEditSubjectMarkMappingData(int id = 0)
        {
            int lookUpId = 0; string actionfrom = "";
            ViewData["companylist"] = CommonBAL.FillCompany(SessionInfo.User.fk_companyid);
            ViewData["Classlist"] = CommonBAL.FillClass(SessionInfo.User.fk_companyid);
            ViewData["ExamCategorylist"] = CommonBAL.GetLookUpList(0, lookUpId, actionfrom, "Exam Category");
            
           // ViewData["LookUplist"] = CommonBAL.GetLookUpList(SessionInfo.User.fk_companyid, 0, "Add");
            if (id > 0)
            {
                objBal.GetSubjectMarkMappingData(out _list, out objBasicPagingMDL, id, SessionInfo.User.fk_companyid);
                return View("AddEditSubjectMarkMappingData", _list[0]);
            }
            else
            {
                SubjectMarkMappingMDL obj = new SubjectMarkMappingMDL();
                obj.FK_CompanyId = 1;
                obj.IsActive = true;
                return View("AddEditSubjectMarkMappingData", obj);
            }
        }


        [HttpPost]
        public ActionResult AddEditSubjectMarkMappingData(SubjectMarkMappingMDL obj)
        {
            ViewData["companylist"] = CommonBAL.FillCompany(SessionInfo.User.fk_companyid);
            ViewData["Classlist"] = CommonBAL.FillClass(SessionInfo.User.fk_companyid);
            //ViewData["LookUplist"] = CommonBAL.GetLookUpList(SessionInfo.User.fk_companyid, 0, "Add");
            obj.userId = SessionInfo.User.userid;
            Messages msg = objBal.InsertSubjectMarkMappingData(obj);
            TempData["Message"] = msg;
            return RedirectToAction("Index");
        }

        public JsonResult GetLookUpDetailList(int companyid, string lookUpTypeName)
        {
            int lookUpId = 0; string actionfrom = ""; 
            return Json(CommonBAL.GetLookUpList(companyid, lookUpId, actionfrom, lookUpTypeName), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSubjectWiseMarksList(int companyid, int examtypeId,int PkId)
        {
            return Json(CommonBAL.GetSubjectWiseMarksList(companyid, examtypeId, PkId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult BindStudent(int classId=0, int companyId = 0)
        {
            return Json(CommonBAL.GetStudentBySchoolWise(classId, companyId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteSchoolConfigurationData(int pkId)
        {
            Messages msg = objBal.DeleteSchoolConfigurationData(pkId);
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StudentReportCardDetails(int id)
        {
            ViewStudentResultMDL obj = new ViewStudentResultMDL();
            obj = objBal.StudentReportCardDetails(id);

            // 1. Create an instance of your model
            var studentData = new ViewStudentResultNewMDL();

            // 2. Populate the model with dummy or database data
            studentData.StudentName = "AYUSHI MISHRA";
            studentData.FatherName = "SATYA PRAKASH MISHRA";
            studentData.MotherName = "VANDANA MISHRA";
            studentData.DateOfBirth = new DateTime(2012, 06, 27);
            studentData.Class = "VI";
            studentData.Section = "B";
            studentData.RollNumber = "09";
            // Make sure this path is correct relative to your project's content folder
            studentData.StudentPhotoUrl = Url.Content("~/Content/Images/ayushi-mishra.jpg");

            // Populate the subjects and marks (You would get this from a database)
            studentData.Subjects = new List<SubjectResult>
        {
            new SubjectResult
            {
                SubjectName = "ENGLISH",
                Term1 = new TermMarks { PT = 7, NB = 5, SEA = 5, HY = 26, Total = 43, Grade = "C2" },
                Term2 = new TermMarks { PT = 6, NB = 5, SEA = 5, YE = 34, Total = 49, Grade = "C2" },
                GrandTotal = 46.0,
                OverallGrade = "C2"
            },
            // ... Add all other subjects from the report card
        };

            // Populate co-scholastic areas
            studentData.CoScholasticAreas = new List<CoScholasticArea>
        {
            new CoScholasticArea { AreaName = "Work Education", Term1Grade = "A", Term2Grade = "A" },
            // ... Add all other co-scholastic areas
        };

            studentData.OverallMarks = 404;
            studentData.OverallMaxMarks = 800;
            studentData.OverallPercentage = 50.50;
            studentData.OverallGrade = "C2";
            studentData.Attendance = 54;
            studentData.Remarks = "Need Improvement";
            studentData.PromotedToClass = "VII";
            studentData.Date = DateTime.Now;

            // 3. Return the View with the populated model
         
            return View(studentData);
        }

        [HttpGet]
        public string PrintReportcard(int id)
        {
            string htmlForPdf = "";
            try
            {
                ViewStudentResultMDL obj = new ViewStudentResultMDL();
                obj = objBal.StudentReportCardDetails(id);
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
            ViewStudentResultMDL obj = objBal.StudentReportCardDetails(id);
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

                return File(pdfBytes, "application/pdf", $"ReportCard_{obj.ExamMarksDetails[0].StudentId}.pdf");
            }
        }

        [NonAction]
        public static string GenerateHtml(ViewStudentResultMDL model)
        {
            var sb = new StringBuilder();
            return sb.ToString();
        }

    }
}