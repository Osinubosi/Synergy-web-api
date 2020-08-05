using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Synergy.Service.Helpers
{
   public class EmailFormatter
    {
        private const string EmailFolderName = "EmailTemplates";
        private const string EmailConfirmation = "EmailConfirmation.html";


        public static string ConfirmEmail(string logo, string url, string year, string rootPath)
        {
            string templatePath = CombinePath(rootPath, EmailConfirmation);
            string emailContent = "";
            using (StreamReader sr = new StreamReader(templatePath))
            {
                emailContent = sr.ReadToEnd();
            }
            emailContent = emailContent.Replace("{Logo}", logo);
            emailContent = emailContent.Replace("{url}", url);
            emailContent = emailContent.Replace("{year}", year);
            return emailContent;
        }

        private static string CombinePath(string rootPath, string templateName)
        {
            return Path.Combine(rootPath, EmailFolderName, templateName);
        }
    }
}
