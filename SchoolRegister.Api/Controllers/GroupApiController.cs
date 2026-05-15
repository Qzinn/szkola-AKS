using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Api.Controllers;

[Authorize(Roles = "Admin")]
public class GroupApiController : BaseApiController
{
    private readonly IGroupService _groupService;

    public GroupApiController(
        ILogger logger,
        IMapper mapper,
        IGroupService groupService) : base(logger, mapper)
    {
        _groupService = groupService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            var groups = _groupService.GetGroups();

            return Ok(groups);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }

    [HttpPost]
    public IActionResult Add(
        [FromBody] AddOrUpdateGroupVm vm)
    {
        try
        {
            var group =
                _groupService.AddOrUpdateGroup(vm);

            return Ok(group);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }

    [HttpPut]
    public IActionResult Update(
        [FromBody] AddOrUpdateGroupVm vm)
    {
        try
        {
            var group =
                _groupService.AddOrUpdateGroup(vm);

            return Ok(group);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }
}