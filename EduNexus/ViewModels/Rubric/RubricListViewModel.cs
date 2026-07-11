namespace EduNexus.ViewModels.Rubric
{
    public class RubricListViewModel
    {
        public long RubricId { get; set; }
        public string RubricName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal TotalWeight { get; set; }
    }
}