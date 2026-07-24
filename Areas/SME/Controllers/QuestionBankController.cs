using EduNexus.Models;
using EduNexus.Services.SME.Interfaces;
using EduNexus.ViewModels.QuestionBank;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EduNexus.Areas.SME.Controllers;

[Area("SME")]
public class QuestionBankController : Controller
{
    private readonly IQuestionBankService _questionBankService;

    public QuestionBankController(IQuestionBankService questionBankService)
    {
        _questionBankService = questionBankService;
    }

    //=========================================
    // LIST
    //=========================================

    public async Task<IActionResult> Index()
    {
       var banks = await _questionBankService.GetAllAsync();

        return View(banks);
    }


    //=========================================
    // DETAILS
    //=========================================

    public async Task<IActionResult> Details(long id)
    {
        var bank = await _questionBankService.GetByIdAsync(id);

        if (bank == null)
            return NotFound();

        return View(bank);
    }

    //=========================================
    // CREATE
    //=========================================

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var courses = await _questionBankService.GetCoursesAsync();

        ViewBag.Courses = new SelectList(
            courses,
            "CourseId",
            "Title");

        return View(new QuestionBankViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(QuestionBankViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            var courses = await _questionBankService.GetCoursesAsync();

            ViewBag.Courses = new SelectList(
                courses,
                "CourseId",
                "Title",
                vm.CourseId);

            return View(vm);
        }

        QuestionBank bank = new()
        {
            CourseId = vm.CourseId,
            BankName = vm.BankName,
            Description = vm.Description,
            IsPublic = vm.IsPublic,
            CreatedBy = 1
        };

        await _questionBankService.CreateAsync(bank);

        return RedirectToAction(nameof(Index));
    }

    //=========================================
    // EDIT
    //=========================================

    [HttpGet]
    public async Task<IActionResult> Edit(long id)
    {
        var bank = await _questionBankService.GetByIdAsync(id);

        if (bank == null)
            return NotFound();

        var courses = await _questionBankService.GetCoursesAsync();

        ViewBag.Courses = new SelectList(
            courses,
            "CourseId",
            "Title",
            bank.CourseId);

        QuestionBankViewModel vm = new()
        {
            QuestionBankId = bank.QuestionBankId,
            CourseId = bank.CourseId,
            BankName = bank.BankName,
            Description = bank.Description,
            IsPublic = bank.IsPublic
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(QuestionBankViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            var courses = await _questionBankService.GetCoursesAsync();

            ViewBag.Courses = new SelectList(
                courses,
                "CourseId",
                "Title",
                vm.CourseId);

            return View(vm);
        }

        var bank = await _questionBankService.GetByIdAsync(vm.QuestionBankId);

        if (bank == null)
            return NotFound();

        bank.CourseId = vm.CourseId;
        bank.BankName = vm.BankName;
        bank.Description = vm.Description;
        bank.IsPublic = vm.IsPublic;

        await _questionBankService.UpdateAsync(bank);

        return RedirectToAction(nameof(Index));
    }

    //=========================================
    // DELETE
    //=========================================

    [HttpPost]
    public async Task<IActionResult> Delete(long id)
    {
        await _questionBankService.DeleteAsync(id);

        return RedirectToAction(nameof(Index));
    }
}