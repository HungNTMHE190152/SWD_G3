using EduNexus.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Roles = RoleNames.Student)]
    public class DashboardController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}