using Newtonsoft.Json;

namespace Nop.Plugin.Payments.PayU.Models.Requests
{
    public class AuthorizationRequest
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty(PropertyName = "grant_type")]
        public string GrantType { get; set; }
    }
}