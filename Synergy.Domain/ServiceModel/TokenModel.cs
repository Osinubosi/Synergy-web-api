using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Synergy.Domain.ServiceModel
{
  public  class TokenModel
    {
        [JsonProperty("auth_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("id")]
        [JsonIgnore]
        public string UserId { get; set; }

        [JsonProperty("user_name")]
        [JsonIgnore]
        public string UserName { get; set; }
    }
}
