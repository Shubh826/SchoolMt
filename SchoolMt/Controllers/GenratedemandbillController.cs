using BAL;
using BAL.Common;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MDL;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Org.BouncyCastle.Asn1.X509;
using SchoolMt.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SchoolMt.Controllers
{
    public class GenratedemandbillController : Controller
    {
        // GET: Genratedemandbill
        public ActionResult Index()
        {
            GenrateDmdbillMdl obj = new GenrateDmdbillMdl();
            ViewData["CompanyList"] = CommonBAL.FillCompany(SessionInfo.User.fk_companyid);
            ViewData["ClassList"] = CommonBAL.FillClassName(SessionInfo.User.fk_companyid);
            if (TempData["Message"] != null)
            {
                ViewBag.Msg = (Messages)TempData["Message"];
                TempData["Message"] = null;
            }
            obj.IsActive = true;
           // obj.IsCompany = true;
            obj.FK_CompanyId = SessionInfo.User.fk_companyid;
            return View("Index", obj);
           
        }

        [HttpPost]
        public ActionResult Genratedemandbill(GenrateDmdbillMdl objGenrateDmdbillMdl)
        {
           // objGenrateDmdbillMdl = new GenrateDmdbillMdl();
            int FK_CompanyId = SessionInfo.User.fk_companyid;
            ViewData["CompanyList"] = CommonBAL.FillCompany(FK_CompanyId);
            ViewData["FormList"] = CommonBAL.FillForm(FK_CompanyId);
            GenrateDmdBillBAL objBal = new GenrateDmdBillBAL();
            List<StudentFeeDetailsMDL> _StudentFeeDetailsMDL = new List<StudentFeeDetailsMDL>();
            bool a = objBal.getFeedetails(out  _StudentFeeDetailsMDL, objGenrateDmdbillMdl);





            return CreatePdf(_StudentFeeDetailsMDL);




           // return View("AddEditRole", objGenrateDmdbillMdl);

        }

        public FileResult CreatePdf(List<StudentFeeDetailsMDL> _StudentFeeDetailsMDL)
        {
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            // file name to be created
            string strPDFFileName = string.Format("Demandbill" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
           // Document doc = new Document();
            Document doc = new Document(PageSize.A4, 25, 25, 25, 15);
            doc.SetMargins(0f, 0f, 0f, 0f);
            // Create PDF Table with 5 columns
            PdfPTable tableLayout = new PdfPTable(2);
            doc.SetMargins(0f, 0f, 0f, 0f);
            // Create PDF Table
            // file will created in this path
            string strAttachment = Server.MapPath("~/Downloadss/" + strPDFFileName);
            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();
            // Add Content to PDF
            doc.Add(Add_Content_To_PDF(tableLayout, _StudentFeeDetailsMDL,doc));
            // Closing the document
            doc.Close();
            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;
            return File(workStream, "application/pdf", strPDFFileName);
        }

        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, List<StudentFeeDetailsMDL> _StudentFeeDetailsMDL, Document doc)
        {
            //float[] headers = { 100, 24, 100  }; // Header Widths
           // tableLayout.SetWidths(headers); // Set the pdf headers
            tableLayout.WidthPercentage = 100; // Set the PDF File witdh percentage
            tableLayout.HeaderRows = 1;
            // Add Title to the PDF file at the top
            //List<Employee> employees = _context.employees.ToList<Employee>();
            tableLayout.AddCell(new PdfPCell(new Phrase("Creating Pdf using ItextSharp", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 2,
                Border = 0,
                PaddingBottom = 10,
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            //// Add header
            ///

            //AddCellToHeader(tableLayout, "StudentName");
            //AddCellToHeader(tableLayout, "FatherName");
            //AddCellToHeader(tableLayout, "MobileNo");
            //AddCellToHeader(tableLayout, "City");
            //AddCellToHeader(tableLayout, "Hire Date");
            //// Add body
            int j = 0,k= _StudentFeeDetailsMDL.Count();
            foreach (var stu in _StudentFeeDetailsMDL)
            {

                //for (int i = 0; i < 2; i++)
                //{
                   
                    PdfPTable table = new PdfPTable(2);
                    table.TotalWidth = 300f;
                    table.LockedWidth = true;
                    Font RED = new Font(Font.FontFamily.HELVETICA, 12, Font.ITALIC, BaseColor.RED);
                    table.AddCell(new PdfPCell(new Phrase("Demand Bill", RED))
                    {
                        Colspan = 2,
                        Border = 0,
                       
                        // PaddingBottom = 10,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    }); ; ;

                    table.AddCell(new PdfPCell(new Phrase("True Sunshine Academy", new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                    {
                        Colspan = 2,
                        Border = 0,
                       // PaddingBottom = 10,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    });
                    table.AddCell(new PdfPCell(new Phrase("Jahanabad Saifabad, Patti,Pratapgarh (U.P.)", new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(11, 0, 0))))
                    {
                        Colspan = 2,
                        Border = 0,
                        PaddingBottom = 10,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    });

                    //PdfPCell cell = new PdfPCell(new Phrase("True Sunshine Academy"));
                    //cell.Colspan = 2;
                    //cell.HorizontalAlignment = 1;
                    //table.AddCell(cell);
                    //PdfPCell celladd = new PdfPCell(new Phrase("Jahanabad Saifabad, Patti,Pratapgarh (U.P.)"));
                    //celladd.Colspan = 2;
                    //celladd.HorizontalAlignment = 1;
                    //table.AddCell(celladd);
                    //for (int i = 0; i < 12; i++)
                    //{
                    //    int currentMonth = (startMonth + i - 1) % 12 + 1;
                    //    int currentYear = startYear + (startMonth + i - 1) / 12;


                    //table.AddCell("Student Name :");
                    //table.AddCell(stu.StudentName);
                    //table.AddCell("Col 3 Row 1");
                    //table.AddCell("Col 1 Row 2");
                    //table.AddCell("Col 2 Row 2");
                    //table.AddCell("Col 3 Row 2");


                    PdfPCell cellname = new PdfPCell(new Phrase("Student Name :" + stu.StudentName.ToString()));
                    cellname.Colspan = 2;
                   //  cellname.HorizontalAlignment = 1;
                    table.AddCell(cellname);
                    PdfPCell cellfname = new PdfPCell(new Phrase("Father Name :" + stu.FatherName));
                    cellfname.Colspan = 2;
                    //cellfname.HorizontalAlignment = 1;
                    table.AddCell(cellfname);
                    PdfPCell cellclass = new PdfPCell(new Phrase("Class :" + stu.ClassName));
                    
                    table.AddCell(cellclass);
                    PdfPCell celldate = new PdfPCell(new Phrase("Date :" + DateTime.Now.ToString("dd/MM/yyyy")));
                    celldate.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(celldate);

                    
                    PdfPCell cellprev = new PdfPCell(new Phrase("Previous due :"));
                    cellprev.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cellprev);
                    PdfPCell cellprevamnt = new PdfPCell(new Phrase(stu.PreviousDueAmount.ToString()));
                    cellprevamnt.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cellprevamnt);
                // int startMonth = 4; // April
                int startYear = DateTime.Now.Year; // Current 
                                                   //DateTime.Now.Year; // Current year
                string[] a = stu.AcademicSession.Split('-');
                if(a.Length>0)
                {
                     startYear = Convert.ToInt32(a[0]);// session year
                }
               

                int endmonth = DateTime.Now.Month; // Current month
                int startMonth = DateTime.ParseExact(stu.ApplicableMonth, "MMMM", CultureInfo.CurrentCulture).Month;
                    // Loop through 12 months starting from the specified start month
                    int dueamount = 0;
                int examfee = 0;
                int duetrnsamount = 0;
                    string duemonthName = string.Empty;
                    for (int m = 0; m < 12; m++)
                    {
                        // Calculate the current month
                        int currentMonth = (startMonth + m- 1) % 12 + 1;
                        // Calculate the current year
                        int currentYear = startYear + (startMonth + m - 1) / 12;
                        
                        if(currentMonth == 4 && stu.AprilFee==0 &&  stu.AprilTrnsFee == 0)
                        {
                            duemonthName = "April";
                            dueamount = stu.ApplicableMonthFee;
                            duetrnsamount = stu.ApplicableTrnsFee;
                        }
                        if (currentMonth == 5 && stu.MayFee == 0 && stu.MayTrnsFee==0)
                        {
                            duemonthName = duemonthName+","+"May";
                            dueamount = dueamount+stu.ApplicableMonthFee;
                            duetrnsamount = duetrnsamount+stu.ApplicableTrnsFee;
                        }
                        if (currentMonth == 6 && stu.JuneFee == 0 )
                        {
                            duemonthName = duemonthName + "," + "June";
                            dueamount = dueamount + stu.ApplicableMonthFee;
                        }
                        if (currentMonth == 7 && stu.JulyFee == 0 && stu.JulyTrnsFee == 0)
                        {
                            duemonthName = duemonthName + "," + "July";
                            dueamount = dueamount + stu.ApplicableMonthFee;
                            duetrnsamount = duetrnsamount + stu.ApplicableTrnsFee;
                        }
                        if (currentMonth == 8 && stu.AugustFee == 0 && stu.AugustTrnsFee == 0)
                        {
                            duemonthName = duemonthName + "," + "August";
                            dueamount = dueamount + stu.ApplicableMonthFee;
                            duetrnsamount = duetrnsamount + stu.ApplicableTrnsFee;
                        }
                        if (currentMonth == 9 && stu.SeptemberFee == 0 && stu.SeptemberTrnsFee == 0)
                        {
                            duemonthName = duemonthName + "," + "September";
                        if (stu.ExaminationFee1 == 0)
                        {
                            examfee = 500;
                        }
                            dueamount = dueamount + stu.ApplicableMonthFee;
                            duetrnsamount = duetrnsamount + stu.ApplicableTrnsFee;
                        }
                        if (currentMonth == 10 && stu.OctoberFee == 0 && stu.OctoberTrnsFee == 0)
                        {
                        if (stu.ExaminationFee1 == 0 && examfee==0)
                        {
                            examfee = 500;
                        }
                        duemonthName = duemonthName + "," + "October";
                            dueamount = dueamount + stu.ApplicableMonthFee;
                            duetrnsamount = duetrnsamount + stu.ApplicableTrnsFee;
                        }
                        if (currentMonth == 11 && stu.NovemberFee == 0 && stu.NovemberTrnsFee == 0)
                        {
                        if (stu.ExaminationFee1 == 0 && examfee == 0)
                        {
                            examfee = 500;
                        }
                        duemonthName = duemonthName + "," + "November";
                            dueamount = dueamount + stu.ApplicableMonthFee;
                            duetrnsamount = duetrnsamount + stu.ApplicableTrnsFee;
                        }
                        if (currentMonth == 12 && stu.DecemberFee == 0 && stu.DecemberTrnsFee == 0)
                        {
                        if (stu.ExaminationFee1 == 0 && examfee == 0)
                        {
                            examfee = 500;
                        }
                        duemonthName = duemonthName + "," + "December";
                            dueamount = dueamount + stu.ApplicableMonthFee;
                            duetrnsamount = duetrnsamount + stu.ApplicableTrnsFee;
                        }
                        if (currentMonth == 1 && stu.JanuaryFee == 0 && stu.JanuaryTrnsFee == 0)
                        {
                        if (stu.ExaminationFee1 == 0 && examfee == 0)
                        {
                            examfee = 500;
                        }
                        duemonthName = duemonthName + "," + "January";
                            dueamount = dueamount + stu.ApplicableMonthFee;
                            
                            duetrnsamount = duetrnsamount + stu.ApplicableTrnsFee;
                        }
                        if (currentMonth == 2 && stu.FebruaryFee == 0 && stu.FebruaryTrnsFee == 0)
                        {
                        if (stu.ExaminationFee1 == 0 && examfee == 0)
                        {
                            examfee = 500;
                        }
                        duemonthName = duemonthName + "," + "February";
                            dueamount = dueamount + stu.ApplicableMonthFee;
                        if (stu.ExaminationFee2 == 0)
                        {
                            examfee = examfee + 500;
                        }
                        duetrnsamount = duetrnsamount + stu.ApplicableTrnsFee;
                        }
                        if (currentMonth == 3 && stu.MarchFee == 0 && stu.MarchTrnsFee == 0)
                        {
                        if (stu.ExaminationFee1 == 0 && examfee == 0)
                        {
                            examfee = 500;
                        }
                        if (stu.ExaminationFee2 == 0 && examfee == 0)
                        {
                            examfee = 500;
                        }
                        duemonthName = duemonthName + "," + "March";
                            dueamount = dueamount + stu.ApplicableMonthFee;
                            duetrnsamount = duetrnsamount + stu.ApplicableTrnsFee;
                        }

                        if(endmonth==currentMonth)
                    {
                        break;
                    }
                    }
                    if (duemonthName.StartsWith(","))
                    {
                        duemonthName = duemonthName.Substring(1);
                    }

                    PdfPCell cellmonthdue = new PdfPCell(new Phrase(duemonthName));
                    cellmonthdue.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cellmonthdue);
                    PdfPCell cellmonthdueamt = new PdfPCell(new Phrase(dueamount.ToString()));
                    cellmonthdueamt.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cellmonthdueamt);
                    PdfPCell cellmonthduetrns = new PdfPCell(new Phrase("Transport"));
                    cellmonthduetrns.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cellmonthduetrns);
               
                PdfPCell cellmonthduetrnsamt = new PdfPCell(new Phrase(duetrnsamount.ToString()));
                    cellmonthduetrnsamt.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cellmonthduetrnsamt);

                PdfPCell cellexam = new PdfPCell(new Phrase("Exam Fee"));
                cellexam.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellexam);
                PdfPCell cellexamamt = new PdfPCell(new Phrase(examfee.ToString()));
                cellexamamt.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cellexamamt);
                PdfPCell celltotal= new PdfPCell(new Phrase("Total Amount", RED));
                    celltotal.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(celltotal);
                    PdfPCell celltotalamt = new PdfPCell(new Phrase((duetrnsamount+dueamount+ stu.PreviousDueAmount+ examfee).ToString(), RED)); ;
                    celltotalamt.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(celltotalamt);

                   
                    PdfPCell cellsig = new PdfPCell(new Phrase("Signature"));
                    cellsig.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellsig.Colspan = 2;
                    cellsig.PaddingBottom = 30; ;
                    table.AddCell(cellsig);

                    // PdfPCell mcell = new PdfPCell(new Phrase(table));
                    tableLayout.AddCell(table);
                if (k % 2 != 0)
                {
                    if (k == j + 1)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(""));
                        tableLayout.AddCell(cell);
                    }
                }

                // }

                if (j > 0 && tableLayout.Rows.Count % 7== 0)
                {
                     doc.Add(tableLayout);
                    tableLayout.DeleteBodyRows();
                    doc.NewPage();
                }

                j++;
                //  }
                //    DateTime endDate = DateTime.Today;
                //    for (DateTime dt = startDate; dt <= endDate; dt = dt.AddMonths(1))
                //    {
                //    }

                //AddCellToBody(tableLayout, stu.StudentName.ToString());
                //AddCellToBody(tableLayout, stu.FatherName);
                //AddCellToBody(tableLayout, stu.MobileNo);
                //AddCellToBody(tableLayout, stu.MobileNo);
                //AddCellToBody(tableLayout, stu.MobileNo);
            }
            return tableLayout;
        }
        // Method to add single cell to the Header
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {

            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.YELLOW)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 5,
                BackgroundColor = new iTextSharp.text.BaseColor(128, 0, 0)
            });
        }
        // Method to add single cell to the body
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 5,
                BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
            });
        }
    }
}