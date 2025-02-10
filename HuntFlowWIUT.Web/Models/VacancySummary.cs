namespace HuntFlowWIUT.Web.Models
{
    public class VacancySummary
    {
        public int Account_Division { get; set; }
        public int? Account_Region { get; set; }
        public string Position { get; set; }
        public string Company { get; set; }
        public string Money { get; set; }
        public int Priority { get; set; }
        public bool Hidden { get; set; }
        public string State { get; set; }
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public List<string> Additional_Fields_List { get; set; } = new();
        public bool Multiple { get; set; }
        public int? Parent { get; set; }
        public int? Account_Vacancy_Status_Group { get; set; }
    }
}
