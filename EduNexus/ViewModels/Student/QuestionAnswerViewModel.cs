namespace EduNexus.ViewModels.Student
{
    public class QuestionAnswerViewModel
    {
        public long QuestionId { get; set; }
        public string QuestionContent { get; set; } = string.Empty;
        public string QuestionType { get; set; } = string.Empty;
        public string? Answer { get; set; }
        public int DisplayOrder { get; set; }
    }
}
