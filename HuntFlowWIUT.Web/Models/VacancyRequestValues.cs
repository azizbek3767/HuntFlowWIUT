using System.Text.Json.Serialization;

namespace HuntFlowWIUT.Web.Models
{
    public class VacancyRequestValues
    {
        [JsonPropertyName("position")]
        public string Position { get; set; }

        [JsonPropertyName("account_division")]
        public int AccountDivision { get; set; }

        [JsonPropertyName("category")]
        public int Category { get; set; }
    }
}
