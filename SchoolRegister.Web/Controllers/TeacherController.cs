using Microsoft.AspNetCore.Mvc;
using SchoolRegister.Services.Interfaces;

namespace SchoolRegister.Web.Controllers;

public class TeacherController : Controller
{
    private readonly ITeacherService _teacherService;

    public TeacherController(ITeacherService teacherService)
    {
        _teacherService = teacherService;
    }

    public IActionResult Index()
    {
        var teachers = _teacherService.GetTeachers();

        return View(teachers);
    }
}