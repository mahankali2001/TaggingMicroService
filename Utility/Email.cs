using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using Core.Logger;
using Utility;

namespace Utility
{
    public class Email : IDisposable
    {
        private Persistence.Entities.IEmailAttributes _emailAttributes;
        private static readonly ILogger Logger = LogManager.GetLogger("< UTILITY - MAIL >>");

        public Email(Persistence.Entities.IEmailAttributes emailAttributes)
        {
            _emailAttributes = emailAttributes;
        }

        public string SendEmail()
        {
            try
            {
                ValidateEmailAttributes();
                var message = new MailMessage(_emailAttributes.FromAddress, _emailAttributes.ToAddress)
                {
                    Subject = _emailAttributes.Subject
                };
                if (!string.IsNullOrEmpty(_emailAttributes.CcAddress))
                    message.CC.Add(_emailAttributes.CcAddress);
                if (!string.IsNullOrEmpty(_emailAttributes.BccAddress))
                    message.Bcc.Add(_emailAttributes.BccAddress);
                message.IsBodyHtml = true;
                message.BodyEncoding = Encoding.UTF8;

                string sEmailBody = EmailHTML();
                ReplaceParamsForHTML(ref sEmailBody, _emailAttributes.TemplateNameValues);
                message.Body = sEmailBody;
                var mailClient = new SmtpClient(_emailAttributes.SMTPServer) { UseDefaultCredentials = true };
                var tm =  System.Configuration.ConfigurationManager.AppSettings["DEBUG"];
                if (String.IsNullOrEmpty(tm) || (tm.ToUpper().Trim() == "FALSE"))
                {
                    mailClient.Send(message);
                }
                else
                    Logger.Error("Configuration -DEBUG set to TRUE - TestingEmail - SEND EMAIL DISABLED ... email would not be sent ");
                return message.Body;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return string.Format("Error while sending email <<{0}>>", ex);
            }
        }

        private void ValidateEmailAttributes()
        {
            var sb = new StringBuilder();
            if (string.IsNullOrEmpty(_emailAttributes.FromAddress))
                sb.Append(" Required <FromAddress> - Not Available");
            if (string.IsNullOrEmpty(_emailAttributes.ToAddress))
                sb.Append(" Required <ToaAddress> - Not Available");
            if (string.IsNullOrEmpty(_emailAttributes.Subject))
                sb.Append(" Required <Subject> - Not Available");
            //if (string.IsNullOrEmpty(_emailAttributes.TemplateFileLocation))
            //    sb.Append(" Required <TemplateFileLocation> - Not Available");

            string s = sb.ToString();
            if (!String.IsNullOrEmpty(s))
            {
                throw new ArgumentException("Email Feature Failed : " + s);
            }
        }

        private string EmailHTML()
        {

            if (!String.IsNullOrEmpty(_emailAttributes.Template))
                return _emailAttributes.Template;
            else
            {
                string file = _emailAttributes.TemplateFileLocation;

                if (String.IsNullOrEmpty(file))
                    throw new ArgumentException("Email Template should be configured either in the DB or the file location needs to be provided");

                var fi = new FileInfo(file);
                var emailBody = new StringBuilder("");
                if (System.IO.File.Exists(file))
                {
                    StreamReader sr = System.IO.File.OpenText(file);
                    emailBody.Append(sr.ReadToEnd());
                    sr.Close();
                }
                return emailBody.ToString();
            }
        }

        private string ReplaceParamsForHTML(ref string emailBody, System.Collections.Specialized.NameValueCollection emailParams)
        {
            emailBody = emailParams.AllKeys.Aggregate(emailBody, (current, key) =>
            {
                var strings = emailParams.GetValues(key);
                return strings != null ? current.Replace("{" + key + "}", strings[0]) : null;
            });
            return emailBody;
        }

        public void Dispose()
        {
            _emailAttributes = null;
        }
    }
}
