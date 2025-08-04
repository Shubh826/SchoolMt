using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Data;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web.UI;
namespace SchoolMt.Common
{
    public class ExportHelper : Controller
    {
        public byte[] ExportPDF_ByteAarray(string HTMLContent = "", string pageSize = null)
        {
            byte[] bytes = null;
            //StringBuilder strhtml = new StringBuilder();
            //string filePath = HostingEnvironment.MapPath("~/html-templetes/IDCardForm.html");
            //strhtml.Append(System.IO.File.ReadAllText(filePath));
            //
            //strhtml.Replace("[Member-Image]", "http://elen.in/images/resource/testimonial1.jpg");
            //strhtml.Replace("[Member-No]", "9999999");
            //strhtml.Replace("[Member-Name]", "Vinish");

            //string HTMLContent = strhtml.ToString();
            StringReader sr = new StringReader(HTMLContent);

            // comment code start//
            //Boilerplate iTextSharp setup here
            //  Create a stream that we can write to, in this case a MemoryStream
            using (var ms = new MemoryStream())
            {

                //Create an iTextSharp Document which is an abstraction of a PDF but **NOT** a PDF
                using (var doc = new Document(PageSize.A4, 20f, 20f, 20f, 20f))
                {

                    //Create a writer that's bound to our PDF abstraction and our stream
                    using (var writer = PdfWriter.GetInstance(doc, ms))
                    {

                        //Open the document for writing
                        doc.Open();
                        // doc.Add(new Chunk("")); // << this will do the trick. 

                        //Our sample HTML and CSS
                        string example_html = HTMLContent;
                        string example_css = "";//@".borLeft { border-left: 1px solid #000; } .borRight { border-right: 1px solid #000; } .borTop { border-top: 1px solid #000; } .borBottom { border-bottom: 1px solid #999; } .borBottomB { border-bottom: 1px solid #000; } .top_aligned_image { vertical-align: top; } .dotted { font-family: Helvetica, Arial, sans-serif; line-height: 10pt; font-weight: bold; margin: 0; text-align: center; border: 1px dashed #000; padding: 10pt 15pt; float: right; }";//@"td

                        using (var msCss = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_css)))
                        {
                            using (var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_html)))
                            {
                                //Parse the HTML
                                iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msHtml, msCss);
                            }
                        }
                        doc.Close();
                    }
                }

                //After all of the PDF "stuff" above is done and closed but **before** we
                //close the MemoryStream, grab all of the active bytes from the stream
                bytes = ms.ToArray();
            }
            // comment code end

            return bytes;
        }

        public string GetHtmlString(string HTMLContent = "")
        {
            string printJs = @"<script>window.print();</script>";
            string _HTMLContent = HTMLContent.ToString(); 
            return _HTMLContent;

        }

        public FileResult ExportExcel<T>(List<T> data, string FileName = "", string ExportFormat = ".xls", string MDLAttrName = "", params string[] ColumnsToTake)
        {
            // return ExportExcel(ListToDataTable<T>(data), Heading, showSlno, ColumnsToTake);
            DataTable dt = ConvertListToDataTable(ListToDataTable<T>(data), MDLAttrName, ColumnsToTake);
            return ExportToFormats(dt, FileName, ExportFormat);

        }

        public FileResult ExportToFormats(DataTable tableToExport, string ReportType, string ExportFormat)
        {
            GridView DataGrid = new GridView();
            DataGrid.AllowPaging = false;
            DataGrid.DataSource = tableToExport;
            DataGrid.DataBind();
            /*Start: Prince* :-To Resolve Large number shows with+E*/
            int cellcount = DataGrid.Rows[0].Cells.Count;
            DataGrid.Rows[0].HorizontalAlign = HorizontalAlign.Left;
            for (int i = 0; i < DataGrid.Rows.Count; i++)
            {

                for (int j = 0; j < cellcount; j++)
                {

                    DataGrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Left;
                    DataGrid.Rows[i].Cells[j].Attributes.Add("style", "mso-number-format:\\@");
                }
            }
            /*End: Prince :-To Resolve Large number shows with+E*/
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            DataGrid.RenderControl(hw);
            byte[] content = Encoding.UTF8.GetBytes(sw.ToString());

            string FileName = ReportType + "_" + DateTime.Now.ToString("dd.MM.yyyy_HH.mm") + ExportFormat;

            if (ExportFormat == ".xls")
            {
                return File(content, "application/vnd.ms-excel", FileName); //"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            }

            else if (ExportFormat == ".xlsx")
            {
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName);
            }

            else if (ExportFormat == ".doc")
            {
                return File(content, "application/msword", FileName); // "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
            }

            else if (ExportFormat == ".docx")
            {
                return File(content, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", FileName);
            }
            else if (ExportFormat == ".pdf")
            {
                content = ExportPDF(tableToExport, ReportType);
                return File(content, "application/octet-stream", FileName);

            }
            else if (ExportFormat == ".csv")
            {
                // Create a MemoryStream to hold CSV 
                byte[] outputBuffer = null;
                using (MemoryStream tempStream = new MemoryStream())
                {
                    // Write CSV content to the MemoryStream with UTF-8 encoding and BOM
                    using (StreamWriter writer = new StreamWriter(tempStream, new UTF8Encoding(true)))
                    {
                        // Write DataTable content to the StreamWriter
                        WriteDataTable(tableToExport, writer, true);
                    }

                    // Get the byte array from the MemoryStream
                    outputBuffer = tempStream.ToArray();

                    // Return the CSV file as a FileResult
                    return File(outputBuffer, "text/csv", FileName);
                }
            }
            else
            {
                //Response.Clear();
                //Response.ContentType = "application/pdf";                
                //Response.AddHeader("content-disposition", "attachment;filename=" +FileName+ "");
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //DataGrid.RenderControl(hw);
                //StringReader sr = new StringReader(sw.ToString());
                //Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                //pdfDoc.Open();
                //htmlparser.Parse(sr);
                //pdfDoc.Close();
                //Response.Write(pdfDoc);
                //Response.End();
                ////ExportToPDF(tableToExport, FileName);
                return null;
            }
        }

        public static DataTable ListToDataTable<T>(List<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dataTable = new DataTable();

            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];
                dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }

            object[] values = new object[properties.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);
                }

                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
        public DataTable ConvertListToDataTable(DataTable datadt, string MDLAttrName = "", params string[] ColumnsToTake)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("No" + ".", typeof(string));
            foreach (string s in ColumnsToTake)
            {
                dt.Columns.Add(s, typeof(string));
            }
            int index = 1;
            foreach (DataRow lstdr in datadt.Rows)
            {
                DataRow dr = dt.NewRow();
                dr["No" + "."] = index.ToString();
                var stringArray = MDLAttrName.Split(',');
                int i = 0;
                foreach (string s in ColumnsToTake)
                {
                    dr[s] = lstdr[stringArray[i] != null ? stringArray[i].Trim() : ""].ToString();
                    i = i + 1;
                }
                index = index + 1;
                dt.Rows.Add(dr);
            }
            return dt;
        }


        public byte[] ExportPDF(DataTable tableToExport, string HeaderText, string FileName = "")
        {
            byte[] bytes = null;
            // DataTable tableToExport = new DataTable();
            // tableToExport = ConvertListToDataTable(ListToDataTable<T>(data), MDLAttrName, ColumnsToTake);
            // string url = "";
            //*my code start-----------------*//
            StringBuilder strhtml = new StringBuilder();
            strhtml.Append("<html>");
            strhtml.Append("<head>");
            strhtml.Append("</head>");
            strhtml.Append("<body style='font-family:Helvetica,Arial,sans-serif;font-size:8pt;'>");
            // strhtml.Append("<table width='100%' border='1px' cellspacing='0' cellpadding='3' style='font-family:Helvetica,Arial, sans-serif;font-size:8pt;line-height:10pt;'>");

            strhtml.Append("<div>");
            //correct code start//

            strhtml.Append("<div style='width:100%'>");
            strhtml.Append("<div style='width:40%;'><img src='D:/Multilingual/MFTS/MFTS/app-assets/images/logo/logo-dark.png' alt='logo' height='40px' weight='124px'/></div>");
            strhtml.Append("<div style='width:98%;font-weight:bold;text-align:right'>" + HeaderText.ToString() + "</div>");
            strhtml.Append("</div>");
            strhtml.Append("<hr/>");
            //correct code End//


            //code start text
            //strhtml.Append("<table>");
            //strhtml.Append("<tr><td><img src='D:/Multilingual/MFTS/MFTS/app-assets/images/logo/logo-dark.png' alt='logo' height='40px' weight='124px'/>"+"<p align='center'>"+ HeaderText.ToString()+ "</p></td></tr></table>");
            //// strhtml.Append("<td>" + HeaderText.ToString() + "</td></tr></table>");
            ////code End text


            strhtml.Append("<table width='100%' border='1' cellspacing='0' cellpadding='3' style='font-family:Helvetica,Arial, sans-serif;font-size:8pt;'>");

            // strhtml.Append("<table class='table table-striped table-bordered dataTable mb-0' width='100%' style='font-family:Helvetica,Arial, sans-serif;font-size:8pt;border:'1px;''>");

            strhtml.Append("<tbody>");

            strhtml.Append("<tr>");
            for (int i = 0; i < tableToExport.Columns.Count; i++)
            {
                strhtml.Append("<th  class='thback' style='bgcolor:'#FF0000';font-color:'#fff';border:'1';font-family:Helvetica,Arial,sans-serif;font-size:8pt;margin:0;background-color:'black'; color:'white';' align='center'>" + tableToExport.Columns[i].ToString() + "</th>");
            }
            strhtml.Append("</tr>");

            for (int i = 0; i < tableToExport.Rows.Count; i++)
            {
                strhtml.Append("<tr>");
                for (int j = 0; j < tableToExport.Columns.Count; j++)
                {
                    strhtml.Append("<td  style='font-family:Helvetica,Arial,sans-serif;font-size:8pt;' align='center'>" + tableToExport.Rows[i][j].ToString() + "</td>");
                    // table.AddCell(tableToExport.Rows[i][j].ToString());
                }
                strhtml.Append("</tr>");
            }

            strhtml.Append("</tbody>");
            strhtml.Append("</table>");
            strhtml.Append("</div>");
            strhtml.Append("</body>");
            strhtml.Append("</html>");

            string HTMLContent = strhtml.ToString();
            StringReader sr = new StringReader(strhtml.ToString());

            // comment code start//
            //Boilerplate iTextSharp setup here
            //  Create a stream that we can write to, in this case a MemoryStream
            using (var ms = new MemoryStream())
            {

                //Create an iTextSharp Document which is an abstraction of a PDF but **NOT** a PDF
                using (var doc = new Document(PageSize.A4, 10f, 10f, 10f, 10f))
                {

                    //Create a writer that's bound to our PDF abstraction and our stream
                    using (var writer = PdfWriter.GetInstance(doc, ms))
                    {

                        //Open the document for writing
                        doc.Open();
                        // doc.Add(new Chunk("")); // << this will do the trick. 

                        //Our sample HTML and CSS
                        string example_html = strhtml.ToString();
                        string example_css = @"td{border='1px'}.borLeft{border-left:1px;}.borRight{border-right:1px;}.borTop{border-top:1px;}.borBottom{border-bottom:1px;}
                                                .dotted{font-family: Helvetica, Arial, sans-serif;line-height: 10pt;font-weight: bold;margin: 0;text-align:center;border:1px dashed #000;padding: 10pt 15pt;float:right;}
                                        .thback{background-color: #bde9ba;} 
                                                ";

                        //  string example_css = @"";
                        //In order to read CSS as a string we need to switch to a different constructor
                        //that takes Streams instead of TextReaders.
                        //Below we convert the strings into UTF8 byte array and wrap those in MemoryStreams
                        using (var msCss = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_css)))
                        {
                            using (var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_html)))
                            {

                                //Parse the HTML
                                iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msHtml, msCss);
                            }
                        }
                        doc.Close();
                    }
                }

                //After all of the PDF "stuff" above is done and closed but **before** we
                //close the MemoryStream, grab all of the active bytes from the stream
                bytes = ms.ToArray();
            }
            // comment code end
            return bytes;
        }

        #region csv helper
        public static void WriteDataTable(DataTable sourceTable, TextWriter writer, bool includeHeaders)
        {
            if (includeHeaders)
            {
                IEnumerable<String> headerValues = sourceTable.Columns
                    .OfType<DataColumn>()
                    .Select(column => QuoteValue(column.ColumnName));

                writer.WriteLine(String.Join(",", headerValues));
            }

            IEnumerable<String> items = null;

            foreach (DataRow row in sourceTable.Rows)
            {
                items = row.ItemArray.Select(o => QuoteValue(o?.ToString() ?? String.Empty));
                writer.WriteLine(String.Join(",", items));
            }

            writer.Flush();
        }

        private static string QuoteValue(string value)
        {
            return String.Concat("\"",
            value.Replace("\"", "\"\""), "\"");
        }

        #endregion

    }
}