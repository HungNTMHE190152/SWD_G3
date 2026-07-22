using EduNexus.Areas.Admin.ViewModels.CourseReview;
using EduNexus.Services.Common;

namespace EduNexus.Services.Administration.Interfaces
{
    public interface ICourseReviewService
    {
        Task<CourseReviewIndexViewModel> GetCoursesAsync(
            CourseReviewFilterViewModel filter);

        Task<CourseReviewDetailsViewModel?> GetDetailsAsync(
            long courseId);

        Task<ServiceResult> ApproveAsync(
            long courseId,
            long reviewedBy,
            string? reviewComment);

        Task<ServiceResult> RejectAsync(
            long courseId,
            long reviewedBy,
            string? reviewComment);
    }
}