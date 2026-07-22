using EduNexus.Areas.Admin.ViewModels.CourseReview;
using EduNexus.Constants;
using EduNexus.Extensions;
using EduNexus.Services.Administration.Interfaces;
using EduNexus.Services.Common;
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
        public async Task<IActionResult> Details(long id)
        {
            CourseReviewDetailsViewModel? viewModel =
                await _courseReviewService
                    .GetDetailsAsync(id);

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(
            CourseReviewDecisionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] =
                    GetFirstModelError();

                return RedirectToAction(
                    nameof(Details),
                    new { id = model.CourseId });
            }

            long currentAdminUserId =
                User.GetCurrentUserId();

            ServiceResult result =
                await _courseReviewService
                    .ApproveAsync(
                        model.CourseId,
                        currentAdminUserId,
                        model.ReviewComment);

            SetTempData(result);

            return RedirectToAction(
                nameof(Details),
                new { id = model.CourseId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(
            CourseReviewDecisionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] =
                    GetFirstModelError();

                return RedirectToAction(
                    nameof(Details),
                    new { id = model.CourseId });
            }

            long currentAdminUserId =
                User.GetCurrentUserId();

            ServiceResult result =
                await _courseReviewService
                    .RejectAsync(
                        model.CourseId,
                        currentAdminUserId,
                        model.ReviewComment);

            SetTempData(result);

            return RedirectToAction(
                nameof(Details),
                new { id = model.CourseId });
        }

        private void SetTempData(ServiceResult result)
        {
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] =
                    result.Message;
            }
            else
            {
                TempData["ErrorMessage"] =
                    result.Message;
            }
        }

        private string GetFirstModelError()
        {
            return ModelState.Values
                .SelectMany(value => value.Errors)
                .Select(error =>
                    error.ErrorMessage)
                .FirstOrDefault()
                ?? "The submitted data is invalid.";
        }
    }
}