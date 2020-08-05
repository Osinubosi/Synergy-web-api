using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Synergy.Domain.Constants;
using Synergy.Domain.Interfaces;
using Synergy.Domain.ServiceModel;

namespace Synergy.Domain.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly IOptions<EmailConfigurationProvider> _emailConfigurationProvider;

        public EmailService(IOptions<EmailConfigurationProvider> emailConfigurationProvider)
        {
            _emailConfigurationProvider = emailConfigurationProvider;
        }

        public void SendEmail(EmailRequest request, string t)
        {
            try
            {
                var configReader = _emailConfigurationProvider.Value;
                var client = new SendGridClient(configReader.ApiKey.Trim());
                var from = new EmailAddress(configReader.FromEmail.Trim(), configReader.SenderName);
                var subject = request.Subject;

                List<EmailAddress> to = new List<EmailAddress>
                {
                    new EmailAddress(request.To.Trim())
                };

                if (!string.IsNullOrEmpty(configReader.CopyAddresses.Trim()))
                {
                    var addresses = Utilities.GetCopyEmailAddresses(configReader.CopyAddresses);
                    if (addresses.Count > 0)
                    {
                        to.AddRange(addresses);
                    }
                }
                var htmlContent = request.Body;
                var message = MailHelper.CreateSingleEmailToMultipleRecipients(from, to, subject, "", htmlContent);
                client.SendEmailAsync(message);
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception)
            {

            }

        }

        public async Task<int> SendEmail(EmailRequest request)
        {
            try
            {
                var configReader = _emailConfigurationProvider.Value;
                var client = new SendGridClient(configReader.ApiKey.Trim());
                var from = new EmailAddress(configReader.FromEmail.Trim(), configReader.SenderName);
                var subject = request.Subject;

                //List<EmailAddress> to = new List<EmailAddress>
                //{
                //    new EmailAddress(request.To.Trim())
                //};

                //if (!string.IsNullOrEmpty(configReader.CopyAddresses.Trim()))
                //{
                //    var addresses = Utilities.GetCopyEmailAddresses(configReader.CopyAddresses);
                //    if (addresses.Count > 0)
                //    {
                //        to.AddRange(addresses);
                //    }
                //}

                var to = new EmailAddress
                {
                    Email = request.To.Trim()
                };
                var htmlContent = request.Body;
                var message = MailHelper.CreateSingleEmail(from,to, request.Subject, "", htmlContent);
                var sendResponse = await client.SendEmailAsync(message);

                HttpContent body = sendResponse.Body;

                if (sendResponse.StatusCode == HttpStatusCode.OK)
                    return 1;
                else
                    return 0;

            }
            catch (Exception ex)
            {
                return 0;
                // ignored
            }
        }
    }
}
