using EduNexus.Constants;
using EduNexus.Data;
using EduNexus.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EduNexus.Controllers;

// TODO: Khi Person 1 hoàn thiện Auth, đổi lại thành [Authorize(Roles = RoleNames.Sme)]
[AllowAnonymous]
public class QuestionImportController : Controller
{
    private readonly EduNexusContext _context;

    public QuestionImportController(EduNexusContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index(long bankId)
    {
        ViewBag.BankId = bankId;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Import(long bankId)
    {
        long demoSmeId = 2; // fake user

        // Create a mock import history record
        var history = new QuestionImportHistory
        {
            BankId = bankId,
            ImportedBy = demoSmeId,
            ImportedAt = DateTime.Now,
            TotalQuestions = 10,
            SuccessQuestions = 8,
            FailedQuestions = 2
        };

        _context.QuestionImportHistories.Add(history);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Question", new { bankId = bankId });
    }
}
