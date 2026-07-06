namespace EduNexus.ViewModels.Dashboard;

public class PersonalProgressViewModel
{
    public long StudentId { get; set; }

    public string StudentName { get; set; } = string.Empty;

    public decimal OverallProgress { get; set; }

    public List<CourseProgressDetailViewModel> CourseProgressList { get; set; } = new();

    public List<QuizProgressItemViewModel> QuizProgressList { get; set; } = new();

    public List<AssignmentProgressItemViewModel> AssignmentProgressList { get; set; } = new();
}