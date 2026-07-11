using System.ComponentModel.DataAnnotations;

namespace EduNexus.ViewModels.Questions
{
    public class ChoiceViewModel
    {
        public long ChoiceId { get; set; }
        public string ChoiceContent { get; set; } = null!;
        public bool? IsCorrect { get; set; }
        public int? DisplayOrder { get; set; }
    }

    public class ChoiceFormViewModel
    {
        public long ChoiceId { get; set; }
        public long QuestionId { get; set; }

        [Required(ErrorMessage = "Choice content cannot be empty")]
        [Display(Name = "Choice Content")]
        public string ChoiceContent { get; set; } = null!;

        [Display(Name = "Is Correct?")]
        public bool IsCorrect { get; set; }

        public int? DisplayOrder { get; set; }
    }
}
