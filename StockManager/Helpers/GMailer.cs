using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace StockManager.Helpers
{
    public class GMailer
    {
        static string email_from = ConfigurationManager.AppSettings["SMTP_EMAIL"].ToString();
        static string from_password = ConfigurationManager.AppSettings["SMTP_PASSWORD"].ToString();

        public static void Send(string to, string subject, string body)
        {
            MailAddress fromAddress = new MailAddress(email_from);
            MailAddress toAddress = new MailAddress(to);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, from_password)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
        }

    }
}