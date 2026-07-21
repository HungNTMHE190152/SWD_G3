using EduNexus.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = RoleNames.Teacher)]
    public class DashboardController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}