using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EduNexus.ViewModels.Quizzes
{
    public class QuizListViewModel
    {
        public long QuizId { get; set; }
        public string QuizTitle { get; set; } = null!;
        public int Duration { get; set; }
        public decimal? PassingScore { get; set; }
        public int QuestionCount { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

    public class QuizFormViewModel
    {
        public long QuizId { get; set; }

        [Required]
        [Display(Name = "Assignment")]
        public long AssignmentId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200)]
        [Display(Name = "Quiz Title")]
        public string QuizTitle { get; set; } = null!;

        public string? Description { get; set; }

        [Required]
        [Range(1, 1440, ErrorMessage = "Duration must be between 1 and 1440 minutes")]
        [Display(Name = "Duration (minutes)")]
        public int Duration { get; set; }

        [Display(Name = "Passing Score")]
        [Range(0, 100)]
        public decimal? PassingScore { get; set; }

        [Display(Name = "Shuffle Questions")]
        public bool ShuffleQuestion { get; set; }

        [Display(Name = "Shuffle Answers")]
        public bool ShuffleAnswer { get; set; }
    }
    
    public class QuizQuestionSelectionViewModel
    {
        public long QuizId { get; set; }
        public long BankId { get; set; }
        public List<QuizQuestionItemViewModel> AvailableQuestions { get; set; } = new List<QuizQuestionItemViewModel>();
        public List<long> SelectedQuestionIds { get; set; } = new List<long>();
    }

    public class QuizQuestionItemViewModel
    {
        public long QuestionId { get; set; }
        public string QuestionContent { get; set; } = null!;
        public string QuestionType { get; set; } = null!;
        public bool IsAlreadyInQuiz { get; set; }
    }
}
