using System;
using System.Net;
using System.Net.Mail;
using UnityEngine;

namespace ImmersiveGalleryAI.Common.Backend
{
    public class EmailSender
    {
        private static string Subject = "[ImmersiveGalleryAI] Please upgrade my credits!";
        private readonly string CredentialName;
        private readonly string CredentialPassword;
        private readonly string Provider;

        public EmailSender(string credentialName, string credentialPassword, string provider)
        {
            CredentialName = credentialName;
            CredentialPassword = credentialPassword;
            Provider = provider;
        }

        public void SendEmail(string senderEmail, string senderName, string adminEmail)
        {
            MailMessage mailMessage = new MailMessage {From = new MailAddress(senderEmail)};
            mailMessage.To.Add(adminEmail);
            mailMessage.Subject = Subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = GetEmailText(senderName, senderEmail);

            SmtpClient smtpServer = new SmtpClient(Provider)
            {
                Port = 587,
                Credentials = new NetworkCredential(CredentialName, CredentialPassword) as ICredentialsByHost,
                EnableSsl = true
            };

            try
            {
                smtpServer.Send(mailMessage);
            }
            catch (Exception e)
            {
                Debug.LogError($"Can't send email, because: {e}");
                return;
            }

            Debug.Log($"Message sent!");
        }

        private string GetEmailText(string userName, string senderEmail)
        {
            return $@"
            <html>
                <body style='font-family: Arial, sans-serif; color: #333;'>
                    <p><strong style='color: #2E86C1;'>Hi Admin!</strong></p>
                    <p>This is <strong style='color: #E74C3C;'>{userName}</strong> ({senderEmail}).</p>
                    <p>Please upgrade my credits in the <strong style='color: #27AE60;'>{Application.productName}</strong> application.</p>
                </body>
            </html>";
        }
    }
}