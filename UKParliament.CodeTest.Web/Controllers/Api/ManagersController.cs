using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using UKParliament.CodeTest.Data.HATEOAS.Interfaces;
using UKParliament.CodeTest.Data.ViewModels;
using UKParliament.CodeTest.Services.HATEOAS.Interfaces;
using UKParliament.CodeTest.Services.Services.Interfaces;

namespace UKParliament.CodeTest.Web.Controllers.Api;

[Route("api/[controller]")]
[ApiController]
public class ManagersController(
    IManagerService managerService,
    IResourceService<ManagerViewModel> resourceService,
    IValidator<EmployeeViewModel> validator
) : ControllerBase
{
    private string GetControllerName() =>
        ControllerContext.RouteData.Values["controller"]?.ToString()?.ToLower() ?? "";

    [HttpGet]
    [Route("")]
    public async Task<ActionResult<IEnumerable<ShortManagerViewModel>>> GetAll()
    {
        return await Task.FromResult(Ok(managerService.GetAll()));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IResource<ManagerViewModel>>> View(int id)
    {
        var result = await managerService.Get(id);
        if (result is null)
        {
            return NotFound();
        }

        var resource = resourceService.GenerateResource(result, GetControllerName());
        return Ok(resource);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<IResource<ManagerViewModel>>> Put(
        [FromBody] ManagerViewModel person
    )
    {
        var validation = await validator.ValidateAsync(person);
        if (!validation.IsValid)
        {
            return BadRequest(validation);
        }

        var result = await managerService.Update(person);
        if (result is null)
        {
            return BadRequest("Employee not found");
        }

        var resource = resourceService.GenerateResource(result, GetControllerName());
        return Ok(resource);
    }
}
