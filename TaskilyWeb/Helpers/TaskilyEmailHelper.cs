using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace TaskilyWeb.Helpers
{
    public class TaskilyEmailHelper
    {


        public void SendWelcomeEmail(string to)
        {
            SendEmail(to, "welcome to taskily", "~/content/emails/welcome.html", new Dictionary<string, string>());
        }

        public void SendUpgradeEmail(string to)
        {
            SendEmail(to, "Tanks for upgrading taskily", "~/content/emails/upgrade.html", new Dictionary<string, string>());
        }

        public void SendExpireEmail(string to, DateTime expiry)
        {
            var replacements = new Dictionary<string, string>();
            replacements.Add("{{expire}}", expiry.ToString("dddd dd MMM yyyy"));
            SendEmail(to, "Your taskily is about expire", "~/content/emails/expire.html", replacements);
        }

        private void SendEmail(string to, string subject, string file, IDictionary<string, string>replacements)
        {
            // replacements everytime...
            replacements.Add("{{to}}", to);
            replacements.Add("{{subject}}", subject);
            
            // name of the user 
            replacements.Add("{{user}}", HttpContext.Current.User.Identity.Name);

            using (SmtpClient client = new SmtpClient())
            {
                var fromAddress = new MailAddress("taskily@jumoo.co.uk");
                var toAddress = new MailAddress(to);

                var eMail = new MailMessage(fromAddress, toAddress);
                eMail.Subject = subject;

                var filePath = HttpContext.Current.Server.MapPath(file);

                if (File.Exists(filePath))
                {
                    var msg = File.ReadAllText(filePath);
                    eMail.IsBodyHtml = true;
                    eMail.Body = GenerateBody(msg, replacements);

                    client.Send(eMail);
                }
                else
                {
                    throw new FileNotFoundException(filePath);
                }
            }
        }

        private string GenerateBody(string message, IDictionary<string, string> replacements)
        {
            foreach(var replacement in replacements)
            {
                if (message.Contains(replacement.Key))
                {
                    message = message.Replace(replacement.Key, replacement.Value);
                }
            }

            return message; 
        }
    }
}