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

        // GET: /Vacancies/Apply/{id}
        /*[HttpGet]
        public async Task<IActionResult> Apply(int id)
        {
            var vacancy = await _huntflowService.GetVacancyDetailAsync(_accountId, id);
            if (vacancy == null)
            {
                return NotFound();
            }

            var model = new ApplicantCreationViewModel
            {
                Position = vacancy.Position,
                Company = vacancy.Company,
                Birthday = DateTime.Today
            };

            if (model.Externals == null || model.Externals.Count == 0)
            {
                model.Externals.Add(new ExternalResumeViewModel { Data = new ResumeDataViewModel() });
            }
            if (model.Social == null || model.Social.Count == 0)
            {
                model.Social.Add(new SocialAccountViewModel());
            }

            return View(model);
        }*/
        
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
            model.PhotoFile = null;

            var applicant = await _huntflowService.CreateApplicantAsync(_accountId, model);
            TempData["SuccessMessage"] = "Your application was submitted successfully!";
            return RedirectToAction("Details", new { id = id });
        }




        // POST: /Vacancies/Apply/{id}
        /*[HttpPost]
        public async Task<IActionResult> Apply(int id, ApplicantCreationViewModel model)
        {
            if (model.Birthday == DateTime.MinValue)
            {
                ModelState.AddModelError("Birthday", "Please enter a valid birthday.");
                return View(model);
            }

            *//*if (!ModelState.IsValid)
            {
                return View(model);
            }*//*
            
            if (!ModelState.IsValid)
            {
                var countries = await _countryService.GetCountriesAsync();
                var defaultCountry = countries.FirstOrDefault(c => c.Name.Equals("Uzbekistan", StringComparison.OrdinalIgnoreCase));
                int? defaultCountryId = defaultCountry?.Id;
                model.Countries = new SelectList(countries, "Id", "Name", defaultCountryId);
                ViewBag.AccountId = _accountId;
                ViewBag.AuthToken = _configuration["Huntflow:AccessToken"];
                // Log ModelState errors (using Console.WriteLine for demonstration; replace with your logging mechanism)
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    foreach (var error in state.Errors)
                    {
                        Console.WriteLine($"Error in {key}: {error.ErrorMessage}");
                    }
                }
                // Return the view with the model so that errors are displayed
                return View(model);
            }
            // Ensure that the merged application data is set into the external resume block.
            if (model.Externals.Count > 0)
            {
                model.Externals[0].Data.Body = model.MergedApplicationData; // The merged data from the hidden field.
            }
            // Set the submission date
            model.SubmissionDate = DateTime.Now;

            var applicant = await _huntflowService.CreateApplicantAsync(_accountId, model);

            TempData["SuccessMessage"] = "Your application was submitted successfully!";

            return RedirectToAction("Details", new { id = id });
        }*/
    }
}
