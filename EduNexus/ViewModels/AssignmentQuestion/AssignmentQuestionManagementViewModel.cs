namespace EduNexus.ViewModels.AssignmentQuestion;

public class AssignmentQuestionManagementViewModel
{
    public long AssignmentId { get; set; }

    public string AssignmentTitle { get; set; } = "";

    public List<AssignmentQuestionItemViewModel> CurrentQuestions { get; set; }
        = new();

    public List<AssignmentQuestionItemViewModel> AvailableQuestions { get; set; }
        = new();
}