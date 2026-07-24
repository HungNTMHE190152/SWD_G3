using EduNexus.Models;
using EduNexus.Services.SME.Interfaces;
using EduNexus.ViewModels.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EduNexus.Areas.SME.Controllers
{
    [Area("SME")]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        // ===========================
        // Course List
        // ===========================

        public async Task<IActionResult> Index()
        {
            var courses = await _courseService.GetAllAsync();

            return View(courses);
        }

        // ===========================
        // Details
        // ===========================

        public async Task<IActionResult> Details(long id)
        {
            var course = await _courseService.GetByIdAsync(id);

            if (course == null)
                return NotFound();

            return View(course);
        }

        // ===========================
        // Create
        // ===========================

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadCategoryDropdown();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoryDropdown();

                return View(model);
            }

            Course course = new Course()
            {
                CategoryId = model.CategoryId,

                CourseCode = model.CourseCode,

                Title = model.Title,

                Description = model.Description,

                ThumbnailUrl = model.ThumbnailUrl,

                // TODO
                // Sau này lấy User đang login
                CreatedBy = 1
            };

            bool result = await _courseService.CreateAsync(course);

            if (result)
            {
                TempData["Success"] = "Course created successfully.";

                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Create course failed.";

            await LoadCategoryDropdown();

            return View(model);
        }

        // ===========================
        // Edit
        // ===========================

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var course = await _courseService.GetByIdAsync(id);

            if (course == null)
                return NotFound();

            CourseViewModel model = new()
            {
                CourseId = course.CourseId,

                CategoryId = course.CategoryId,

                CourseCode = course.CourseCode,

                Title = course.Title,

                Description = course.Description,

                ThumbnailUrl = course.ThumbnailUrl
            };

            await LoadCategoryDropdown();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CourseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoryDropdown();

                return View(model);
            }

            var course = await _courseService.GetByIdAsync(model.CourseId);

            if (course == null)
                return NotFound();

            course.CategoryId = model.CategoryId;

            course.CourseCode = model.CourseCode;

            course.Title = model.Title;

            course.Description = model.Description;

            course.ThumbnailUrl = model.ThumbnailUrl;

            bool result = await _courseService.UpdateAsync(course);

            if (result)
            {
                TempData["Success"] = "Course updated.";

                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Update failed.";

            await LoadCategoryDropdown();

            return View(model);
        }

        // ===========================
        // Delete
        // ===========================

        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            bool result = await _courseService.DeleteAsync(id);

            if (result)
            {
                TempData["Success"] = "Course deleted.";
            }
            else
            {
                TempData["Error"] = "Delete failed.";
            }

            return RedirectToAction(nameof(Index));
        }

        // ===========================
        // Submit
        // ===========================

        [HttpPost]
        public async Task<IActionResult> SubmitForReview(long id)
        {
            bool result = await _courseService.SubmitForReviewAsync(id);

            if (result)
            {
                TempData["Success"] = "Course submitted for review.";
            }
            else
            {
                TempData["Error"] = "Submit failed.";
            }

            return RedirectToAction(nameof(Index));
        }

        // ===========================
        // Helper
        // ===========================

        private async Task LoadCategoryDropdown()
        {
            var categories = await _courseService.GetCategoriesAsync();

            ViewBag.Categories = new SelectList(
                categories,
                "CategoryId",
                "CategoryName");
        }
    }
}