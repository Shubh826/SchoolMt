using BAL;
using MDL;
using MDL.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Configuration;
using System.IO;

namespace SchoolMt.Controllers
{
    public class StudentApisController : ApiController
    {
        public static string STMDomainPath = ConfigurationManager.AppSettings["STMDomainPath"].ToString();
        public static string SMTDomainUrl = ConfigurationManager.AppSettings["SMTDomainUrl"].ToString();

        public object ImageHandling { get; private set; }

        [HttpGet]
        [Route("api/StudentApis/GetClass")]
        public ServiceResult<List<DropDownMDL>> GetClass()
        {
            ServiceResult<List<DropDownMDL>> objResult = new ServiceResult<List<DropDownMDL>>();
            string Msg = string.Empty;
            StudentApisBAL objStudentApisBAL = new StudentApisBAL();
            List<DropDownMDL> _ClassDataList = null;

            try
            {
                var result = objStudentApisBAL.GetClass(out _ClassDataList);
                if (_ClassDataList.Count > 0)
                {
                    objResult.Data = _ClassDataList;
                    objResult.Result = true;
                    objResult.Message = "Success";
                    objResult.Description = "Success";
                }
                else
                {
                    objResult.Data = null;
                    objResult.Result = false;
                    objResult.Message = "No Data Found!";
                    objResult.Description = "Failed";
                }

            }
            catch (Exception Ex)
            {
                objResult.Data = null;
                objResult.Result = false;
                objResult.Message = "Error Occured";
                objResult.Description = "Error Occured";
                var objBase = System.Reflection.MethodBase.GetCurrentMethod();
            }

            return objResult;
        }
        [HttpGet]
        [Route("api/StudentApis/GetStudents")]
        public ServiceResult<List<DropDownMDL>> GetStudents(int FK_ClassId)
        {
            ServiceResult<List<DropDownMDL>> objResult = new ServiceResult<List<DropDownMDL>>();
            string Msg = string.Empty;
            StudentApisBAL objStudentApisBAL = new StudentApisBAL();
            List<DropDownMDL> _StudentsDataList = null;

            try
            {
                var result = objStudentApisBAL.GetStudents(FK_ClassId, out _StudentsDataList);
                if (_StudentsDataList.Count > 0)
                {
                    objResult.Data = _StudentsDataList;
                    objResult.Result = true;
                    objResult.Message = "Success";
                    objResult.Description = "Success";
                }
                else
                {
                    objResult.Data = null;
                    objResult.Result = false;
                    objResult.Message = "No Data Found!";
                    objResult.Description = "Failed";
                }

            }
            catch (Exception Ex)
            {
                objResult.Data = null;
                objResult.Result = false;
                objResult.Message = "Error Occured";
                objResult.Description = "Error Occured";
                var objBase = System.Reflection.MethodBase.GetCurrentMethod();
            }

            return objResult;
        }
        [HttpPost]
        [Route("api/StudentApis/PostStudentImage")]
        public ServiceResult<PostStudentImageMDL> PostStudentImage()
        {
            ServiceResult<PostStudentImageMDL> objResult = new ServiceResult<PostStudentImageMDL>();
            PostStudentImageMDL objStudentData = new PostStudentImageMDL();
            StudentApisBAL objStudentApisBAL = new StudentApisBAL();
            Messages msg = new Messages();
            string ServerImagePath = "";
            Boolean check = false;
            try
            {
                var httpRequest = HttpContext.Current.Request;

                //variable assignment
                var formdata = httpRequest.Form;
                HttpPostedFile StudentsImage = null;

                JavaScriptSerializer jss = new JavaScriptSerializer();

                if (httpRequest.Form["StudentData"] != null)
                {
                    string Data = httpRequest.Form["StudentData"];
                    objStudentData = JsonConvert.DeserializeObject<PostStudentImageMDL>(Data);

                }

                if (httpRequest.Files != null && httpRequest.Files.Count > 0)
                {
                    foreach (string img in httpRequest.Files)
                    {
                        if (img == "StudentImage" && httpRequest.Files[img].ContentLength > 0)
                        {
                            StudentsImage = httpRequest.Files[img];
                        }

                    }
                }
                //string ServerImagePath = Server.MapPath("//App_Images/" + "SBTMSStudentImages/StudentImage/");
                ServerImagePath = HttpContext.Current.Server.MapPath("/App_Images/" + "SMSStudentImages/" + objStudentData.ClassId + "/");
                //string targetPath = STMDomainPath + "/App_Images/SBTMSStudentImages/" + "StudentImage/";
                string targetUrl = SMTDomainUrl + "/App_Images/SMSStudentImages/" + objStudentData.ClassId + "/";
                string fileName = string.Empty;

                string srcfile1 = string.Empty;

                string srcfile1URL = string.Empty;

                if (httpRequest.Files != null && httpRequest.Files.Count > 0)
                {
                    foreach (string img in httpRequest.Files)
                    {
                        var content = httpRequest.Files[img];
                        if (img.ToUpper() == "STUDENTIMAGE")
                        {
                            fileName = content.FileName;
                            string fileExtension = Path.GetExtension(fileName);
                            string ImageName = Convert.ToString(objStudentData.StudentId) + "_StudentImage" + fileExtension;
                            srcfile1 = ImageName;
                            srcfile1URL = targetUrl + ImageName;
                            //DeleteIfFileExists(srcfile1URL);
                            string contentType = content.ContentType;
                            srcfile1 = NewUploadImage(content, ServerImagePath, ImageName);
                            objStudentData.StudentImageURL = srcfile1URL;

                        }
                    }

                }
                msg = objStudentApisBAL.PostStudentImage(objStudentData);
                if (msg.Message_Id == 1)
                {
                    objResult.Result = Convert.ToBoolean(msg.Message_Id);
                    objResult.Message = msg.Message;
                }
                else
                {
                    objResult.Data = null;
                    objResult.Result = Convert.ToBoolean(msg.Message_Id);
                    objResult.Message = msg.Message;
                }

                //}
                return objResult;
            }
            catch (Exception ex)
            {
                objResult.Result = false;
                objResult.Message = ex.Message;
                objResult.Data = null;
                var objBase = System.Reflection.MethodBase.GetCurrentMethod();
                return objResult;

            }
        }
        [HttpGet]
        [Route("api/StudentApis/GetSection")]
        public ServiceResult<List<DropDownMDL>> GetSection()
        {
            ServiceResult<List<DropDownMDL>> objResult = new ServiceResult<List<DropDownMDL>>();
            string Msg = string.Empty;
            StudentApisBAL objStudentApisBAL = new StudentApisBAL();
            List<DropDownMDL> _SectionDataList = null;

            try
            {
                var result = objStudentApisBAL.GetSection(out _SectionDataList);
                if (_SectionDataList.Count > 0)
                {
                    objResult.Data = _SectionDataList;
                    objResult.Result = true;
                    objResult.Message = "Success";
                    objResult.Description = "Success";
                }
                else
                {
                    objResult.Data = null;
                    objResult.Result = false;
                    objResult.Message = "No Data Found!";
                    objResult.Description = "Failed";
                }

            }
            catch (Exception Ex)
            {
                objResult.Data = null;
                objResult.Result = false;
                objResult.Message = "Error Occured";
                objResult.Description = "Error Occured";
                var objBase = System.Reflection.MethodBase.GetCurrentMethod();
            }

            return objResult;
        }
        public static string NewUploadImage(HttpPostedFile postedFile, string imagePath, string ImageName)
        {
            try
            {
                if (!System.IO.Directory.Exists(imagePath))
                {
                    System.IO.Directory.CreateDirectory(imagePath);                    //Create directory to save image
                }
                if (System.IO.File.Exists(imagePath + "\\" + ImageName))
                {
                    bool res = DeleteFile(imagePath, ImageName);
                    if (res)
                    {
                        postedFile.SaveAs(imagePath + "\\" + ImageName);
                    }
                }
                else
                {
                    postedFile.SaveAs(imagePath + "\\" + ImageName);
                }

                if (System.IO.File.Exists(imagePath + "\\" + ImageName))
                    return System.IO.Path.Combine(imagePath, ImageName);
                else
                    return "";
            }
            catch (Exception ex)
            {
                //ErrorLogBAL.SetError(ex, System.Reflection.MethodBase.GetCurrentMethod(), "Image Saving", imagePath + " / " + ImageName);
                return "";
            }

        }
        public static bool DeleteFile(string imagepath, string imagename)
        {
            bool result = false;
            if (System.IO.File.Exists(imagepath + "\\" + imagename))
            {
                System.IO.File.Delete(imagepath + "\\" + imagename);
                result = true;
            }
            else
            {
                result = true;
            }
            return result;
        }


    }
}
