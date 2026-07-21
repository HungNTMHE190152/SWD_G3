using EduNexus.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNexus.Areas.SME.Controllers
{
    [Area("SME")]
    [Authorize(Roles = RoleNames.Sme)]
    public class DashboardController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}