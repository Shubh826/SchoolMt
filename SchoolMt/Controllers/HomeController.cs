using BAL.Account;
using SchoolMt.Common;
using SchoolMt.Filter;
using Cryptography;
using MDL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace SchoolMt.Controllers
{
    [HandleErrorExt]
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class HomeController : Controller
    {
        private List<FormMDL> _formlist;
        UserMDL _User;
        LoginBAL objlogin = new LoginBAL();
        //UserBAL objUserBal = new UserBAL();
        private List<UserMDL> _UserDatalist;

        // BasicPagingMDL objBasicPagingMDL = null;

        public ActionResult HomePage()
        {
            return View();
        }

        [SkipCustomAuthenticationAttribute]
        public ActionResult Index()
        {
            return View();
        }

        [SkipCustomAuthenticationAttribute]
        public ActionResult Login()
        {
            LoginMDL ObjLoginMDL = new LoginMDL();
            if (Request.Cookies["userid"] != null)
                ObjLoginMDL.UserName = Request.Cookies["userid"].Value;
            if (Request.Cookies["pwd"] != null)
                ObjLoginMDL.Password = Request.Cookies["pwd"].Value;
            if (Request.Cookies["userid"] != null && Request.Cookies["pwd"] != null)
                ObjLoginMDL.RememberMe = true;
            if (TempData["ErrMessage"] != null)
            {
                ViewBag.Message = (Messages)TempData["Message"];
                ViewBag.Message_Id = (Messages)TempData["Message_Id"];
                TempData["ErrMessage"] = null;
            }
            return View(ObjLoginMDL);
        }

        [HttpPost]
        [SkipCustomAuthenticationAttribute]
        public ActionResult Login(LoginMDL ObjLoginMDL)
        {
            if (ModelState.IsValid)
            {
                ObjLoginMDL.Password = ClsCrypto.Encrypt(ObjLoginMDL.Password);
                LoginBAL objLoginBAL = new LoginBAL();
                Messages objMsg = objLoginBAL.AuthenticateUser(ObjLoginMDL, out _User, out _formlist);
                if (objMsg.Message_Id == 1)
                {
                    if (ObjLoginMDL.RememberMe == true)
                    {
                        Response.Cookies["userid"].Value = ObjLoginMDL.UserName;
                        Response.Cookies["pwd"].Value = ClsCrypto.Decrypt(ObjLoginMDL.Password);
                        Response.Cookies["userid"].Expires = DateTime.Now.AddDays(5);
                        Response.Cookies["pwd"].Expires = DateTime.Now.AddDays(5);
                    }
                    else
                    {
                        Response.Cookies["userid"].Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies["pwd"].Expires = DateTime.Now.AddDays(-1);
                    }
                    SessionInfo.User = _User;
                    SessionInfo.formlist = _formlist;
                    //SessionInfo.CompanyRights = _CompanyRightsMDL;
                    string actionName = SessionInfo.formlist.FirstOrDefault(x => x.PK_FormId == Convert.ToInt32(x.HomePage)).ActionName;
                    string controllerName = SessionInfo.formlist.FirstOrDefault(x => x.PK_FormId == Convert.ToInt32(x.HomePage)).ControllerName;
                    string area = SessionInfo.formlist.FirstOrDefault(x => x.PK_FormId == Convert.ToInt32(x.HomePage)).Area;
                    if (string.IsNullOrWhiteSpace(actionName) || string.IsNullOrWhiteSpace(actionName))
                    {
                        actionName = "HomePage";
                        controllerName = "Home";
                    }

                    return RedirectToAction(actionName, controllerName, new { Area = area });
                }
                else
                {
                    ViewBag.Message = objMsg.Message;
                    TempData["ErrMessage"] = objMsg;
                    //TempData["Message_Id"] = objMsg.Message_Id;

                    return View(ObjLoginMDL);
                }
            }
            else
            {
                return View(ObjLoginMDL);
            }
        }


        [SkipCustomAuthenticationAttribute]
        public ActionResult ForgetPassword()
        {
            TempData.Keep();
            ViewBag.Message = TempData["Mess"];
            TempData["Mess"] = null;
            return View();
        }

        [HttpPost]
        [SkipCustomAuthenticationAttribute]
        public ActionResult ForgotPassword(string EmailID)
        {
            //Verify Email ID
            //Generate Reset password link 
            //Send Email 
            string message = "";
            Messages mess = new Messages();
            bool status = false;


            LoginMDL obj = objlogin.GetUserDetails(EmailID);
            var account = obj.EmailID;
            if (account != null)
            {
                //Send email for reset password
                string resetCode = Guid.NewGuid().ToString();
                SendVerificationLinkEmail(account, resetCode, "ResetPassword");
                mess = objlogin.InsertResetcode(resetCode, account);
                message = mess.Message;
            }
            else
            {
                message = "User not found";
            }

            ViewBag.Message = message;
            TempData["Mess"] = message;
            return RedirectToAction("ForgetPassword");
        }


        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/portal" + "/Home/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress(ConfigurationManager.AppSettings["FromMail"], ConfigurationManager.AppSettings["SenderName"]);
            var toEmail = new MailAddress(emailID);
            //var fromEmailPassword = ConfigurationManager.AppSettings["Password"];// Replace with actual password

            string subject = "";
            string body = "";
            if (emailFor == "VerifyAccount")
            {
                body = string.Empty;
                var root = AppDomain.CurrentDomain.BaseDirectory; using (var reader = new System.IO.StreamReader(root + @"/Templates/Signup-Email.html"))
                {
                    string readFile = reader.ReadToEnd();
                    string StrContent = string.Empty;
                    StrContent = readFile;
                    //Assing the field values in the template
                    StrContent = StrContent.Replace("[USERName]", emailID);
                    //StrContent = StrContent.Replace("[Year]", DateTime.Now.Year.ToString());
                    body = StrContent.ToString();
                }

                subject = "Your account is successfully created!";


            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";
                body = "Hi, <br />  <br /> We got request for reset your account password. Please click on the below link to reset your password" +
                    " <br />  <br /> <a href=" + link + ">Reset Password link</a>";
            }
            //else
            //{
            //    //subject = "New account is created!";
            //    //var root = AppDomain.CurrentDomain.BaseDirectory; using (var reader = new System.IO.StreamReader(root + @"/Templates/Signup-Email.html")) ;
            //    //var mailer = AppDomain.CurrentDomain.BaseDirectory; using (var reader = new System.IO.StreamReader(root + @"/Templates/mailer-banner.png"))
            //    //{
            //    //    string readFile = reader.ReadToEnd();
            //    //    string StrContent = string.Empty;
            //    //    StrContent = readFile;
            //    //    //Assing the field values in the template
            //    //    StrContent = StrContent.Replace("[USERName]", emailID);
            //    //    StrContent = StrContent.Replace("[mailerbanner]", mailer);
            //    //    //StrContent = StrContent.Replace("[Code]", callbackUrl);
            //    //    //StrContent = StrContent.Replace("[Year]", DateTime.Now.Year.ToString());
            //    //    body = StrContent.ToString();
            //    //}


            //}
            ////var smtp = new SmtpClient { Host = ConfigurationManager.AppSettings["Host"], Port = 25, EnableSsl = true,
            //    DeliveryMethod = SmtpDeliveryMethod.Network, UseDefaultCredentials = false, Credentials = new NetworkCredential(fromAddress.Address, fromPassword) };

            //var smtp = new SmtpClient
            //{
            //    Host = ConfigurationManager.AppSettings["Host"],
            //    Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
            //    EnableSsl = true,
            //    DeliveryMethod = SmtpDeliveryMethod.Network,
            //    UseDefaultCredentials = false,
            //    Credentials = new System.Net.NetworkCredential(fromEmail.Address, fromEmailPassword)




            //};

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
                From = fromEmail

            })
                Email.SendMailFromSendGrid(message).Wait();
        }
        [NonAction]
        public void signupemailtomerchent(signupMDL ObjsignupMDL)
        {

            var fromEmail = new MailAddress(ConfigurationManager.AppSettings["FromMail"], ConfigurationManager.AppSettings["SenderName"]);
            var toEmail = new MailAddress(ConfigurationManager.AppSettings["adminmail"]);
            var fromEmailPassword = ConfigurationManager.AppSettings["Password"];// Replace with actual password

            string subject = "";
            string body = "";
            body = string.Empty;
            var root = AppDomain.CurrentDomain.BaseDirectory; using (var reader = new System.IO.StreamReader(root + @"/Templates/Merchent-Signup-Email.html"))
            {
                string readFile = reader.ReadToEnd();
                string StrContent = string.Empty;
                StrContent = readFile;
                //Assing the field values in the template
                StrContent = StrContent.Replace("[Email]", ObjsignupMDL.EmailID);
                StrContent = StrContent.Replace("[FirstName]", ObjsignupMDL.FName);
                StrContent = StrContent.Replace("[CompanyName]", ObjsignupMDL.CompanyName);
                StrContent = StrContent.Replace("[ContactNo]", ObjsignupMDL.ContactNo);
                if (ObjsignupMDL.IsAgency == true)
                {
                    ObjsignupMDL.AccountType = "Agency";
                }
                if (ObjsignupMDL.IsClient == true)
                {
                    ObjsignupMDL.AccountType = "Client";
                }
                if (ObjsignupMDL.IsBoth == true)
                {
                    ObjsignupMDL.AccountType = "Both";
                }
                StrContent = StrContent.Replace("[Type]", ObjsignupMDL.AccountType);
                body = StrContent.ToString();
            }
            subject = "New Signup Alert";
            //var smtp = new SmtpClient
            //{
            //    Host = ConfigurationManager.AppSettings["Host"],
            //    Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
            //    EnableSsl = true,
            //    DeliveryMethod = SmtpDeliveryMethod.Network,
            //    UseDefaultCredentials = false,
            //    Credentials = new System.Net.NetworkCredential(fromEmail.Address, fromEmailPassword)




            //};

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
                From = fromEmail

            })
                //smtp.Send(message);
                Email.SendMailFromSendGrid(message).Wait();
        }



        [SkipCustomAuthenticationAttribute]
        public ActionResult SignUp()
        {
            TempData.Keep();
            ViewBag.Message = TempData["Message"];
            TempData["Message"] = null;
            return View();
        }


        [HttpPost]
        [SkipCustomAuthenticationAttribute]
        public ActionResult signup(signupMDL ObjsignupMDL)
        {
            if (ModelState.IsValid)
            {
                var adminmail = new MailAddress(ConfigurationManager.AppSettings["adminmail"]);
                string admin = adminmail.ToString();
                ObjsignupMDL.Password = ClsCrypto.Encrypt(ObjsignupMDL.Password);
                LoginBAL objLoginBAL = new LoginBAL();
                Messages objMsg = objLoginBAL.signup(ObjsignupMDL);
                if (objMsg.Message_Id == 1)
                {
                    SendVerificationLinkEmail(ObjsignupMDL.EmailID, "", "VerifyAccount");

                    signupemailtomerchent(ObjsignupMDL);
                }
                else
                {
                    ViewBag.Message = objMsg.Message;
                    return View(ObjsignupMDL);
                }
            }
            else
            {
                return View(ObjsignupMDL);
            }
            TempData["Message"]
            = "Your request has been registered!";
            return RedirectToAction("signup");
        }

        [SkipCustomAuthenticationAttribute]
        public ActionResult ResetPassword(string id)
        {
            //Verify the reset password link
            //Find account associated with this link
            //redirect to reset password page
            Messages mess = new Messages();
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }

            mess = objlogin.InsertResetcode(id, "");
            if (mess.Message_Id != 0)
            {
                ResetPassword model = new ResetPassword();
                model.ResetCode = id;
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SkipCustomAuthenticationAttribute]
        public ActionResult ResetPassword(ResetPassword model)
        {
            var message = "";

            Messages objmsg = new Messages();
            if (ModelState.IsValid)
            {
                model.Password = ClsCrypto.Encrypt(model.Password);

                objmsg = objlogin.Updatepassword(model);
                if (objmsg.Message_Id != 0)
                {
                    ViewBag.Message = objmsg.Message;
                }

            }
            else
            {
                ViewBag.Message = "Something invalid";
                return View(model);
            }
            ViewBag.Message = message;
            return RedirectToAction("Login");
        }
    }
}