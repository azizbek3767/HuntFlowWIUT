using System.ComponentModel.DataAnnotations;

namespace HuntFlowWIUT.Web.Models.ViewModels
{
    public class VacancyApplicationViewModel
    {
        [Display(Name = "Division ID")]
        [Required]
        public int AccountDivision { get; set; }

        [Display(Name = "Region ID")]
        [Required]
        public int AccountRegion { get; set; }

        [Display(Name = "Position")]
        [Required]
        public string Position { get; set; }

        [Display(Name = "Company")]
        public string Company { get; set; }

        [Display(Name = "Salary")]
        public string Money { get; set; }

        [Display(Name = "Priority")]
        [Range(0, 1)]
        public int Priority { get; set; }

        [Display(Name = "Hidden")]
        public bool Hidden { get; set; }

        [Display(Name = "State")]
        [Required]
        public string State { get; set; } = "OPEN";

        [Display(Name = "Applicant Offer ID")]
        [Required]
        public int AccountApplicantOffer { get; set; }

        [Display(Name = "Application Details")]
        public string Body { get; set; }

        public string Requirements { get; set; }
        public string Conditions { get; set; }

        public List<int> Coworkers { get; set; } = new List<int>();
        public List<int> Files { get; set; } = new List<int>();

        [Required]
        [Display(Name = "Fill Quotas")]
        public List<FillQuotaViewModel> FillQuotas { get; set; } = new List<FillQuotaViewModel>();
    }

    public class FillQuotaViewModel
    {
        [Display(Name = "Deadline")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime Deadline { get; set; }

        [Display(Name = "Number of Applicants to Hire")]
        [Range(1, 999)]
        [Required]
        public int ApplicantsToHire { get; set; }

        [Display(Name = "Vacancy Request ID")]
        [Required]
        public int VacancyRequest { get; set; }
    }
}
