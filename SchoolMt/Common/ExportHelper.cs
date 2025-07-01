using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
namespace SchoolMt.Common
{
    public class ExportHelper
    {
        /// 
        /// <summary>
        /// Auther : Vinish
        /// Created Date : 2020-02-19 11:23:36.887
        ///Purpose:- Export To PDF File
        /// </summary>
        /// <returns></returns>
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
    }
}