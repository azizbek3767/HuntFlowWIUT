using HuntFlowWIUT.Web.Models;
using HuntFlowWIUT.Web.Models.ViewModels;

namespace HuntFlowWIUT.Web.Services.Interfaces
{
    public interface IHuntFlowService
    {
        public interface IHuntflowService
        {
            Task<Models.VacancyListResponse> GetVacanciesAsync(int accountId, int page = 1, int count = 30);
            Task<Models.VacancyDetail> GetVacancyDetailAsync(int accountId, int vacancyId);
            Task<ApplicantDetail> CreateApplicantAsync(int accountId, ApplicantCreationViewModel model);
        }


    }
}
