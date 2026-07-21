using EduNexus.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Controllers
{
    public class HomeController : Controller
    {
        private readonly EduNexusContext _context;

        public HomeController(EduNexusContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var roleCount = await _context.Roles.CountAsync();

            ViewBag.RoleCount = roleCount;

            return View();
        }
    }
}