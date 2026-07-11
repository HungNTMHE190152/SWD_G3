using System.Collections.Generic;

namespace EduNexus.ViewModels.Questions
{
    public class AiQuestionStagingViewModel
    {
        public long BankId { get; set; }
        public string PromptText { get; set; } = null!;
        public List<AiQuestionItemViewModel> GeneratedQuestions { get; set; } = new List<AiQuestionItemViewModel>();
    }

    public class AiQuestionItemViewModel
    {
        public string QuestionContent { get; set; } = null!;
        public string QuestionType { get; set; } = null!;
        public string? Difficulty { get; set; }
        public string? Explanation { get; set; }
        public List<ChoiceFormViewModel> Choices { get; set; } = new List<ChoiceFormViewModel>();
        public bool IsSelected { get; set; } = true;
    }
}
