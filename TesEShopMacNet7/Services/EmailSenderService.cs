using System;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Text;
using System.IO;
using System.Net;

namespace TestEShopMacNet7.Services
{
    public class EmailSenderService
    {
        public EmailSenderService()
		{}

        public void SendTo(string mailBody, string mailSubject, string receiver)
        {
            string from = "TestEShopMacNet7@gmail.com"; //From address    
            MailMessage message = new MailMessage(from, "jan.benda97@seznam.cz");//receiver);

            message.Subject = mailSubject;
            message.Body = mailBody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
            System.Net.NetworkCredential basicCredential1 = new
            System.Net.NetworkCredential("jan.benda54@gmail.com", "kahe dart sfjs hhri");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;

            try
            {
                client.Send(message);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

