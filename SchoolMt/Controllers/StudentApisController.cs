using BAL;
using MDL;
using MDL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SchoolMt.Controllers
{
    public class StudentApisController : ApiController
    {

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
                var result = objStudentApisBAL.GetStudents(FK_ClassId,out _StudentsDataList);
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

    }
}
