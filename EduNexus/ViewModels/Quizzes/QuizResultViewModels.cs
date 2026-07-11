using System;
using System.Collections.Generic;

namespace EduNexus.ViewModels.Quizzes
{
    public class QuizResultViewModel
    {
        public long AttemptId { get; set; }
        public long QuizId { get; set; }
        public string QuizTitle { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime? SubmitTime { get; set; }
        public decimal? TotalScore { get; set; }
        public decimal? PassingScore { get; set; }
        public string? Status { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
    }

    public class QuizHistoryViewModel
    {
        public long QuizId { get; set; }
        public string QuizTitle { get; set; } = null!;
        public List<QuizResultViewModel> Attempts { get; set; } = new List<QuizResultViewModel>();
    }

    public class QuizReviewViewModel
    {
        public long AttemptId { get; set; }
        public string QuizTitle { get; set; } = null!;
        public decimal? TotalScore { get; set; }
        public List<QuizReviewQuestionViewModel> Questions { get; set; } = new List<QuizReviewQuestionViewModel>();
    }

    public class QuizReviewQuestionViewModel
    {
        public long QuestionId { get; set; }
        public string QuestionContent { get; set; } = null!;
        public string QuestionType { get; set; } = null!;
        public string? Explanation { get; set; }
        
        public long? SelectedChoiceId { get; set; }
        public string? AnswerText { get; set; }
        
        public bool? IsCorrect { get; set; }
        public decimal? Score { get; set; }

        public List<QuizReviewChoiceViewModel> Choices { get; set; } = new List<QuizReviewChoiceViewModel>();
    }

    public class QuizReviewChoiceViewModel
    {
        public long ChoiceId { get; set; }
        public string ChoiceContent { get; set; } = null!;
        public bool IsCorrect { get; set; }
        public bool IsSelected { get; set; }
    }
}
