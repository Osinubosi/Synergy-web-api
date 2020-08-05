using System;
using System.Collections.Generic;
using System.Text;

namespace Synergy.Service.Constants
{
    public static class SuccessCode
    {
        public const string DEFAULT_SUCCESS_CODE = "00";
        public const string DEFAULT_UPDATE_CODE = "S01";
        public const string AWAITING_PHONE_VERIFICATION = "S02";
        public const string AWAITING_EMAIL_VERIFICATION = "S03";
        public const string AWAITING_BILLING_DETAILS_VERIFICATION = "S04";
        public const string AWAINTING_PIN_VERIFICATION = "S05";
        public const string AWAITING_3DSESURE_VERIFCATION = "S06";
        public const string CUSTOMER_ACCOUNT_DELETED = "S07";
        public const string DEFAULT_SUCCESS_WITHOUT_DATA = "S08";
        public const string AWAITING_OTP_VERIFICATION = "S09";

        public const string CUSTOMER_EXIST_WALLET_NOT_FOUND = "S09";
        public const string DETAILS_ALREADY_EXIST = "S10";
        public const string AWAITING_PASSWORD_CREATION = "S11";
    }
}
