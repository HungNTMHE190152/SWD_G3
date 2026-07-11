using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EduNexus.ViewModels.Questions
{
    public class QuestionListViewModel
    {
        public long QuestionId { get; set; }
        public long BankId { get; set; }
        public string QuestionContent { get; set; } = null!;
        public string QuestionType { get; set; } = null!;
        public string? Difficulty { get; set; }
        public bool? IsAigenerated { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int ChoiceCount { get; set; }
    }

    public class QuestionFormViewModel
    {
        public long QuestionId { get; set; }
        public long BankId { get; set; }

        [Required(ErrorMessage = "Question content cannot be empty")]
        [Display(Name = "Question Content")]
        public string QuestionContent { get; set; } = null!;

        [Required]
        [StringLength(30)]
        [Display(Name = "Question Type")]
        public string QuestionType { get; set; } = "Single Choice";

        [StringLength(20)]
        public string? Difficulty { get; set; }

        public string? Explanation { get; set; }

        public bool IsAigenerated { get; set; }

        // Choices can be added dynamically
        public List<ChoiceFormViewModel> Choices { get; set; } = new List<ChoiceFormViewModel>();
    }

    public class QuestionDetailViewModel
    {
        public long QuestionId { get; set; }
        public string QuestionContent { get; set; } = null!;
        public string QuestionType { get; set; } = null!;
        public string? Difficulty { get; set; }
        public string? Explanation { get; set; }
        public List<ChoiceViewModel> Choices { get; set; } = new List<ChoiceViewModel>();
    }
}
