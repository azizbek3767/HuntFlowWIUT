using System.Text.Json.Serialization;

namespace HuntFlowWIUT.Web.Models
{
    public class TokenRefreshRequest
    {
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
