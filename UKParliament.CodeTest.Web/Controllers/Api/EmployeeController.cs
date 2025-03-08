using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using UKParliament.CodeTest.Data.HATEOAS;
using UKParliament.CodeTest.Data.HATEOAS.Interfaces;
using UKParliament.CodeTest.Data.Requests;
using UKParliament.CodeTest.Data.ViewModels;
using UKParliament.CodeTest.Services.HATEOAS.Interfaces;
using UKParliament.CodeTest.Services.Services.Interfaces;

namespace UKParliament.CodeTest.Web.Controllers.Api;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController(
    IEmployeeService employeeService,
    IResourceService<EmployeeViewModel> resourceService,
    IValidator<EmployeeViewModel> validator
) : ControllerBase
{
    private string GetControllerName() =>
        ControllerContext.RouteData.Values["controller"]?.ToString()?.ToLower() ?? "";

    [HttpGet]
    public async Task<
        ActionResult<IResource<IResourceCollection<IResource<EmployeeViewModel>>>>
    > Search([FromQuery] SearchRequest? request)
    {
        var results = await employeeService.Search(request);
        var collection = new ResourceCollection<IResource<EmployeeViewModel>>
        {
            Pagination = results.Pagination,
            Results = results.Results.Select(d =>
                resourceService.GenerateResource(d, GetControllerName())
            ),
        };
        var resource = resourceService.GenerateCollectionResource(collection, GetControllerName());
        return Ok(resource);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IResource<EmployeeViewModel>>> View(int id)
    {
        var result = await employeeService.View(id);
        if (result is null)
        {
            return BadRequest("Employee not found");
        }

        var resource = resourceService.GenerateResource(result, GetControllerName());
        return Ok(resource);
    }

    [HttpPost]
    public async Task<ActionResult<IResource<EmployeeViewModel>>> Post(
        [FromBody] EmployeeViewModel person
    )
    {
        var validation = await validator.ValidateAsync(person);
        if (!validation.IsValid)
        {
            return BadRequest(validation);
        }

        var result = await employeeService.Create(person);
        if (result is null)
        {
            return BadRequest("Failed to create employee");
        }

        var resource = resourceService.GenerateResource(result, GetControllerName());
        return Ok(resource);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<IResource<EmployeeViewModel>>> Put(
        [FromBody] EmployeeViewModel person
    )
    {
        var validation = await validator.ValidateAsync(person);
        if (!validation.IsValid)
        {
            return BadRequest(validation);
        }

        var result = await employeeService.Update(person);
        if (result is null)
        {
            return BadRequest("Employee not found");
        }

        var resource = resourceService.GenerateResource(result, GetControllerName());
        return Ok(resource);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await employeeService.Delete(id);

        return NoContent();
    }
}
