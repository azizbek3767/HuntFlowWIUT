using System.Text.Json.Serialization;

namespace HuntFlowWIUT.Web.Models
{
    public class VacanciesResponse
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("total_items")]
        public int TotalItems { get; set; }

        [JsonPropertyName("items")]
        public List<Vacancy> Items { get; set; }
    }
}
