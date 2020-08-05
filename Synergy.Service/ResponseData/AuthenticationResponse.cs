using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Synergy.Domain.ServiceModel;

namespace Synergy.Service.ResponseData
{
   public class AuthenticationResponse
    {
        public UserDetail UserDetail { get; set; }
        public TokenModel Token { get; set; }
        public CountryDetailLite CountryDetailLite { get; set; }
    }

   public class CountryDetailLite
   {
       public int CountryId { get; set; }
       public string CountryCode { get; set; }
       public string CountryName { get; set; }
       public string DailingCode { get; set; }
   }

   public class UserDetail
   {
       [JsonIgnore]
       public string UserId { get; set; }
       public string FirstName { get; set; }
       public string LastName { get; set; }
       public string EmailAddress { get; set; }
       public string Status { get; set; }
       public string Selfie { get; set; }
   }
}
