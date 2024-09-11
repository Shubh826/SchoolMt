using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
//using ClosedXML;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using ClosedXML.Excel;

namespace FRGMBSystem.Common
{
    /// <summary>
    /// Created By:Deepak Singh
    /// Created Date:18-08-2017
    /// purpose:Import Data in Excel & Convert List To Table & Convert List To table with specific Coloumn
    /// </summary>
    public class ExcelExportHelper: Controller
    {
        public static string ExcelContentType
        {
            get
            { return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; }
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
        // public static byte[] ExportExcel(DataTable dataTable, string heading = "", bool showSrNo = false, params string[] columnsToTake)  
        //{  
  
        //    byte[] result = null;  
        //    using (ExcelPackage package = new ExcelPackage())  
        //    {  
        //        ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(String.Format("{0} Data",heading));  
        //        int startRowFrom = String.IsNullOrEmpty(heading) ? 1 : 3;  
  
        //        if (showSrNo)  
        //        {  
        //            DataColumn dataColumn = dataTable.Columns.Add("#", typeof(int));  
        //            dataColumn.SetOrdinal(0);  
        //            int index = 1;  
        //            foreach (DataRow item in dataTable.Rows)  
        //            {  
        //                item[0] = index;  
        //                index++;  
        //            }  
        //        }  
  
  
        //        // add the content into the Excel file  
        //        workSheet.Cells["A" + startRowFrom].LoadFromDataTable(dataTable, true);  
  
        //        // autofit width of cells with small content  
        //        int columnIndex = 1;  
        //        //foreach (DataColumn column in dataTable.Columns)  
        //        //{  
        //        //    ExcelRange columnCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, columnIndex];  
        //        //    int maxLength = columnCells.Max(cell => cell.Value.ToString().Count());  
        //        //    if (maxLength < 150)  
        //        //    {  
        //        //        workSheet.Column(columnIndex).AutoFit();  
        //        //    }  
  
  
        //        //    columnIndex++;  
        //        //}  
  
        //        // format header - bold, yellow on black  
        //        using (ExcelRange r = workSheet.Cells[startRowFrom, 1, startRowFrom, dataTable.Columns.Count])  
        //        {  
        //            r.Style.Font.Color.SetColor(System.Drawing.Color.White);  
        //            r.Style.Font.Bold = true;  
        //            r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;  
        //            r.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#1fb5ad"));  
        //        }  
  
        //        // format cells - add borders  
        //        using (ExcelRange r = workSheet.Cells[startRowFrom + 1, 1, startRowFrom + dataTable.Rows.Count, dataTable.Columns.Count])  
        //        {  
        //            r.Style.Border.Top.Style = ExcelBorderStyle.Thin;  
        //            r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;  
        //            r.Style.Border.Left.Style = ExcelBorderStyle.Thin;  
        //            r.Style.Border.Right.Style = ExcelBorderStyle.Thin;  
  
        //            r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);  
        //            r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);  
        //            r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);  
        //            r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);  
        //        }  
  
        //        // removed ignored columns  
        //        for (int i = dataTable.Columns.Count - 1; i >= 0; i--)  
        //        {  
        //            if (i == 0 && showSrNo)  
        //            {  
        //                continue;  
        //            }  
        //            if (!columnsToTake.Contains(dataTable.Columns[i].ColumnName))  
        //            {  
        //                workSheet.DeleteColumn(i + 1);  
        //            }  
        //        }  
  
        //        if (!String.IsNullOrEmpty(heading))  
        //        {  
        //            workSheet.Cells["A1"].Value = heading;  
        //            workSheet.Cells["A1"].Style.Font.Size = 20;  
  
        //            workSheet.InsertColumn(1, 1);  
        //            workSheet.InsertRow(1, 1);  
        //            workSheet.Column(1).Width = 5;  
        //        }  
  
        //        result = package.GetAsByteArray();  
        //    }  
  
        //    return result;  
        //}

        public FileResult ExportExcel<T>(List<T> data, string FileName = "", string ExportFormat= ".xls", string MDLAttrName="", params string[] ColumnsToTake)
        {
            // return ExportExcel(ListToDataTable<T>(data), Heading, showSlno, ColumnsToTake);
             DataTable dt=  ConvertListToDataTable(ListToDataTable<T>(data), MDLAttrName, ColumnsToTake);
            return ExportToFormats(dt, FileName, ExportFormat);

        }
        /// <summary>
        /// By Vinish 10022021 11:24 AM
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="FileName"></param>
        /// <param name="ExportFormat"></param>
        /// <param name="MDLAttrName"></param>
        /// <param name="ColumnsToTake"></param>
        /// <returns></returns>

        public FileResult ExportExcelByClosedXml<T>(List<T> data, string FileName = "", string ExportFormat = ".xls", string MDLAttrName = "", params string[] ColumnsToTake)
        {
            DataTable dt = ConvertListToDataTable(ListToDataTable<T>(data), MDLAttrName, ColumnsToTake);
            dt.TableName = DateTime.Now.ToString("dd.MM.yyyy_HH.mm");
            using (XLWorkbook wb = new XLWorkbook())
            {
                //var table  = wb.Worksheets.Add(dt);
                var table = wb.AddWorksheet(DateTime.Now.ToString("dd.MM.yyyy_HH.mm")).FirstCell().InsertTable(dt);
                //using (var rangeRow = table.HeadersRow())
                //{
                //    rangeRow.Style.Fill.BackgroundColor = XLColor.White;
                //    rangeRow.Style.Font.FontColor = XLColor.FromTheme(XLThemeColor.Text1);
                //}
                table.Theme = XLTableTheme.None;
                
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    if (ExportFormat == ".xls")
                    {
                        return File(stream.ToArray(), "application/vnd.ms-excel", $"{FileName}_{DateTime.Now.ToString("dd.MM.yyyy_HH.mm") }.xls");
                    }

                    else if (ExportFormat == ".xlsx")
                    {
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{FileName}_{DateTime.Now.ToString("dd.MM.yyyy_HH.mm") }.xlsx");
                    }
                    else {
                        return null;
                    }
                }
            }
        }
        public FileResult ExportToFormats(DataTable tableToExport, string ReportType, string ExportFormat)
        {
            GridView DataGrid = new GridView();
            DataGrid.AllowPaging = false;
            DataGrid.DataSource = tableToExport;
            DataGrid.DataBind();
           

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
            else if (ExportFormat == ".csv")
            {
                //Created By : Shubham Singh On 04/11/2020
                byte[] outputBuffer = null;
                using (MemoryStream tempStream = new MemoryStream())
                {
                    using (StreamWriter writer = new StreamWriter(tempStream))
                    {
                        WriteDataTable(tableToExport, writer, true);
                    }

                    outputBuffer = tempStream.ToArray();
                }
                return File(outputBuffer, "text/csv", FileName);
                ////StringBuilder sb = ExportToCSV(tableToExport);
                ////sw = new StringWriter(sb);
                ////hw = new HtmlTextWriter(sw);
                ////content = Encoding.UTF8.GetBytes(sw.ToString());
                ////return File(content, "application/CSV", FileName);
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

        #region csv file export
        // Created By Shubham Singh On 11/03/2020
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
        public DataTable ConvertListToDataTable(DataTable datadt, string MDLAttrName = "", params string[] ColumnsToTake)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Sr. No.", typeof(string));
            foreach (string s in ColumnsToTake)
            {
                dt.Columns.Add(s, typeof(string));
            }
            int index = 1;
            foreach (DataRow lstdr in datadt.Rows)
            {
                DataRow dr = dt.NewRow();
                dr["Sr. No."] = index.ToString();
                var stringArray = MDLAttrName.Split(',');
                int i = 0;
                foreach (string s in ColumnsToTake)
                {
                    dr[s] = lstdr[stringArray[i]!=null? stringArray[i].Trim() : ""].ToString();
                    i = i + 1;
                }
                index = index + 1;
                dt.Rows.Add(dr);
          }
                return dt;
        }


        #region region for export excel common methods for area

        public FileResult ExportExcelArea<T>(List<T> data, string FileName = "", string ExportFormat = ".xls", string MDLAttrName = "", params string[] ColumnsToTake)
        {
            // return ExportExcel(ListToDataTable<T>(data), Heading, showSlno, ColumnsToTake);
            DataTable dt = ConvertListToDataTableArea(ListToDataTable<T>(data), MDLAttrName, ColumnsToTake);
            return ExportToFormatsArea(dt, FileName, ExportFormat);

        }

        public FileResult ExportToFormatsArea(DataTable tableToExport, string ReportType, string ExportFormat)
        {
            GridView DataGrid = new GridView();
            DataGrid.AllowPaging = false;
            DataGrid.DataSource = tableToExport;
            DataGrid.DataBind();


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


        public DataTable ConvertListToDataTableArea(DataTable datadt, string MDLAttrName = "", params string[] ColumnsToTake)
        {
            DataTable dt = new DataTable();
           // dt.Columns.Add("Sr. No.", typeof(string));
            foreach (string s in ColumnsToTake)
            {
                dt.Columns.Add(s, typeof(string));
            }
           // int index = 1;
            foreach (DataRow lstdr in datadt.Rows)
            {
                DataRow dr = dt.NewRow();
               // dr["Sr. No."] = index.ToString();
                var stringArray = MDLAttrName.Split(',');
                int i = 0;
                foreach (string s in ColumnsToTake)
                {
                    dr[s] = lstdr[stringArray[i]].ToString();
                    i = i + 1;
                }
               // index = index + 1;
                dt.Rows.Add(dr);
            }
            return dt;
        }
        #endregion

        /// <summary>
        /// created by ::prince kumar srivastava
        /// created date:21-05-2020
        /// pupose:-convert list to data table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="FileName"></param>
        /// <param name="ExportFormat"></param>
        /// <param name="MDLAttrName"></param>
        /// <param name="ColumnsToTake"></param>
        /// <returns></returns>
        public DataTable ExportExcelDataTable<T>(List<T> data, string MDLAttrName = "", params string[] ColumnsToTake)
        {
            DataTable dt = ConvertListToDataTable(ListToDataTable<T>(data), MDLAttrName, ColumnsToTake);
            return dt;
        }
        /// <summary>
        /// Purpose:-Method to convert data table to CSV File
        /// </summary>
        /// <param name="tableToExport"></param>
        /// <returns>Object Of String Bulder Class</returns>
        public StringBuilder ExportToCSV(DataTable tableToExport)
        {

            StringBuilder sb = new StringBuilder();
            for (int k = 0; k < tableToExport.Columns.Count; k++)
            {
                sb.Append(tableToExport.Columns[k].ColumnName.Replace(",", ";") + ',');
            }

                sb.Append("\r\n");
            for (int i = 0; i < tableToExport.Rows.Count; i++)
            {
                for (int k = 0; k < tableToExport.Columns.Count; k++)
                {
                    sb.Append(tableToExport.Rows[i][k].ToString().Replace(",", ";") + ',');
                }

              sb.Append("\r\n");

            }

            return sb;
        }

        /// 
        /// <summary>
        /// Auther : Vinish
        /// Created Date : 2020-02-19 11:23:36.887
        ///Purpose:- Export To PDF File
        /// </summary>
        /// <returns></returns>
        public byte[] ExportPDF_ByteAarray(string HTMLContent = "")
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
    }
}