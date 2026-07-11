namespace EduNexus.ViewModels.RubricCriterion
{
    public class RubricCriterionItemViewModel
    {
        public long CriterionId { get; set; }           // Đổi từ RubricCriterionId
        public long RubricId { get; set; }
        public string CriterionName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Weight { get; set; }
        public decimal MaxScore { get; set; }
        public int DisplayOrder { get; set; }
    }
}
