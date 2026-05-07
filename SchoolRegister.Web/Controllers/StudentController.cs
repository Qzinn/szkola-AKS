using Microsoft.AspNetCore.Mvc;
using SchoolRegister.Services.Interfaces;

namespace SchoolRegister.Web.Controllers;

public class StudentController : Controller
{
    private readonly IStudentService _studentService;

    public StudentController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    public IActionResult Index()
    {
        var students = _studentService.GetStudents();

        return View(students);
    }
}