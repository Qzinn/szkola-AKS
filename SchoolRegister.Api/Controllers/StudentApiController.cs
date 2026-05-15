using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Api.Controllers;

[Authorize]
public class StudentApiController : BaseApiController
{
    private readonly IStudentService _studentService;

    private readonly IGradeService _gradeService;

    public StudentApiController(
        ILogger logger,
        IMapper mapper,
        IStudentService studentService,
        IGradeService gradeService) : base(logger, mapper)
    {
        _studentService = studentService;
        _gradeService = gradeService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            var students = _studentService.GetStudents();

            return Ok(students);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }

    [HttpPost("grades")]
    public IActionResult Grades(
        [FromBody] GetGradesReportVm vm)
    {
        try
        {
            var grades =
                _gradeService.GetGradesReportForStudent(vm);

            return Ok(grades);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }
}