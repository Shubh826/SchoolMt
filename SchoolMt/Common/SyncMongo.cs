using BAL;
using MDL;
using MDL.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;

namespace SchoolMt.Common
{
    public class SyncMongo
    {
        internal Messages RegisterClientOrCompany(RegisterClientMDL _SyncMongoList)
        {
            Messages result = new Messages();
            try
            {
                InstallationBAL objInstallationBAL = new InstallationBAL();
                // string path = "http://180.179.221.27:8081/vts-service/registerClientOrCompany ";

                //string URL = path;

                string path = WebConfigurationManager.AppSettings["path"];
                string URL = path + "vts-service/registerClientOrCompany";

                var DATA = JsonConvert.SerializeObject(_SyncMongoList);
                WebRequest webRequest = WebRequest.Create(URL);
                var data = Encoding.ASCII.GetBytes(DATA);
                HttpWebRequest httpRequest = (HttpWebRequest)webRequest;
                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/json";
                httpRequest.ContentLength = data.Length;
                using (Stream webStream = httpRequest.GetRequestStream())
                using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
                {
                    requestWriter.Write(DATA);
                }
                WebResponse webResponse = httpRequest.GetResponse();
                Stream responseStream = webResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream);
                string resultdata = streamReader.ReadToEnd();

                JavaScriptSerializer jss = new JavaScriptSerializer();
                ServiceResultMDL<string> objServiceResult = jss.Deserialize<ServiceResultMDL<string>>(resultdata);
                if (objServiceResult.Message.ToUpper() == "SUCCESSFULL")
                {
                    result.Message = "SUCCESSFULL";
                    result.Message_Id = 1;
                    // result = objInstallationBAL.AddEdit_MapMachineDeviceSyncStatus(lst.FK_MachineId, true);


                }
            }
            catch (Exception ex)
            {
                result.Message = "Failed";
                result.Message_Id = 0;
            }
            return result;
        }
        
    }
}