using System.ComponentModel.DataAnnotations;

namespace EduNexus.Areas.Admin.ViewModels.RankingConfiguration
{
    public class RankingConfigurationFormViewModel
        : IValidatableObject
    {
        [Display(Name = "Assignment weight")]
        [Range(
            typeof(decimal),
            "0",
            "100",
            ErrorMessage =
                "Assignment weight must be between 0 and 100.")]
        public decimal AssignmentPercentage { get; set; } = 50m;

        [Display(Name = "Quiz weight")]
        [Range(
            typeof(decimal),
            "0",
            "100",
            ErrorMessage =
                "Quiz weight must be between 0 and 100.")]
        public decimal QuizPercentage { get; set; } = 30m;

        [Display(Name = "Lesson progress weight")]
        [Range(
            typeof(decimal),
            "0",
            "100",
            ErrorMessage =
                "Lesson progress weight must be between 0 and 100.")]
        public decimal LessonProgressPercentage { get; set; } = 20m;

        public decimal TotalPercentage =>
            AssignmentPercentage
            + QuizPercentage
            + LessonProgressPercentage;

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (TotalPercentage != 100m)
            {
                yield return new ValidationResult(
                    $"The total weight must equal 100%. "
                    + $"Current total: {TotalPercentage}%.",
                    new[]
                    {
                        nameof(AssignmentPercentage),
                        nameof(QuizPercentage),
                        nameof(LessonProgressPercentage)
                    });
            }
        }
    }
}