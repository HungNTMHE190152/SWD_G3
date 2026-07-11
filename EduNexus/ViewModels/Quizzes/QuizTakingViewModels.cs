using System;
using System.Collections.Generic;

namespace EduNexus.ViewModels.Quizzes
{
    public class NewQuizViewModel
    {
        public long QuizId { get; set; }
        public string QuizTitle { get; set; } = null!;
        public string? Description { get; set; }
        public int Duration { get; set; }
        public decimal? PassingScore { get; set; }
        public int QuestionCount { get; set; }
        
        public bool HasPreviousAttempts { get; set; }
        public int PreviousAttemptCount { get; set; }
    }

    public class QuizTakingViewModel
    {
        public long AttemptId { get; set; }
        public long QuizId { get; set; }
        public string QuizTitle { get; set; } = null!;
        public int Duration { get; set; }
        public DateTime StartTime { get; set; }
        
        public List<QuizTakingQuestionViewModel> Questions { get; set; } = new List<QuizTakingQuestionViewModel>();
    }

    public class QuizTakingQuestionViewModel
    {
        public long QuestionId { get; set; }
        public string QuestionContent { get; set; } = null!;
        public string QuestionType { get; set; } = null!;
        public List<QuizTakingChoiceViewModel> Choices { get; set; } = new List<QuizTakingChoiceViewModel>();
        
        // Form binding
        public long? SelectedChoiceId { get; set; }
        public string? AnswerText { get; set; }
    }

    public class QuizTakingChoiceViewModel
    {
        public long ChoiceId { get; set; }
        public string ChoiceContent { get; set; } = null!;
    }

    public class QuizSubmitViewModel
    {
        public long AttemptId { get; set; }
        // We will bind answers using a dictionary
        public Dictionary<long, long?> SelectedChoices { get; set; } = new Dictionary<long, long?>();
        public Dictionary<long, string?> TextAnswers { get; set; } = new Dictionary<long, string?>();
    }
}
