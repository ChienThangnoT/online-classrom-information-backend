using AutoMapper.Internal;
using LMSystem.Repository.Helpers;
using LMSystem.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Services.Services
{
    public class MailService(IOptions<EmailConfig> emailConfig) : IMailService
    {
        readonly EmailConfig _emailConfig = emailConfig.Value;

        public async Task SendEmailAsync(EmailRequest emailRequest)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient(_emailConfig.Mail, _emailConfig.Port);
                smtpClient.Credentials = new NetworkCredential(_emailConfig.Mail, _emailConfig.Password);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.EnableSsl = true;

                MailMessage msg = new MailMessage();

                msg.From = new MailAddress(_emailConfig.Mail);
                msg.To.Add(emailRequest.To);
                msg.Subject = emailRequest.Subject;
                msg.Body = emailRequest.Content;

                if (emailRequest.AttachmentFilePaths.Length > 0)
                {
                    foreach (var path in emailRequest.AttachmentFilePaths)
                    {
                        Attachment attachment = new Attachment(path);
                        msg.Attachments.Add(attachment);
                    }
                }
                await smtpClient.SendMailAsync(msg);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task SendConFirmEmailAsync(EmailRequest emailRequest)
        {
            //try
            //{
                SmtpClient smtpClient = new SmtpClient(_emailConfig.Host, _emailConfig.Port);
                smtpClient.Credentials = new NetworkCredential(_emailConfig.Mail, _emailConfig.Password);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.EnableSsl = true;

                MailMessage msg = new MailMessage();

                msg.From = new MailAddress(_emailConfig.Mail);
                msg.To.Add(emailRequest.To);
                msg.IsBodyHtml = true;  // body is html, decode to html
                msg.Subject = emailRequest.Subject;
                msg.Body = emailRequest.Content;

                if (emailRequest.AttachmentFilePaths.Length > 0)
                {
                    foreach (var path in emailRequest.AttachmentFilePaths)
                    {
                        Attachment attachment = new Attachment(path);
                        msg.Attachments.Add(attachment);
                    }
                }
                await smtpClient.SendMailAsync(msg);
            }
            //catch (Exception)
            //{
            //    throw;
            //}
        }
    }

