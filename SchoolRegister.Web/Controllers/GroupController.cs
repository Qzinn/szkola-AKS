using Microsoft.AspNetCore.Mvc;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Web.Controllers;

public class GroupController : Controller
{
    private readonly IGroupService _groupService;

    public GroupController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    public IActionResult Index()
    {
        var groups = _groupService.GetGroups();

        return View(groups);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(AddOrUpdateGroupVm vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        _groupService.AddOrUpdateGroup(vm);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult AttachStudent()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AttachStudent(AttachDetachStudentToGroupVm vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        _groupService.AttachStudentToGroup(vm);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult DetachStudent()
    {
        return View();
    }

    [HttpPost]
    public IActionResult DetachStudent(AttachDetachStudentToGroupVm vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        _groupService.DetachStudentFromGroup(vm);

        return RedirectToAction(nameof(Index));
    }
}