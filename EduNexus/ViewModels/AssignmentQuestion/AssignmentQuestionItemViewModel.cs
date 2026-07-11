namespace EduNexus.ViewModels.AssignmentQuestion;

public class AssignmentQuestionItemViewModel
{
    public long AssignmentQuestionId { get; set; }

    public long AssignmentId { get; set; }

    public long QuestionId { get; set; }

    public string QuestionContent { get; set; } = "";

    public string QuestionType { get; set; } = "";

    public string? Difficulty { get; set; }

    public decimal? Score { get; set; }

    public int? DisplayOrder { get; set; }
}