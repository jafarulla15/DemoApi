using DemoProject.Utilities.Mail;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DemoProject.Utilities
{
    public class MailService : IMailService
    {
        private readonly MailServerConfig _mailServerConfig;
        public MailService(IOptions<MailServerConfig> MailServerConfigs)
        {
            _mailServerConfig = MailServerConfigs.Value;
        }

        /// <summary>
        /// Send mail method
        /// </summary>
        /// <param name="mailRequest"></param>
        /// <returns></returns>
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(_mailServerConfig.Mail, _mailServerConfig.DisplayName);
                message.To.Add(new MailAddress(mailRequest.ToEmail));
                message.Subject = mailRequest.Subject;
                if (mailRequest.Attachments != null && mailRequest.Attachments.Count > 0)
                {
                    foreach (var attachment in mailRequest.Attachments)
                    {
                        using (var ms = new MemoryStream())
                        {
                            Attachment att = new Attachment(new MemoryStream(attachment.fileBytes), attachment.fileName);
                            message.Attachments.Add(att);
                        }
                    }
                }

                message.IsBodyHtml = true;
                message.Body = mailRequest.Body;
                smtp.Port = _mailServerConfig.Port;
                smtp.Host = _mailServerConfig.Host;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(_mailServerConfig.Mail, _mailServerConfig.Password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                // TODO: write into FIle
            }
        }

        /// <summary>
        /// Send QR code over mail
        /// </summary>
        /// <param name="mailRequest"></param>
        /// <param name="qrCode"></param>
        /// <returns></returns>
        public async Task SendEmailWithQRCodeAsync(MailRequest mailRequest, Byte[] qrCode)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(_mailServerConfig.Mail, _mailServerConfig.DisplayName);
                message.To.Add(new MailAddress(mailRequest.ToEmail));
                message.Subject = mailRequest.Subject;
                if (mailRequest.Attachments != null && mailRequest.Attachments.Count > 0)
                {
                    foreach (var attachment in mailRequest.Attachments)
                    {
                        using (var ms = new MemoryStream())
                        {
                            Attachment att = new Attachment(new MemoryStream(attachment.fileBytes), attachment.fileName);
                            message.Attachments.Add(att);
                        }
                    }
                }
                //body

                string htmlMessage = @"<html>
                         <body>
                            <p>" + mailRequest.Body + @"</p>
                         <img src='cid:EmbeddedContent_1' width='200' height='200' />
                         </body>
                         </html>";


                // Create the HTML view
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                                                             htmlMessage,
                                                             Encoding.UTF8,
                                                             MediaTypeNames.Text.Html);
                // Create a plain text message for client that don't support HTML
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                                                            Regex.Replace(htmlMessage,
                                                                          "<[^>]+?>",
                                                                          string.Empty),
                                                            Encoding.UTF8,
                                                            MediaTypeNames.Text.Plain);
                string mediaType = MediaTypeNames.Image.Jpeg;
                LinkedResource img = new LinkedResource(new MemoryStream(qrCode), new ContentType() { MediaType = mediaType, Name = "qr-code.jpeg" });
                // Make sure you set all these values!!!
                img.ContentId = "EmbeddedContent_1";
                img.ContentType.MediaType = mediaType;
                img.TransferEncoding = TransferEncoding.Base64;
                img.ContentType.Name = img.ContentId;
                img.ContentLink = new Uri("cid:" + img.ContentId);
                htmlView.LinkedResources.Add(img);

                message.AlternateViews.Add(plainView);
                message.AlternateViews.Add(htmlView);

                message.IsBodyHtml = true;
                //message.Body = mailRequest.Body;
                smtp.Port = _mailServerConfig.Port;
                smtp.Host = _mailServerConfig.Host;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(_mailServerConfig.Mail, _mailServerConfig.Password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                //TODO: write into file
            }
        }
    }

    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
        Task SendEmailWithQRCodeAsync(MailRequest mailRequest, Byte[] qrCode);
    }
}
