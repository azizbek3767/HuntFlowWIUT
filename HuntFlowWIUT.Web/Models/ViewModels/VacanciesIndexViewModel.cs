namespace HuntFlowWIUT.Web.Models.ViewModels
{
    public class VacanciesIndexViewModel
    {
        public int Page { get; set; }
        public int Count { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public List<VacancySummary> Vacancies { get; set; } = new();
    }
}
