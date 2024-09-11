using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using System.Web;

namespace SchoolMt.Common
{
    public class MimeTypes
    {
        private static List<string> knownTypes;

        private static Dictionary<string, string> mimeTypes;
        private static Dictionary<string, string> ExtensionMap = new Dictionary<string, string>();

        [DllImport("urlmon.dll", CharSet = CharSet.Auto)]
        private static extern UInt32 FindMimeFromData(
            UInt32 pBC, [MarshalAs(UnmanagedType.LPStr)]
        string pwzUrl, [MarshalAs(UnmanagedType.LPArray)]
        byte[] pBuffer, UInt32 cbSize, [MarshalAs(UnmanagedType.LPStr)]
        string pwzMimeProposed, UInt32 dwMimeFlags, ref UInt32 ppwzMimeOut, UInt32 dwReserverd
    );

        public static bool GetContentType(HttpPostedFileBase file, string path)
        {
            if (knownTypes == null || mimeTypes == null)
                InitializeMimeTypeLists();
            string contentType = "";
            var type = false;
            string extension = Path.GetExtension(file.FileName).Replace(".", "").ToLower();
            mimeTypes.TryGetValue(extension, out contentType);
            if (!string.IsNullOrEmpty(contentType))
            {
                if (knownTypes.Contains(contentType))
                {
                    string headerType = ScanFileForMimeType(file.FileName, path);
                    if (headerType != "application/octet-stream" || string.IsNullOrEmpty(contentType))
                    {
                        type = true;
                    }
                }
            }

            return type;
        }

        private static string ScanFileForMimeType(string fileName, string path)
        {
            try
            {
                byte[] buffer = new byte[256];
                using (FileStream fs = new FileStream(path + fileName, FileMode.Open))
                {
                    int readLength = Convert.ToInt32(Math.Min(256, fs.Length));
                    fs.Read(buffer, 0, readLength);
                }

                UInt32 mimeType = default(UInt32);
                FindMimeFromData(0, null, buffer, 256, null, 0, ref mimeType, 0);
                IntPtr mimeTypePtr = new IntPtr(mimeType);
                string mime = Marshal.PtrToStringUni(mimeTypePtr);
                Marshal.FreeCoTaskMem(mimeTypePtr);
                if (string.IsNullOrEmpty(mime))
                    mime = "application/octet-stream";
                return mime;
            }
            catch (Exception ex)
            {
                return "application/octet-stream";
            }
        }

        private static void InitializeMimeTypeLists()
        {
            knownTypes = new string[]
                         {
                             "text/plain",
                             "text/richtext",
                             "image/gif",
                             "image/jpeg",
                             "image/pjpeg",
                             "image/png",
                             "image/x-png",
                             "image/tiff",
                             "image/bmp",
                             "image/x-xbitmap",
                             "application/pdf",
                             "application/msword",
                             "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                         }.ToList();

            mimeTypes = new Dictionary<string, string>
                        {
                            {"bm", "image/bmp"},
                            {"bmp", "image/bmp"},
                            {"doc", "application/msword"},
                            {"docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
                            {"jpeg", "image/jpeg"},
                            {"jpg", "image/jpeg"},
                            {"pdf", "application/pdf"},
                            {"png", "image/png"},
                            {"rt", "text/richtext"},
                            {"rtf", "text/richtext"},
                            {"text", "text/plain"},
                            {"tif", "image/tiff"},
                            {"tiff", "image/tiff"},
                            {"txt", "text/plain"}
                        };
        }

        public static bool IsExeFile(string path)
        {
            var returntype = false;
            if (path.StartsWith("%PDF")) //Pdf file hex check
            {
                returntype = true;
            }
            else if (path.StartsWith("PK")) //DOCX file hex check
            {
                returntype = true;
            }
            else if (path.Contains("PNG")) //PNG file hex check
            {
                returntype = true;
            }
            else if (path.StartsWith("ÿØÿà") || path.StartsWith("ÿØÿá")) //JPG file hex check
            {
                returntype = true;
            }
            else if (path.StartsWith("GIF89a") || path.StartsWith("GIF87a")) //GIF file hex check
            {
                returntype = true;
            }
            else if (path.StartsWith("BM")) //BMP file hex check
            {
                returntype = true;
            }
            else if (path.StartsWith("II*") || path.StartsWith("MM.*")) //tiff file hex check
            {
                returntype = true;
            }
            else if (path.StartsWith("ÐÏà¡±á")) //Doc file hex check
            {
                returntype = true;
            }
            else if (path.StartsWith("MZ")) //EXE file hex check
            {
                returntype = false;
            }

            return returntype;
        }

        public static bool IsDCMFile(string path)
        {
            var returntype = !path.StartsWith("MZ");

            return returntype;
        }
    }
}
