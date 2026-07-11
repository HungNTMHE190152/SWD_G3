using EduNexus.Data;
using EduNexus.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly EduNexusContext _context;

    public ProfileController(EduNexusContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        long userId = User.GetCurrentUserId();

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.UserId == userId);

        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }
}