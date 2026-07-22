using EduNexus.Areas.Admin.ViewModels.CourseReview;

namespace EduNexus.Services.Administration.Interfaces
{
    public interface ICourseReviewService
    {
        Task<CourseReviewIndexViewModel> GetCoursesAsync(
            CourseReviewFilterViewModel filter);
    }
}