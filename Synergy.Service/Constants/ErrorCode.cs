using System;
using System.Collections.Generic;
using System.Text;

namespace Synergy.Service.Constants
{
  public static  class ErrorCode
    {
        public const string UNIQUE_IDENTITY_VIOLATION = "E01";
        public const string INTERNAL_SERVER_ERROR = "E02";
        public const string INVALID_CLIENT_REQUEST = "E03";
        public const string NOT_FOUND = "E04";
        public const string USER_NOT_VERIFIED = "E05";
        public const string FILE_FORMAT_NOT_SUPPORTED = "E06";
        public const string INVALID_ACCOUNT_CREDENTIALS = "E07";
        public const string UNAUTHORIZED_USER_ACCESS = "E07";
        public const string TRANSACTION_NOT_FOUND = "E08";
        public const string UNAUTHORIZED_CLIENT_CREDENTIALS = "E09";
        public const string INVALID_OTP = "E10";
        public const string EXPIRED_OTP = "E11";
        public const string USER_WITH_CREDENTIALS_ALREADY_EXIST = "E12";
        public const string DOCUMENT_ALREADY_APPROVED = "E13";
        public const string NO_UPLOADED_FILE_CONTENT = "E14";
        public const string FAILED_FILE_UPLOAD_TO_LOCAL_DIRECTORY = "E15";
        public const string INSUFFICIENT_BALANCE = "E16";
        public const string FAILED_EMAIL_VERIFICATION = "E17";
        public const string FAILED_PHONE_NUMBER_VERIFICATION = "E18";
    }
}
