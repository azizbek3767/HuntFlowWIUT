using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HuntFlowWIUT.Web.Models.ViewModels
{
    public class ApplicantCreationAcademicViewModel
    {
        // 1. Position applied for* (pre-filled from vacancy details)
        [Required]
        [JsonPropertyName("position")]
        public string Position { get; set; }

        // 2. Area Of Interest
        public string AreaOfInterest { get; set; } = "";

        // 3. First Name* (required)
        [Required]
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        // 4. Last Name* (required)
        [Required]
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        // 5. Contact telephone numbers* (required)
        [Required]
        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        // 6. Citizenship* (required) – expects a country id
        [Required]
        public int CitizenshipId { get; set; }

        // 7. E-mail Address* (required)
        [Required]
        [EmailAddress]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        // 8. Photo – two properties:
        [JsonIgnore]
        public IFormFile PhotoFile { get; set; }
        [JsonPropertyName("photo")]
        public int Photo { get; set; }

        // 9. Research IDs block (optional)
        public ResearchIds ResearchIds { get; set; } = new ResearchIds();

        // 10. Degrees (required) – a list of selected degrees; at least one required.
        [Required(ErrorMessage = "Please select at least one degree.")]
        [JsonPropertyName("degrees")]
        public List<string> Degrees { get; set; } = new List<string>();

        // 11. How did you hear about this vacancy?* (required) and detail
        [Required]
        public string HowDidYouHear { get; set; }
        public string? HowDidYouHearDetail { get; set; } = "";

        // 12. Are you currently in Uzbekistan?* (required)
        [Required]
        public bool CurrentlyInUzbekistan { get; set; }

        // 13. Have you applied for any position at WIUT before?* (required)
        [Required]
        public bool AppliedBeforeAtWIUT { get; set; }

        // 14. Have you ever worked at WIUT?* (required)
        [Required]
        public bool EverWorkedAtWIUT { get; set; }

        // 15. Do you have relatives who are currently employed at WIUT?* (required)
        [Required]
        [JsonPropertyName("has_relatives_at_wiut")]
        public bool HasRelativesAtWIUT { get; set; }

        // 16. Have you ever been convicted of a crime? (required)
        [Required]
        [JsonPropertyName("convicted")]
        public bool Convicted { get; set; }

        // 17. Desired salary level (monthly): number and currency.
        [JsonPropertyName("desired_salary")]
        public decimal DesiredSalary { get; set; }
        [JsonPropertyName("salary_currency")]
        public string SalaryCurrency { get; set; } = "UZS";  // default

        // File attachments (section "Please upload your files below:") – each file is represented by an IFormFile property (ignored in JSON)
        // and an integer property that will be sent in JSON.
        [JsonIgnore]
        public IFormFile? CoverLetterFile { get; set; }
        [JsonPropertyName("cover_letter")]
        public int CoverLetter { get; set; }

        [JsonIgnore]
        public IFormFile? CvFile { get; set; }
        [JsonPropertyName("cv")]
        public int Cv { get; set; }

        [JsonIgnore]
        public IFormFile? EvaluationReportFile { get; set; }
        [JsonPropertyName("evaluation_report")]
        public int EvaluationReport { get; set; }

        [JsonIgnore]
        public IFormFile? VisionFile { get; set; }
        [JsonPropertyName("vision")]
        public int Vision { get; set; }

        [JsonIgnore]
        public IFormFile? TeachingPortfolioFile { get; set; }
        [JsonPropertyName("teaching_portfolio")]
        public int TeachingPortfolio { get; set; }

        [JsonIgnore]
        public IFormFile? ResearchStatementFile { get; set; }
        [JsonPropertyName("research_statement")]
        public int ResearchStatement { get; set; }

        [JsonIgnore]
        public IFormFile? DissertationFile { get; set; }
        [JsonPropertyName("dissertation")]
        public int Dissertation { get; set; }

        [JsonIgnore]
        public IFormFile? DiplomasFile { get; set; }
        [JsonPropertyName("diplomas")]
        public int Diplomas { get; set; }

        [JsonIgnore]
        public IFormFile? TranscriptsFile { get; set; }
        [JsonPropertyName("transcripts")]
        public int Transcripts { get; set; }

        [JsonIgnore]
        public IFormFile? EnglishCertificateFile { get; set; }
        [JsonPropertyName("english_certificate")]
        public int EnglishCertificate { get; set; }

        [JsonIgnore]
        public IFormFile? ReferencesFile { get; set; }
        [JsonPropertyName("references")]
        public int References { get; set; }

        // 29. Declaration checkbox (required)
        [Required]
        [JsonPropertyName("declaration")]
        public bool Declaration { get; set; }

        // Hidden field: submission date/time
        public DateTime SubmissionDate { get; set; }

        // Merged data: for storing merged application details (if applicable)
        public string MergedApplicationData { get; set; } = "";

        // Optional: Countries list for dropdown binding (not sent in JSON)
        [JsonIgnore]
        public IEnumerable<SelectListItem>? Countries { get; set; }

        // The external resume block that stores the merged data and resume file ID.
        [JsonPropertyName("externals")]
        public List<ExternalResumeViewModel>? Externals { get; set; } = new List<ExternalResumeViewModel>();

        [JsonPropertyName("social")]
        public List<SocialAccountViewModel>? Social { get; set; } = new List<SocialAccountViewModel>();
        // *** New property for Countries ***
        public IFormFile? ResumeFile { get; set; }
    }

    public class ResearchIds
    {
        [JsonPropertyName("google_scholar")]
        public string GoogleScholar { get; set; } = "";

        [JsonPropertyName("orcid")]
        public string Orcid { get; set; } = "";

        [JsonPropertyName("other")]
        public string Other { get; set; } = "";
    }
   
}
