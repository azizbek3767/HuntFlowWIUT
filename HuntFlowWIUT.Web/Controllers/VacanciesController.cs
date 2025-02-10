using HuntFlowWIUT.Web.Models;
using HuntFlowWIUT.Web.Models.ViewModels;
using HuntFlowWIUT.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static HuntFlowWIUT.Web.Services.Interfaces.IHuntFlowService;

namespace HuntFlowWIUT.Web.Controllers
{
    public class VacanciesController : Controller
    {
        private readonly IHuntflowService _huntflowService;
        private readonly IConfiguration _configuration;
        private readonly int _accountId;

        public VacanciesController(IHuntflowService huntflowService, IConfiguration configuration)
        {
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
        [HttpGet]
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
        }

        // POST: /Vacancies/Apply/{id}
        [HttpPost]
        public async Task<IActionResult> Apply(int id, ApplicantCreationViewModel model)
        {
            if (model.Birthday == DateTime.MinValue)
            {
                ModelState.AddModelError("Birthday", "Please enter a valid birthday.");
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var applicant = await _huntflowService.CreateApplicantAsync(_accountId, model);

            TempData["SuccessMessage"] = "Your application was submitted successfully!";

            return RedirectToAction("Details", new { id = id });
        }
    }
}
