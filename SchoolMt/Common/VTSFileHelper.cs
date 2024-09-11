
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SchoolMt.Common
{
    public class VTSFileHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="path"></param>
        /// <param name="StyleName"></param>
        /// <returns></returns>
        public static bool Upload(HttpPostedFileBase file, string path, string NewName)
        {
            try
            {
                    var fileName = Path.GetFileName(file.FileName);
                    var fileExtension = Path.GetExtension(fileName);
                    var filenamewithoutextension = "";                
                    filenamewithoutextension = !String.IsNullOrEmpty(fileExtension) ? fileName.Replace(fileExtension, "") : fileName;
                var filenamewithextension = NewName;//ResetFileNames(file, NewName);                  
                    bool folderExists = Directory.Exists(path);
                    if (!folderExists)
                       Directory.CreateDirectory(path);
                    file.SaveAs(path + filenamewithextension);
                    return true;
            }
            catch (Exception ex)
            {
                //Error.Log("FileHelperClass-Report-Upload", MethodBase.GetCurrentMethod().ToString(), ex.Message, ex.Source, ex.StackTrace);
                return false;
            }
        }

        /// <summary>
      /// 
      /// </summary>
      /// <param name="fileId"></param>
      /// <param name="filepath"></param>
      /// <param name="filename"></param>
      /// <returns></returns>
        public static string Delete(string fileId, string filepath, string filename)
        {
            try
            {             
                    var f = HttpContext.Current.Server.MapPath("~/" + filepath + filename);
                    if (File.Exists(f))
                    {
                        if (f != null)
                        {
                            File.Delete(f);
                        }
                    }              
            }
            catch (Exception ex)
            {
               //Log the error
                return "f";
            }
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filenamewithoutextension"></param>
        /// <param name="StyleName"></param>
        /// <returns></returns>
        public static string ResetFileNames(HttpPostedFileBase file, string NewName)
        {
            var fileName = Path.GetFileName(file.FileName);
            var fileExtension = Path.GetExtension(fileName);
            string filenamewithoutextension = "";

            filenamewithoutextension = !String.IsNullOrEmpty(fileExtension) ? fileName.Replace(fileExtension, "") : fileName;


            filenamewithoutextension = Regex.Replace(filenamewithoutextension, "&\\w+;", "");
            // remove anything that is not letters, numbers, dash, or space
            filenamewithoutextension = Regex.Replace(filenamewithoutextension, "[^A-Za-z0-9\\-\\s]", "");
            // remove any leading or trailing spaces left over
            filenamewithoutextension = filenamewithoutextension.Trim();
            // replace spaces with single dash
            filenamewithoutextension = Regex.Replace(filenamewithoutextension, "\\s+", "-");
            // if we end up with multiple dashes, collapse to single dash            
            filenamewithoutextension = Regex.Replace(filenamewithoutextension, "\\-{2,}", "-");
            // make it all lower case
            filenamewithoutextension = filenamewithoutextension.ToLower();
            // if it's too long, clip it
            if (filenamewithoutextension.Length > 30)
            {
                filenamewithoutextension = filenamewithoutextension.Substring(0, 29);
            }
            // remove trailing dash, if there is one                        
            if (filenamewithoutextension.EndsWith("-"))
            {
                filenamewithoutextension = filenamewithoutextension.Substring(0, filenamewithoutextension
                                                                                  .Length - 1);
            }

            //filenamewithoutextension = NewName + "_" + filenamewithoutextension;
            filenamewithoutextension = NewName ;
            return filenamewithoutextension+ fileExtension;
        }

        /// <summary>
       /// 
       /// </summary>
       /// <param name="fileExtension"></param>
       /// <returns></returns>
        public static bool CheckUploadedImageExtension(string fileExtension)
        {
            if (!string.IsNullOrEmpty(fileExtension))
            {
                var ext = Path.GetExtension(fileExtension);
                if (ext != null) ext = ext.ToLower();
                switch (ext)
                {
                    //Image Files
                    case ".jpeg":
                        return true;

                    case ".png":
                        return true;

                    case ".bmp":
                        return true;

                    case ".gif":
                        return true;

                    case ".jpg":
                        return true;

                    case ".tiff":
                        return true;
                    case ".pdf":
                        return true;

                    default:
                        return false;
                }
            }
            return true;
        }


     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileSize"></param>
        /// <returns></returns>
        public static bool CheckUploadedFileSize(long fileSize)
        {
            return fileSize <= 20971520;
        }


        public static bool CheckUploadedCalendarFileSize(long fileSize)
        {
            return fileSize <= 3145728;
        }

        public static ImageError CheckValidImage(HttpPostedFileBase file)
        {
            var fileName = Path.GetFileName(file.FileName);
            var fileExtension = Path.GetExtension(fileName);
            var filenamewithoutextension = "";
            if (fileName != null)
                filenamewithoutextension = !String.IsNullOrEmpty(fileExtension) ? fileName.Replace(fileExtension, "") : fileName;
            else
                return ImageError.InValidImage;
            var isValidFile = VTSFileHelper.CheckUploadedFileExtensionForCalendars(fileExtension);
            if (!isValidFile)
            {
                return ImageError.InValidImage;
            }
            var isValidSize = VTSFileHelper.CheckUploadedFileSize(file.ContentLength);
            if (!isValidSize)
            {
               return ImageError.ImageSize;
            }
            var stream = new byte[10];
            var stream2 = new char[10];
            file.InputStream.Read(stream, 0, 9);
            for (var o = 0; o < 9; o++)
            {
                int kkk = stream[o];
                stream2[o] = Convert.ToChar(kkk);
            }
            var s = new string(stream2);
            if (!MimeTypes.IsDCMFile(s) && !MimeTypes.IsExeFile(s))
            {
                return ImageError.NotImage;
            }
            return ImageError.None;
        }

        public static ImageError CheckValidFile(HttpPostedFileBase file)
        {
            var fileName = Path.GetFileName(file.FileName);
            var fileExtension = Path.GetExtension(fileName);
            var filenamewithoutextension = "";
            if (fileName != null)
                filenamewithoutextension = !String.IsNullOrEmpty(fileExtension) ? fileName.Replace(fileExtension, "") : fileName;
            else
                return ImageError.InValidImage;
            var isValidFile = VTSFileHelper.CheckUploadedFileExtension(fileExtension);
            if (!isValidFile)
            {
                return ImageError.InValidImage;
            }
            var isValidSize = VTSFileHelper.CheckUploadedFileSize(file.ContentLength);
            if (!isValidSize)
            {
                return ImageError.ImageSize;
            }
            var stream = new byte[10];
            var stream2 = new char[10];
            file.InputStream.Read(stream, 0, 9);
            for (var o = 0; o < 9; o++)
            {
                int kkk = stream[o];
                stream2[o] = Convert.ToChar(kkk);
            }
            var s = new string(stream2);
            if (!MimeTypes.IsDCMFile(s) && !MimeTypes.IsExeFile(s))
            {
                return ImageError.NotImage;
            }
            return ImageError.None;
        }


        public static bool CheckUploadedFileExtension(string fileExtension)
        {
            if (!string.IsNullOrEmpty(fileExtension))
            {
                var ext = Path.GetExtension(fileExtension);
                if (ext != null) ext = ext.ToLower();
                switch (ext)
                {
                    case ".pdf":
                        return true;
                    case ".docx":
                        return true;
                    case ".doc":
                        return true;
                    case ".xlsx":
                        return true;
                    case ".xls":
                        return true;
                    case ".xlt":
                        return true;
                    case ".xlsm":
                        return true;
                    case ".xlsb":
                        return true;
                    default:
                        return false;
                }
            }
            return true;
        }



        public static ImageError CheckValidCalendarFile(HttpPostedFileBase file)
        {
            var fileName = Path.GetFileName(file.FileName);
            var fileExtension = Path.GetExtension(fileName);
            var filenamewithoutextension = "";
            if (fileName != null)
                filenamewithoutextension = !String.IsNullOrEmpty(fileExtension) ? fileName.Replace(fileExtension, "") : fileName;
            else
                return ImageError.InValidImage;
            var isValidFile = VTSFileHelper.CheckUploadedFileExtensionForCalendars(fileExtension);
            
            if (!isValidFile)
            {
                return ImageError.InValidImage;
            }

            

            var isValidSize = VTSFileHelper.CheckUploadedCalendarFileSize(file.ContentLength);
            if (!isValidSize)
            {
                return ImageError.ImageSize;
            }
            var stream = new byte[10];
            var stream2 = new char[10];
            file.InputStream.Read(stream, 0, 9);
            for (var o = 0; o < 9; o++)
            {
                int kkk = stream[o];
                stream2[o] = Convert.ToChar(kkk);
            }
            var s = new string(stream2);
            if (!MimeTypes.IsDCMFile(s) && !MimeTypes.IsExeFile(s))
            {
                return ImageError.NotImage;
            }
            return ImageError.None;
        }


        public static bool CheckUploadedFileExtensionForCalendars(string fileExtension)
        {
            if (!string.IsNullOrEmpty(fileExtension))
            {
                var ext = Path.GetExtension(fileExtension);
                if (ext != null) ext = ext.ToLower();
                switch (ext)
                {
                    case ".pdf":
                        return true;
                    case ".jpeg":
                        return true;

                    case ".png":
                        return true;

                    case ".bmp":
                        return true;

                    case ".gif":
                        return true;

                    case ".jpg":
                        return true;

                    case ".tiff":
                        return true;

                    default:
                        return false;
                }
            }
            return true;
        }




        public static ImageError CheckValidImageFile(HttpPostedFileBase file)
        {
            var fileName = Path.GetFileName(file.FileName);
            var fileExtension = Path.GetExtension(fileName);
            var filenamewithoutextension = "";
            if (fileName != null)
                filenamewithoutextension = !String.IsNullOrEmpty(fileExtension) ? fileName.Replace(fileExtension, "") : fileName;
            else
                return ImageError.InValidImage;
            var isValidFile = VTSFileHelper.CheckUploadedImageFileExtension(fileExtension);

            if (!isValidFile)
            {
                return ImageError.InValidImage;
            }

            var isValidSize = VTSFileHelper.CheckUploadedCalendarFileSize(file.ContentLength);
            if (!isValidSize)
            {
                return ImageError.ImageSize;
            }
            var stream = new byte[10];
            var stream2 = new char[10];
            file.InputStream.Read(stream, 0, 9);
            for (var o = 0; o < 9; o++)
            {
                int kkk = stream[o];
                stream2[o] = Convert.ToChar(kkk);
            }
            var s = new string(stream2);
            if (!MimeTypes.IsDCMFile(s) && !MimeTypes.IsExeFile(s))
            {
                return ImageError.NotImage;
            }
            return ImageError.None;
        }
        public static bool CheckUploadedImageFileExtension(string fileExtension)
        {
            if (!string.IsNullOrEmpty(fileExtension))
            {
                var ext = Path.GetExtension(fileExtension);
                if (ext != null) ext = ext.ToLower();
                switch (ext)
                {                    
                    case ".jpeg":
                        return true;

                    case ".png":
                        return true;

                    case ".bmp":
                        return true;

                    case ".gif":
                        return true;

                    case ".jpg":
                        return true;

                    default:
                        return false;
                }
            }
            return true;
        }

    }
}
