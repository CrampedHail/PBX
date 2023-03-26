using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace PBX.Controllers
{
    public class EmailSender
    {
        public void SendEmail(string email, string subject, string body)
        {
            var fromAddress = new MailAddress("pbx.serwis.ogloszeniowy@gmail.com", "Serwis Ogłoszeniowy PBX");
            var toAddress = new MailAddress(email);
            string styles =
                "<style>" +
                "@import url('https://fonts.googleapis.com/css2?family=Poppins&display=swap');" +
                "div.htmlMessage{" +
                "font-family: 'Poppins', sans-serif;" +
                "text-align:center;" +
                "width:90%;" +
                "max-width: 800px;" +
                "padding: 7px 15px;" +
                "background-color: #4484ff;" +
                "border-radius:10px;" +
                "color:white;" +
                "}" +
                "::selection {color: black;background: white;}" +
                "</style>";
            string htmlBody = styles +
                "<center>" +
                "<div align=\" center \" class=\" htmlMessage \">" +
                "<h1>" + subject + "</h1>" +
                "</br></br>" +
                "<p>" + body + "</p>" +
                "</br></br>" +
                "<h3>Pozdrawiamy, zespół PBX</h3>" +
                "</center>";
            var smtp = new SmtpClient
            {
                Host = "sandbox.smtp.mailtrap.io",
                Port = 2525,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("d086a8dbd02ca4", "651d0e131370da")
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
        }
    }
}