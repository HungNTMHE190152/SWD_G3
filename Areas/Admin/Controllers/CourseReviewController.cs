using EduNexus.Areas.Admin.ViewModels.CourseReview;
using EduNexus.Constants;
using EduNexus.Services.Administration.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = RoleNames.Admin)]
    public class CourseReviewController : Controller
    {
        private readonly ICourseReviewService
            _courseReviewService;

        public CourseReviewController(
            ICourseReviewService courseReviewService)
        {
            _courseReviewService =
                courseReviewService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            [FromQuery] CourseReviewFilterViewModel filter)
        {
            CourseReviewIndexViewModel viewModel =
                await _courseReviewService
                    .GetCoursesAsync(filter);

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Details(long id)
        {
            return Content(
                $"Course Review Details will be implemented next. "
                + $"CourseId = {id}");
        }
    }
}