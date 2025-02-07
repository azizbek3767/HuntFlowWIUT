using HuntFlowWIUT.Web.Models;
using HuntFlowWIUT.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HuntFlowWIUT.Web.Controllers
{
    public class HuntFlowController : Controller
    {
        private readonly IHuntFlowService _huntFlowService;
        public HuntFlowController(IHuntFlowService huntFlowService)
        {
            _huntFlowService = huntFlowService;
        }

        public async Task<IActionResult> Vacancies(int accountId = 3360, int page = 1, int count = 30)
        {
            var vacancies = await _huntFlowService.GetVacanciesAsync(accountId, page, count);

            return View(vacancies);
        }

        public async Task<IActionResult> Index(int accountId = 3360, int page = 1, int count = 30)
        {
            VacanciesResponse vacanciesResponse = await _huntFlowService.GetVacanciesAsync(accountId, page, count);

            return View(vacanciesResponse);
        }

        public async Task<IActionResult> Candidates()
        {
            var candidates = await _huntFlowService.GetCandidateAsync();
            return View(candidates);
        }
    }
}
