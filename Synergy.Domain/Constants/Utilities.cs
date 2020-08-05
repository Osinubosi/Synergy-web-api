using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using SendGrid.Helpers.Mail;
using Synergy.Domain.Interfaces;
using Synergy.Domain.ServiceModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Synergy.Domain.Constants
{
    public static class Utilities
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public static string GetSettingConfig(string key, string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                    return string.Empty;
                var fullPath = Path.Combine(path, "TempFolder", "Settings.json");
                using (StreamReader file = File.OpenText(fullPath))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject jsonObject = (JObject)JToken.ReadFrom(reader);
                    var response = jsonObject["Response"] as JObject;
                    if (response != null)
                    {
                        var value = response.Value<string>(key);
                        return value;
                    }
                }


            }
            catch (Exception ex)
            {
                Log.Error(ex, "GetResponseMessage");
                return string.Empty;
            }

            return "";
        }

        public static async Task<string> GenerateJwt(ClaimsIdentity identity, IJwtFactory jwtFactory, string userName, JwtIssuerOptions jwtOptions, JsonSerializerSettings serializerSettings)
        {
            var response = new
            {
                id = identity.Claims.Single(c => c.Type == "id").Value,
                auth_token = await jwtFactory.GenerateEncodedToken(userName, identity),
                expires_in = (int)jwtOptions.ValidFor.TotalSeconds
            };

            return JsonConvert.SerializeObject(response, serializerSettings);
        }

        public static string FileDirectoryUniqueName(string realName)
        {
            if (!string.IsNullOrEmpty(realName))
            {
                if (realName.Contains('.'))
                {
                    realName = RemoveExtensionFromFileName(realName);
                }
                return string.Concat(realName, "_", DateTime.UtcNow.Ticks);
            }
            return null;
        }

        public static string RemoveExtensionFromFileName(string fileName)
        {
            string fileExtension = "";
            if (!string.IsNullOrEmpty(fileName) && fileName.Contains('.'))
            {
                var splittedFileArray = fileName.Split('.');
                for (int i = 0; i < (splittedFileArray.Length - 1); i++)
                {
                    fileExtension += splittedFileArray[i];
                }
            }
            return fileExtension;
        }

        public static string GetFileExtensionFromFileName(string fileName)
        {
            string fileExtension = "";
            if (!string.IsNullOrEmpty(fileName) && fileName.Contains('.'))
            {
                var splittedFileArray = fileName.Split('.');
                fileExtension = splittedFileArray[splittedFileArray.Length - 1];
            }
            return fileExtension;
        }

        public static List<string> GetMimeType(List<string> fileExtensions)
        {
            if (fileExtensions == null)
            {
                return null;
            }

            List<string> mimeTypes = new List<string>();
            foreach (var fileExtension in fileExtensions)
            {
                string extension = (fileExtension.Contains('.')) ? fileExtension : string.Concat('.', fileExtension);
                string fileName = string.Concat(DateTime.Now.Ticks, extension);
                string mimeType = MimeTypes.GetMimeType(fileName);
                mimeTypes.Add(mimeType);
            }
            return mimeTypes;
        }

        public static string GetMimeType(string fileExtension)
        {
            if (string.IsNullOrEmpty(fileExtension))
            {
                return null;
            }

            string extension = (fileExtension.Contains('.')) ? fileExtension : string.Concat('.', fileExtension);
            string fileName = string.Concat(DateTime.Now.Ticks, extension);
            string mimeType = MimeTypes.GetMimeType(fileName);
            return mimeType;
        }

        public static List<EmailAddress> GetCopyEmailAddresses(string copyAddresses)
        {
            if (string.IsNullOrEmpty(copyAddresses))
                return new List<EmailAddress>();

            List<EmailAddress> formattedCopyAddresses = new List<EmailAddress>();
            string[] addresses = copyAddresses.Split(',');
            foreach (var address in addresses)
            {
                if (!string.IsNullOrEmpty(address))
                {
                    formattedCopyAddresses.Add(new EmailAddress(address));
                }
            }
            return formattedCopyAddresses;
        }
    }
}
