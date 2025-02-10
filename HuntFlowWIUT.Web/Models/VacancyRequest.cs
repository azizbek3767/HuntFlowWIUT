using System.Text.Json.Serialization;

namespace HuntFlowWIUT.Web.Models
{
    public class VacancyRequest
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("position")]
        public string Position { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("account_vacancy_request")]
        public int AccountVacancyRequest { get; set; }

        [JsonPropertyName("created")]
        public DateTimeOffset Created { get; set; }

        [JsonPropertyName("updated")]
        public DateTimeOffset Updated { get; set; }

        [JsonPropertyName("changed")]
        public DateTimeOffset Changed { get; set; }

        [JsonPropertyName("vacancy")]
        public int Vacancy { get; set; }

        [JsonPropertyName("creator")]
        public UserInfo Creator { get; set; }

        [JsonPropertyName("files")]
        public List<VacancyRequestFile> Files { get; set; }

        [JsonPropertyName("states")]
        public List<VacancyRequestState> States { get; set; }

        [JsonPropertyName("values")]
        public VacancyRequestValues Values { get; set; }

        [JsonPropertyName("taken_by")]
        public UserInfo TakenBy { get; set; }
    }
}
