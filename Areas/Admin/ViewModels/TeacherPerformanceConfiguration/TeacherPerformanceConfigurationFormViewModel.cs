using System.ComponentModel.DataAnnotations;

namespace EduNexus.Areas.Admin.ViewModels
    .TeacherPerformanceConfiguration
{
    public class TeacherPerformanceConfigurationFormViewModel
        : IValidatableObject
    {
        [Display(Name = "Student feedback weight")]
        [Range(
            typeof(decimal),
            "0",
            "100",
            ErrorMessage =
                "Student feedback weight must be between 0 and 100.")]
        public decimal FeedbackPercentage { get; set; } = 60m;

        [Display(Name = "Classroom completion weight")]
        [Range(
            typeof(decimal),
            "0",
            "100",
            ErrorMessage =
                "Classroom completion weight must be between 0 and 100.")]
        public decimal CompletionPercentage { get; set; } = 20m;

        [Display(Name = "Student performance weight")]
        [Range(
            typeof(decimal),
            "0",
            "100",
            ErrorMessage =
                "Student performance weight must be between 0 and 100.")]
        public decimal StudentPerformancePercentage { get; set; } = 20m;

        [Display(Name = "Minimum evaluation count")]
        [Range(
            1,
            int.MaxValue,
            ErrorMessage =
                "Minimum evaluation count must be greater than zero.")]
        public int MinimumEvaluationCount { get; set; } = 5;

        public decimal TotalPercentage =>
            FeedbackPercentage
            + CompletionPercentage
            + StudentPerformancePercentage;

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
                        nameof(FeedbackPercentage),
                        nameof(CompletionPercentage),
                        nameof(StudentPerformancePercentage)
                    });
            }
        }
    }
}