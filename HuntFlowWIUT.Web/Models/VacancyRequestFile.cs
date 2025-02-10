using System.Text.Json.Serialization;

namespace HuntFlowWIUT.Web.Models
{
    public class VacancyRequestFile
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("content_type")]
        public string ContentType { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
