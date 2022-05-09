using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace exorderproject_api.Helpers
{
    public class MailHelper
    {
        public bool Send(string MailAddress, string MailSubject, string MailContent)
        {
            try
            {
                NetworkCredential cred = new NetworkCredential("exorder@gmail.com", "Ee1234++");
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("exorder@gmail.com", "exorder");
                mail.To.Add(MailAddress);
                mail.Subject = MailSubject;
                mail.IsBodyHtml = true;
                mail.Body = MailContent;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Credentials = cred;
                smtp.Send(mail);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}