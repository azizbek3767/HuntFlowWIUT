/*using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HuntFlowWIUT.Web.Models.ViewModels
{
    public class ApplicantCreationViewModelOld
    {
        [Required]
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [Required]
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("middle_name")]
        public string MiddleName { get; set; }

        [JsonPropertyName("money")]
        public string Money { get; set; }

        [Required]
        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("skype")]
        public string Skype { get; set; }

        [Required]
        [JsonPropertyName("position")]
        public string Position { get; set; }

        [JsonPropertyName("company")]
        public string Company { get; set; }

        [JsonPropertyName("photo")]
        public int? Photo { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [JsonPropertyName("birthday")]
        public DateTime Birthday { get; set; }

        [Required]
        [JsonPropertyName("externals")]
        public List<ExternalResumeViewModel> Externals { get; set; } = new List<ExternalResumeViewModel>();

        [Required]
        [JsonPropertyName("social")]
        public List<SocialAccountViewModel> Social { get; set; } = new List<SocialAccountViewModel>();
    }

    public class ExternalResumeViewModelOld
    {
        [Required]
        [JsonPropertyName("auth_type")]
        public string AuthType { get; set; } = "HH";

        [Required]
        [JsonPropertyName("account_source")]
        public int AccountSource { get; set; } = 1189;

        [Required]
        [JsonPropertyName("data")]
        public ResumeDataViewModel Data { get; set; }

        [JsonPropertyName("files")]
        public List<int> Files { get; set; } = new List<int>();
    }

    public class ResumeDataViewModelOld
    {
        [Required]
        [JsonPropertyName("body")]
        public string Body { get; set; }
    }

    public class SocialAccountViewModelOld
    {
        [Required]
        [JsonPropertyName("social_type")]
        public string SocialType { get; set; } = "TELEGRAM";

        [Required]
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}
*/