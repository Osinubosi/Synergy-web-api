namespace Synergy.Domain.ServiceModel
{
   public class EmailConfigurationProvider
    {
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPassword { get; set; }
        public bool SmtpEnableSsl { get; set; }
        public string FromEmail { get; set; }
        public string SenderName { get; set; }
        public string CopyAddresses { get; set; }
        public string ApiKey { get; set; }
    }
}
