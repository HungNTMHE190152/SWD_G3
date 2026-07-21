using EduNexus.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = RoleNames.Admin)]
    public class CourseReviewController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Content(
                "Course Review List will be implemented next.");
        }

        [HttpGet]
        public IActionResult Details(long id)
        {
            return Content(
                $"Course Review Details: CourseId = {id}");
        }
    }
}