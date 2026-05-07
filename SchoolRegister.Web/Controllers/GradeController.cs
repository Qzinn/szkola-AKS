using Microsoft.AspNetCore.Mvc;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Web.Controllers;

public class GradeController : Controller
{
    private readonly IGradeService _gradeService;

    public GradeController(IGradeService gradeService)
    {
        _gradeService = gradeService;
    }

    public IActionResult Index()
    {
        return RedirectToAction(nameof(Create));
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(AddGradeToStudentVm vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        _gradeService.AddGradeToStudent(vm);

        return RedirectToAction(nameof(Create));
    }

    [HttpGet]
    public IActionResult StudentGrades(int studentId, int getterUserId)
    {
        var vm = new GetGradesReportVm
        {
            StudentId = studentId,
            GetterUserId = getterUserId
        };

        var result = _gradeService.GetGradesReportForStudent(vm);

        return View(result);
    }
}