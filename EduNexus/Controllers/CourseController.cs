using EduNexus.Constants;
using EduNexus.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Controllers;

[Authorize(Roles = RoleNames.Sme)]
public class CourseController : Controller
{
    private readonly ICourseService _courseService;

    public CourseController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    public async Task<IActionResult> Index()
    {
        long demoSmeId = 2;

        var courses = await _courseService.GetCoursesBySmeAsync(demoSmeId);
        return View(courses);
    }

    public async Task<IActionResult> Structure(long id)
    {
        var course = await _courseService.GetCourseStructureAsync(id);

        if (course == null)
        {
            return NotFound();
        }

        return View(course);
    }
}