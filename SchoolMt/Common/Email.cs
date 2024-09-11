using BAL.Common;
using MDL;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace SchoolMt.Common
{
    public class Email
    {
        public static async Task SendMailFromSendGrid(MailMessage emailDetails)
        {
            try
            {
                //"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";
                var apiKey = ConfigurationManager.AppSettings["Sendgridkey"];

                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(emailDetails.From.ToString());
                var subject = emailDetails.Subject;
                var to = new EmailAddress(emailDetails.To.ToString());
                var plainTextContent = "";
                var htmlContent = emailDetails.Body;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
            }
            catch(Exception Ex)
            {
                var objBase = System.Reflection.MethodBase.GetCurrentMethod();
                ErrorlogBal.SetError(Ex, objBase, emailDetails.Subject, "Error From Email Sending");
            }

        }
    }
}