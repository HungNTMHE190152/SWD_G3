using EduNexus.ViewModels.RubricCriterion;

namespace EduNexus.ViewModels.Rubric
{
    public class RubricManagementViewModel
    {
        public long RubricId { get; set; }
        public string RubricName { get; set; } = string.Empty;
        public long AssignmentId { get; set; }
        public string AssignmentTitle { get; set; } = string.Empty;
        public decimal TotalWeight { get; set; }
        public List<RubricCriterionItemViewModel> CurrentCriteria { get; set; } = new();
    }
}
