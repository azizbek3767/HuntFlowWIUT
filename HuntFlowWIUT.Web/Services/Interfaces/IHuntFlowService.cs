using HuntFlowWIUT.Web.Models;

namespace HuntFlowWIUT.Web.Services.Interfaces
{
    public interface IHuntFlowService
    {
        Task<IEnumerable<Candidate>> GetCandidateAsync();

        Task<VacanciesResponse> GetVacanciesAsync(
            int accountId,
            int page = 1,
            int count = 30,
            bool mine = false,
            bool opened = false,
            string[] state = null);
    }
}
