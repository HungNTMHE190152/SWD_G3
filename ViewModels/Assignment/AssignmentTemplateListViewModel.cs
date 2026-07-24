namespace EduNexus.ViewModels.Assignment
{
    public class AssignmentTemplateListViewModel
    {
        public long AssignmentTemplateId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string CourseName { get; set; } = string.Empty;

        public string? ModuleName { get; set; }

        public decimal TotalScore { get; set; }

        public bool HasRubric { get; set; }

        public bool IsActive { get; set; }
    }
}