using System.Text.Json.Serialization;

namespace HuntFlowWIUT.Web.Models
{
    public class VacancyRequestState
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("reason")]
        public string Reason { get; set; }

        [JsonPropertyName("order")]
        public int Order { get; set; }

        [JsonPropertyName("changed")]
        public DateTimeOffset Changed { get; set; }
    }
}
