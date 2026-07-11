namespace EduNexus.ViewModels.Student
{
    public class QuestionItemViewModel
    {
        public long QuestionId { get; set; }
        public string QuestionContent { get; set; } = string.Empty;
        public string QuestionType { get; set; } = string.Empty;
        public string? Difficulty { get; set; }
        public decimal? Score { get; set; }
    }
}
