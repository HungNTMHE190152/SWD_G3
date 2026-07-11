using EduNexus.Constants;
using EduNexus.Services.Interfaces;
using EduNexus.ViewModels.Questions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EduNexus.Controllers;

[Authorize(Roles = RoleNames.Sme)]
public class ChoiceController : Controller
{
    private readonly IChoiceService _choiceService;

    public ChoiceController(IChoiceService choiceService)
    {
        _choiceService = choiceService;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(long id, long questionId)
    {
        await _choiceService.DeleteChoiceAsync(id);
        return RedirectToAction("Edit", "Question", new { id = questionId });
    }
}
