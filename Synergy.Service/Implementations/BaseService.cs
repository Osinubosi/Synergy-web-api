using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NLog;
using Synergy.Domain.Interfaces;
using Synergy.Repository.Interfaces;

namespace Synergy.Service.Implementations
{
   public class BaseService
    {
        public IHostingEnvironment Evn;
        public readonly ICryptographyService CryptographyService;
        public readonly IEmailService EmailService;
       // public readonly ISMSService SmsService;
        public static Logger Log = LogManager.GetCurrentClassLogger();
        public const int SqlServerViolationOfUniqueIndex = 2601;
        public const int SqlServerViolationOfUniqueConstraint = 2627;
        public readonly IConfiguration Config;
        public readonly IDbContext DbContext;
        public string Uploadextension;
        public string CurrencyFolder;
        public int OtpTimerValue;
        public readonly string WebUrl = "BaseEndpoint";
        public readonly string LogoPath;
        public readonly string ClientKeyPrefix;
        public readonly string KycNotificationResponseEmail;
        public string ContentPath { get; set; }

        //private readonly IOptions<ApplicationResourcesProvider> _applicationResourcesProvider;
        // private readonly IOptions<CloudinaryAccount> _cloudinaryAccount;
        // public ApplicationResourcesProvider ApplicationResourcesProvider { get; set; }
        // public CloudinaryAccount CloudinaryConfiguration { get; set; }

        public BaseService(IHostingEnvironment evn, ICryptographyService cryptographyService, IEmailService emailService, IConfiguration config, IDbContext dbContext)
        {
            Evn = evn;
            ContentPath = Evn.ContentRootPath;
            this.CryptographyService = cryptographyService;
            this.EmailService = emailService;
            Config = config;
            DbContext = dbContext;
            Uploadextension = config["UploadExtensions"];
            CurrencyFolder = config["UploadFolder:Currency"];
            OtpTimerValue = int.Parse(config["OtpSetting:OtpExpiry"]);
            LogoPath = config["cadawadaLogo:LogoPath"];
            ClientKeyPrefix = config["ClientCredentials:ClientKeyPrefix"];
            KycNotificationResponseEmail = config["KycNotification:To"];

           // _applicationResourcesProvider = applicationResourcesProvider;
          //  ApplicationResourcesProvider = _applicationResourcesProvider.Value;

           // _cloudinaryAccount = cloudinaryAccount;
           // CloudinaryConfiguration = _cloudinaryAccount.Value;
        }

        public string GetBaseUrl(string key)
        {
            if (string.IsNullOrEmpty(key))
                return string.Empty;

            return Config.GetValue<string>($"Synergy:{key}");
        }
    }
}
