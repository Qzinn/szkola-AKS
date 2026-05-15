using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Api.Controllers;

[Authorize]
public class GradeApiController : BaseApiController
{
    private readonly IGradeService _gradeService;

    public GradeApiController(
        ILogger logger,
        IMapper mapper,
        IGradeService gradeService) : base(logger, mapper)
    {
        _gradeService = gradeService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            var grades = _gradeService.GetGrades();

            return Ok(grades);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }

    [HttpPost]
    [Authorize(Roles = "Teacher")]
    public IActionResult AddGrade(
        [FromBody] AddGradeToStudentVm vm)
    {
        try
        {
            var grade =
                _gradeService.AddGradeToStudent(vm);

            return Ok(grade);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }

    [HttpPost("report")]
    public IActionResult Report(
        [FromBody] GetGradesReportVm vm)
    {
        try
        {
            var report =
                _gradeService.GetGradesReportForStudent(vm);

            return Ok(report);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }
}