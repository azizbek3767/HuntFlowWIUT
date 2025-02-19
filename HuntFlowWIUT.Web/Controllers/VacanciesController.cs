using HuntFlowWIUT.Web.Helpers;
using HuntFlowWIUT.Web.Models;
using HuntFlowWIUT.Web.Models.ViewModels;
using HuntFlowWIUT.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static HuntFlowWIUT.Web.Services.Interfaces.IHuntFlowService;

namespace HuntFlowWIUT.Web.Controllers
{
    public class VacanciesController : Controller
    {
        private readonly IHuntflowService _huntflowService;
        private readonly IConfiguration _configuration;
        private readonly int _accountId;
        private readonly ICountryService _countryService;


        public VacanciesController(ICountryService countryService, IHuntflowService huntflowService, IConfiguration configuration)
        {
            _countryService = countryService;
            _huntflowService = huntflowService;
            _configuration = configuration;
            _accountId = int.Parse(configuration["Huntflow:AccountId"]);
        }

        // GET: /Vacancy/Index
        public async Task<IActionResult> Index(int page = 1, int count = 30)
        {
            var vacancyResponse = await _huntflowService.GetVacanciesAsync(_accountId, page, count);

            var viewModel = new VacanciesIndexViewModel
            {
                Page = vacancyResponse.Page,
                Count = vacancyResponse.Count,
                TotalPages = vacancyResponse.Total_Pages,
                TotalItems = vacancyResponse.Total_Items,
                Vacancies = vacancyResponse.Items
            };

            return View(viewModel);
        }

        // GET: /Vacancy/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var vacancyDetail = await _huntflowService.GetVacancyDetailAsync(_accountId, id);
            return View(vacancyDetail);
        }

        
        [HttpGet]
        public async Task<IActionResult> Apply(int id)
        {
            // Retrieve vacancy details to prefill the Position field.
            var vacancy = await _huntflowService.GetVacancyDetailAsync(_accountId, id);
            if (vacancy == null)
            {
                return NotFound();
            }

            var model = new ApplicantCreationViewModel
            {
                Position = vacancy.Position,
                Company = vacancy.Company,
                Birthday = DateTime.Today, // default value, for example.
                PlaceOfBirth = "Enter place of birth", // Or empty string if preferred
                HowDidYouHear = "",
                SocialProfiles = "",
                // (Set default values for booleans if needed)
                Convicted = false,
                CurrentlyWorkingAtWIUT = false,
                AppliedBeforeAtWIUT = false,
                HasRelativesAtWIUT = false,
                Motivation = "",
                SpecificSkills = "",
                AdditionalInfo = "",
                Declaration = false,
                SubmissionDate = DateTime.Now
            };

            // Ensure at least one Employment block with default dates
            if (model.EmploymentBlocks == null || model.EmploymentBlocks.Count == 0)
            {
                model.EmploymentBlocks.Add(new EmploymentBlockViewModel
                {
                    FromDate = DateTime.Today,
                    ToDate = DateTime.Today,
                    FullOrPartTime = "Full"
                });
            }

            // Ensure at least one Education block with default dates
            if (model.EducationBlocks == null || model.EducationBlocks.Count == 0)
            {
                model.EducationBlocks.Add(new EducationBlockViewModel
                {
                    FromDate = DateTime.Today,
                    ToDate = DateTime.Today,
                    FullOrPartTime = "Full"
                });
            }

            // Ensure at least one Practical Training block with default dates
            if (model.PracticalTrainingBlocks == null || model.PracticalTrainingBlocks.Count == 0)
            {
                model.PracticalTrainingBlocks.Add(new PracticalTrainingBlockViewModel
                {
                    FromDate = DateTime.Today,
                    ToDate = DateTime.Today
                });
            }

            // Ensure at least one Language block with default values
            if (model.LanguageBlocks == null || model.LanguageBlocks.Count == 0)
            {
                model.LanguageBlocks.Add(new LanguageBlockViewModel
                {
                    Reading = 1,
                    Writing = 1,
                    Speaking = 1
                });
            }

            // Ensure External resume is prepopulated with non-null Data
            if (model.Externals == null || model.Externals.Count == 0 || model.Externals[0].Data == null)
            {
                model.Externals = new List<ExternalResumeViewModel>
        {
            new ExternalResumeViewModel { Data = new ResumeDataViewModel { Body = "" } }
        };
            }

            // Ensure Social block exists
            if (model.Social == null || model.Social.Count == 0)
            {
                model.Social = new List<SocialAccountViewModel>
        {
            new SocialAccountViewModel { Value = "" }
        };
            }


            // Ensure the externals and social lists are prepopulated.
            /* if (model.Externals == null || model.Externals.Count == 0)
             {
                 model.Externals.Add(new ExternalResumeViewModel { Data = new ResumeDataViewModel() });
             }
             if (model.Social == null || model.Social.Count == 0)
             {
                 model.Social.Add(new SocialAccountViewModel());
             }*/
            var countries = await _countryService.GetCountriesAsync();
            // Set default country "Uzbekistan"
            var defaultCountry = countries.FirstOrDefault(c => c.Name.Equals("Uzbekistan", StringComparison.OrdinalIgnoreCase));
            int? defaultCountryId = defaultCountry?.Id;
            model.Countries = new SelectList(countries, "Id", "Name", defaultCountryId);
            // Also preselect in CitizenshipId if you wish:
            model.CitizenshipId = defaultCountryId ?? 0;
            /*ViewBag.Countries = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "United States" },
                new SelectListItem { Value = "2", Text = "Canada" },
                new SelectListItem { Value = "3", Text = "United Kingdom" },
                new SelectListItem { Value = "4", Text = "Uzbekistan" }
            };*/
            // Also pass account id and auth token if needed
            ViewBag.AccountId = _accountId;
            ViewBag.AuthToken = _configuration["Huntflow:AccessToken"];

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ApplyAcademic(int id)
        {
            // Retrieve vacancy details (which include AccountDivision, Position, Company, etc.)
            var vacancy = await _huntflowService.GetVacancyDetailAsync(_accountId, id);
            if (vacancy == null)
            {
                return NotFound();
            }
         
                // Build the view model for the second type of application form.
            var model = new ApplicantCreationAcademicViewModel
            {
                // Pre-fill Position from vacancy details.
                Position = vacancy.Position,
                // You can initialize AreaOfInterest to an empty string.
                AreaOfInterest = "",

                // First and Last Name are left for the applicant to fill.
                FirstName = "",
                LastName = "",

                // Contact and Citizenship will be filled by the applicant.
                Phone = "",
                Email = "",

                // Photo: set to 0 initially (it will be updated via AJAX upload)
                Photo = 0,

                // Research IDs block – initialize with empty values.
                ResearchIds = new ResearchIds(),

                // Degrees: start with an empty list; user must select at least one.
                Degrees = new List<string>(),

                // How did you hear about this vacancy? (and detail)
                HowDidYouHear = "",
                HowDidYouHearDetail = "",

                // Boolean selections – default values can be adjusted as needed.
                CurrentlyInUzbekistan = true,
                AppliedBeforeAtWIUT = false,
                EverWorkedAtWIUT = false,
                HasRelativesAtWIUT = false,
                Convicted = false,

                // Desired salary (set default to 0 and currency to UZS)
                DesiredSalary = 0,
                SalaryCurrency = "UZS",

                // File attachments – initialize all file ID properties to 0.
                CoverLetter = 0,
                Cv = 0,
                EvaluationReport = 0,
                Vision = 0,
                TeachingPortfolio = 0,
                ResearchStatement = 0,
                Dissertation = 0,
                Diplomas = 0,
                Transcripts = 0,
                EnglishCertificate = 0,
                References = 0,

                // Declaration and submission date.
                Declaration = false,
                SubmissionDate = DateTime.Now,

                // Merged application data – will be built on the client side via JavaScript.
                MergedApplicationData = ""
            };

            // Populate the Citizenship dropdown.
            var countries = await _countryService.GetCountriesAsync();
            var defaultCountry = countries.FirstOrDefault(c => c.Name.Equals("Uzbekistan", StringComparison.OrdinalIgnoreCase));
            int? defaultCountryId = defaultCountry?.Id;
            model.CitizenshipId = defaultCountryId ?? 0;
            model.Countries = new SelectList(countries, "Id", "Name", defaultCountryId);

            // (If your model has additional properties – e.g., for Social accounts – initialize them here as needed.)
            // For example, if you use social for this form:
            model.Social = new List<SocialAccountViewModel>
            {
                new SocialAccountViewModel { Value = "" }
            };

            // Pass additional values via ViewBag if required.
            ViewBag.AccountId = _accountId;
            ViewBag.AuthToken = _configuration["Huntflow:AccessToken"];

            return View("ApplyAcademic", model);
        }



        [HttpPost]
        public async Task<IActionResult> Apply(int id, ApplicantCreationViewModel model)
        {
            if (model.Birthday == DateTime.MinValue)
            {
                ModelState.AddModelError("Birthday", "Please enter a valid birthday.");
            }

            // Check that at least one of FirstName or LastName is provided.
            if (string.IsNullOrWhiteSpace(model.FirstName) && string.IsNullOrWhiteSpace(model.LastName))
            {
                ModelState.AddModelError("FirstName", "First name or last name is required.");
            }

            if (!ModelState.IsValid)
            {
                var countries = await _countryService.GetCountriesAsync();
                var defaultCountry = countries.FirstOrDefault(c => c.Name.Equals("Uzbekistan", StringComparison.OrdinalIgnoreCase));
                int? defaultCountryId = defaultCountry?.Id;
                model.Countries = new SelectList(countries, "Id", "Name", defaultCountryId);
                ViewBag.AccountId = _accountId;
                ViewBag.AuthToken = _configuration["Huntflow:AccessToken"];

                var errorList = new List<string>();
                foreach (var key in ModelState.Keys)
                {
                    foreach (var error in ModelState[key].Errors)
                    {
                        errorList.Add($"Error in {key}: {error.ErrorMessage}");
                    }
                }
                ViewBag.ErrorList = errorList;
                return View(model);
            }

            // Ensure model.Externals is initialized.
            if (model.Externals == null || model.Externals.Count == 0)
            {
                model.Externals = new List<ExternalResumeViewModel>
        {
            new ExternalResumeViewModel { Data = new ResumeDataViewModel() }
        };
            }
            else if (model.Externals[0].Data == null)
            {
                model.Externals[0].Data = new ResumeDataViewModel();
            }

            // Remove any Social entries with null/empty Value.
            if (model.Social == null)
            {
                model.Social = new List<SocialAccountViewModel>();
            }
            else
            {
                model.Social = model.Social.Where(s => !string.IsNullOrWhiteSpace(s.Value)).ToList();
            }

            // Set the merged application data into the external resume's Data.Body.
            model.Externals[0].Data.Body = model.MergedApplicationData;
            model.SubmissionDate = DateTime.Now;

            var applicant = await _huntflowService.CreateApplicantAsync(_accountId, model);
            TempData["SuccessMessage"] = "Your application was submitted successfully!";
            return RedirectToAction("Details", new { id = id });
        }


        [HttpPost]
        public async Task<IActionResult> ApplyAcademic(int id, ApplicantCreationAcademicViewModel model)
        {
            // Ensure at least one degree is selected.
            if (model.Degrees == null || model.Degrees.Count == 0)
            {
                ModelState.AddModelError("Degrees", "Please select at least one degree.");
            }
            // Additional validations can be added here as needed

            if (!ModelState.IsValid)
            {
                // Repopulate the Countries dropdown
                var countries = await _countryService.GetCountriesAsync();
                var defaultCountry = countries.FirstOrDefault(c =>
                    c.Name.Equals("Uzbekistan", StringComparison.OrdinalIgnoreCase));
                int? defaultCountryId = defaultCountry?.Id;
                model.Countries = new SelectList(countries, "Id", "Name", defaultCountryId);
                model.CitizenshipId = defaultCountryId ?? 0;

                // Set additional ViewBag values
                ViewBag.AccountId = _accountId;
                ViewBag.AuthToken = _configuration["Huntflow:AccessToken"];

                // Build an error list for display
                var errorList = new List<string>();
                foreach (var key in ModelState.Keys)
                {
                    foreach (var error in ModelState[key].Errors)
                    {
                        errorList.Add($"Error in {key}: {error.ErrorMessage}");
                    }
                }
                ViewBag.ErrorList = errorList;

                // Return the view so user can correct the errors
                return View(model);
            }
            // Ensure model.Externals is initialized.
            if (model.Externals == null || model.Externals.Count == 0)
            {
                model.Externals = new List<ExternalResumeViewModel>
        {
            new ExternalResumeViewModel { Data = new ResumeDataViewModel() }
        };
            }
            else if (model.Externals[0].Data == null)
            {
                model.Externals[0].Data = new ResumeDataViewModel();
            }

            // Remove any Social entries with null/empty Value.
            if (model.Social == null)
            {
                model.Social = new List<SocialAccountViewModel>();
            }
            else
            {
                model.Social = model.Social.Where(s => !string.IsNullOrWhiteSpace(s.Value)).ToList();
            }

            // Set the submission date to now
            model.SubmissionDate = DateTime.Now;
            model.Externals[0].Data.Body = model.MergedApplicationData;

            // Call your service to create the applicant (assuming it can handle the type2 model)
            var applicant = await _huntflowService.CreateAcademicApplicantAsync(_accountId, model);

            TempData["SuccessMessage"] = "Your application was submitted successfully!";
            return RedirectToAction("Details", new { id = id });
        }
    }
}
