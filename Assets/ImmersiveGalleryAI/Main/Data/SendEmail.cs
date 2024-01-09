using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using UnityEngine;

namespace ImmersiveGalleryAI.Main.Data
{
    public static class SendEmail
    {
        private static string Email { get; set; } = "fidgetlandprod@gmail.com"; // todo: change it
        private static string Password { get; set; } = "zygmqjopfuyuxodr"; // todo: change it

        public class EmailData
        {
            public string Email;
            public string FilePath;
        }

        public static void Send(ImageData imageData)
        {
            MailMessage mail = new MailMessage
            {
                From = new MailAddress("fidget@land.com", "FidgetLand"),
                To = {"agfidget@gmail.com"},
                Subject = "Image from Immersive Gallery AI",
                Body = GetBodyMessage(),
                IsBodyHtml = true,
                Attachments = {new Attachment(imageData.FilePath, "image/jpg")}
            };
            try
            {
                using SmtpClient smtpServer = new SmtpClient("smtp.gmail.com", 25);
                ICredentialsByHost credentialsByHost = new NetworkCredential(Email, Password);
                smtpServer.Credentials = credentialsByHost;
                smtpServer.EnableSsl = true;
                smtpServer.Send(mail);
                smtpServer.Dispose();
                Debug.Log($"Message sent!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Will work for future. Include logo or something like this
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static AlternateView GetEmbeddedImage(String filePath)
        {
            LinkedResource res = new LinkedResource(filePath);
            res.ContentId = Guid.NewGuid().ToString();
            string htmlBody = @"<img src='cid:" + res.ContentId + @"'/>";
            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(res);
            return alternateView;
        }

        private static string GetBodyMessage()
        {
            string bodyMessage = "Hello!<br/>We would like to share you an image created in the application: Immersive Gallery AI.<br/>You can find it in the attachments.";
            return bodyMessage;
        }
    }
}