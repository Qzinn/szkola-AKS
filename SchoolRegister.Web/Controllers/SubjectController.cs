using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Web.Controllers;

[Authorize]
public class SubjectController : Controller
{
    private readonly ISubjectService _subjectService;

    public SubjectController(ISubjectService subjectService)
    {
        _subjectService = subjectService;
    }

    public IActionResult Index()
    {
        var subjects = _subjectService.GetSubjects();

        return View(subjects);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult AddOrEditSubject(int? id)
    {
        if (id == null)
        {
            return View(new AddOrUpdateSubjectVm());
        }

        var subject = _subjectService.GetSubject(x => x.Id == id);

        if (subject == null)
        {
            return NotFound();
        }

        var vm = new AddOrUpdateSubjectVm()
        {
            Id = subject.Id,
            Name = subject.Name,
            Description = subject.Description,
            TeacherId = subject.TeacherId ?? 0
        };

        return View(vm);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult AddOrEditSubject(AddOrUpdateSubjectVm vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        _subjectService.AddOrUpdateSubject(vm);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Details(int id)
    {
        var subject = _subjectService.GetSubject(x => x.Id == id);

        if (subject == null)
        {
            return NotFound();
        }

        return View(subject);
    }
}