using System.Text.Json.Serialization;

namespace HuntFlowWIUT.Web.Models
{
    public class UserInfo
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}
