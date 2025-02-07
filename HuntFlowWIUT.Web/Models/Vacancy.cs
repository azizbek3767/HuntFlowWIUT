using System.Text.Json.Serialization;

namespace HuntFlowWIUT.Web.Models
{
    public class Vacancy
    {
        [JsonPropertyName("account_division")]
        public int? AccountDivision { get; set; }

        [JsonPropertyName("account_region")]
        public int? AccountRegion { get; set; }

        [JsonPropertyName("position")]
        public string Position { get; set; }

        [JsonPropertyName("company")]
        public string Company { get; set; }

        [JsonPropertyName("money")]
        public string Money { get; set; }

        [JsonPropertyName("priority")]
        public int? Priority { get; set; }

        [JsonPropertyName("hidden")]
        public bool Hidden { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("created")]
        public DateTimeOffset Created { get; set; }

        [JsonPropertyName("additional_fields_list")]
        public List<string> AdditionalFieldsList { get; set; }

        [JsonPropertyName("multiple")]
        public bool Multiple { get; set; }

        [JsonPropertyName("parent")]
        public int? Parent { get; set; }

        [JsonPropertyName("account_vacancy_status_group")]
        public int? AccountVacancyStatusGroup { get; set; }

    }
}
