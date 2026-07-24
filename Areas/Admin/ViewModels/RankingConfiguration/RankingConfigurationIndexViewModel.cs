namespace EduNexus.Areas.Admin.ViewModels.RankingConfiguration
{
    public class RankingConfigurationIndexViewModel
    {
        public RankingConfigurationFormViewModel Form { get; set; }
            = new();

        public bool HasActiveConfiguration { get; set; }

        public long? ActiveConfigurationId { get; set; }

        public string? CurrentUpdatedByName { get; set; }

        public DateTime? CurrentUpdatedAt { get; set; }

        public List<RankingConfigurationHistoryItemViewModel>
            History
        { get; set; } = new();
    }
}