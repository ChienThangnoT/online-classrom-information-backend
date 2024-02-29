using AutoMapper.Internal;
using LMSystem.Repository.Helpers;
using LMSystem.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Services.Services
{
    public class MailService(IOptions<EmailConfig> emailConfig) : IMailService
    {
        readonly EmailConfig _emailConfig = emailConfig.Value;

        public async Task SendEmailAsync(EmailRequest emailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_emailConfig.Mail);
            email.To.Add(MailboxAddress.Parse(emailRequest.To));
            email.Subject = emailRequest.Subject;
            var builder = new BodyBuilder();

            //Attachment
            if (emailRequest.AttachmentFilePaths != null)
            {
                byte[] fileBytes;
                foreach (var file in emailRequest.AttachmentFilePaths)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }

            //Body
            builder.HtmlBody = emailRequest.Content;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_emailConfig.Host, _emailConfig.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailConfig.Mail, _emailConfig.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
        public async Task SendConFirmEmailAsync(EmailRequest emailRequest)
        {
            string MailText = emailRequest.Content;
            MailText = MailText.Replace("[ConfirmLink]", emailRequest.Content);

            //Setup email
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_emailConfig.Mail);
            email.To.Add(MailboxAddress.Parse(emailRequest.To));
            email.Subject = emailRequest.Subject;
            var builder = new BodyBuilder();

            //Body contain the confirm link
            builder.HtmlBody = MailText; //Using Html file edited instead of request.body
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_emailConfig.Host, _emailConfig.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailConfig.Mail, _emailConfig.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);


            //    SmtpClient smtpClient = new SmtpClient(_emailConfig.Host, _emailConfig.Port);
            //    smtpClient.Credentials = new NetworkCredential(_emailConfig.Mail, _emailConfig.Password);
            //    smtpClient.UseDefaultCredentials = false;
            //    smtpClient.EnableSsl = true;

            //    MailMessage msg = new MailMessage();

            //    msg.From = new MailAddress(_emailConfig.Mail);
            //    msg.To.Add(emailRequest.To);
            //    msg.IsBodyHtml = true;  // body is html, decode to html
            //    msg.Subject = emailRequest.Subject;
            //    msg.Body = emailRequest.Content;

            //    if (emailRequest.AttachmentFilePaths.Length > 0)
            //    {
            //        foreach (var path in emailRequest.AttachmentFilePaths)
            //        {
            //            Attachment attachment = new Attachment(path);
            //            msg.Attachments.Add(attachment);
            //        }
            //    }
            //    await smtpClient.SendMailAsync(msg);
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }
    }
}

