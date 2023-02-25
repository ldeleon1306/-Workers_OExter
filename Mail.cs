using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Workers.Mail
{
    public class Mail
    {
        private readonly IConfiguration _configuration;
        private string _mailTo { get; set; }
        private string _mailFrom { get; set; }
        private string _smtpServer { get; set; }

        public Mail(IConfiguration configuration)
        {
            _configuration = configuration;
            _mailTo = _configuration["MailSmtp:mailTo"];
            _mailFrom = _configuration["MailSmtp:mailFrom"];
            _smtpServer = _configuration["MailSmtp:smtp"];
        }

        public void SendEmail(string pathArchivo, string cliente)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(_mailFrom);
                var multiple = _mailTo.Split(';');
                foreach (var to in multiple)
                {
                    if (to != string.Empty)
                        mail.To.Add(to);
                }
                //System.Net.Mail.Attachment attachment;
                //attachment = new System.Net.Mail.Attachment(pathArchivo);
                //mail.Attachments.Add(attachment);
                //if (!String.IsNullOrEmpty(txterror))
                //{
                //  attachment = new System.Net.Mail.Attachment(txterror);
                //  mail.Attachments.Add(attachment);
                //}

                string subject = string.Format($"Prueba {cliente} Mail");
                string bodyMsg = string.Format($"Se procesó un mongo {cliente}");
                mail.Subject = subject;
                mail.Body = bodyMsg;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient(_smtpServer);
                smtp.EnableSsl = false;
                smtp.Port = 25;
                smtp.UseDefaultCredentials = true;
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Se produjo una excepción en el metodo SendEmail: ", ex.Message);
            }
        }
    }
}
