namespace HuntFlowWIUT.Web.Models
{
    public class VacancyListResponse
    {
        public int Page { get; set; }
        public int Count { get; set; }
        public int Total_Pages { get; set; }
        public int Total_Items { get; set; }
        public List<VacancySummary> Items { get; set; } = new();
    }
}
