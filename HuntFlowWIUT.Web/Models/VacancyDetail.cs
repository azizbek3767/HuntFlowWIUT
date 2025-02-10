using Microsoft.AspNetCore.Mvc.Filters;

namespace HuntFlowWIUT.Web.Models
{
    public class VacancyDetail : VacancySummary
    {
        public DateTime Updated { get; set; }
        public string Body { get; set; }
        public string Requirements { get; set; }
        public string Conditions { get; set; }
        public List<FileItem> Files { get; set; } = new();
        public string Source { get; set; }
        public List<VacancyDetail> Blocks { get; set; } = new();
    }
}
