using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HuntFlowWIUT.Web.Models.ViewModels
{
    public class ApplicantCreationViewModel
    {
        // Mapped directly:
        [Required]
        [JsonPropertyName("position")]
        public string Position { get; set; } // Pre-filled with vacancy name (read-only)

        [Required]
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [Required]
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        // MiddleName is skipped and given a default value
        [JsonPropertyName("middle_name")]
        public string MiddleName { get; set; } = "Middle Name";
        
        [JsonPropertyName("money")]
        public string? Money { get; set; } = "Money";

        [Required]
        [DataType(DataType.Date)]
        [JsonPropertyName("birthday")]
        public DateTime Birthday { get; set; }

        [Required]
        public string? PlaceOfBirth { get; set; }

        [Required]
        public int CitizenshipId { get; set; } // assumes a country ID

        [Required]
        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        // Skype is skipped and given a default
        [JsonPropertyName("skype")]
        public string Skype { get; set; } = "Skype";

        // Company is skipped and given a default
        [JsonPropertyName("company")]
        public string Company { get; set; } = "Company";

        // Photo upload (file) – not merged in big string.
        [JsonPropertyName("photo")]
        public IFormFile? PhotoFile { get; set; }

        public string? HowDidYouHear { get; set; } = string.Empty;

        public string? SocialProfiles { get; set; } = string.Empty;

        [Required]
        public bool Convicted { get; set; }

        [Required]
        public bool CurrentlyWorkingAtWIUT { get; set; }

        [Required]
        public bool AppliedBeforeAtWIUT { get; set; }

        [Required]
        public bool HasRelativesAtWIUT { get; set; }

        // Employment section: dynamic list of employment blocks.
        public List<EmploymentBlockViewModel>? EmploymentBlocks { get; set; } = new List<EmploymentBlockViewModel>();

        // Checkbox for supervisor contact
        public bool AllowSupervisorContact { get; set; }

        // Education section
        public List<EducationBlockViewModel>? EducationBlocks { get; set; } = new List<EducationBlockViewModel>();

        // Practical Training section
        public List<PracticalTrainingBlockViewModel>? PracticalTrainingBlocks { get; set; } = new List<PracticalTrainingBlockViewModel>();

        // Languages section
        public List<LanguageBlockViewModel>? LanguageBlocks { get; set; } = new List<LanguageBlockViewModel>();

        // Resume Upload (PDF file) – separate file upload
        public IFormFile? ResumeFile { get; set; }

        public string? Motivation { get; set; }

        public string? SpecificSkills { get; set; }

        public string? AdditionalInfo { get; set; }

        [Required]
        public bool Declaration { get; set; }

        // Submission date – hidden field, set on server-side
        public DateTime SubmissionDate { get; set; }

        // This property will hold the merged application data as a formatted string.
        public string MergedApplicationData { get; set; }

        // The external resume block that stores the merged data and resume file ID.
        [JsonPropertyName("externals")]
        public List<ExternalResumeViewModel>? Externals { get; set; } = new List<ExternalResumeViewModel>();

        [JsonPropertyName("social")]
        public List<SocialAccountViewModel>? Social { get; set; } = new List<SocialAccountViewModel>();
        // *** New property for Countries ***
        public IEnumerable<SelectListItem>? Countries { get; set; }
    }

    public class EmploymentBlockViewModel
    {
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; } = DateTime.Today;
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; } = DateTime.Today;
        public string? FullOrPartTime { get; set; } // "Full" or "Part"
        public string? EmployerName { get; set; }
        public string? PositionInOrg { get; set; }
        public string? MainDuties { get; set; }
        public string? SupervisorInfo { get; set; }
    }

    public class EducationBlockViewModel
    {
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; } = DateTime.Today;
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; } = DateTime.Today;
        public string? FullOrPartTime { get; set; }
        public string? InstitutionInfo { get; set; }
        public string? Qualifications { get; set; }
    }

    public class PracticalTrainingBlockViewModel
    {
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; } = DateTime.Today;
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; } = DateTime.Today;
        public string? OrganisationName { get; set; }
        public string? Certification { get; set; }
    }

    public class LanguageBlockViewModel
    {
        public string? LanguageName { get; set; }
        public int Reading { get; set; }  // 1 to 5
        public int Writing { get; set; }
        public int Speaking { get; set; }
    }

    public class ExternalResumeViewModel
    {
        [JsonPropertyName("auth_type")]
        // For applicant creation we can use "NATIVE" or "HH" as appropriate. Here we'll use "NATIVE".
        public string? AuthType { get; set; } = "NATIVE";

        [JsonPropertyName("account_source")]
        // Use the valid account source value; from your GET sample earlier, it was 1189.
        public int? AccountSource { get; set; } = 1189;

        [JsonPropertyName("data")]
        public ResumeDataViewModel? Data { get; set; }

        // Files: list of file IDs for the resume upload (field 21).
        [JsonPropertyName("files")]
        public List<int>? Files { get; set; } = new List<int>();
    }

    public class ResumeDataViewModel
    {
        [JsonPropertyName("body")]
        public string? Body { get; set; } = "default body";
    }

    public class SocialAccountViewModel
    {
        [JsonPropertyName("social_type")]
        public string? SocialType { get; set; } = "TELEGRAM";

        [JsonPropertyName("value")]
        public string? Value { get; set; } = "";
    }
}
