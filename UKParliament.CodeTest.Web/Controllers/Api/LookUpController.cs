using Microsoft.AspNetCore.Mvc;
using UKParliament.CodeTest.Data.Models;
using UKParliament.CodeTest.Data.ViewModels;
using UKParliament.CodeTest.Services.Services.Interfaces;

namespace UKParliament.CodeTest.Web.Controllers.Api;

[Route("api/[controller]")]
[ApiController]
public class LookUpController(ILookUpService lookUpService) : ControllerBase
{
    [HttpGet]
    [Route("")]
    public ActionResult<IEnumerable<string>> GetItemsToLookFor()
    {
        return Ok(lookUpService.GetLookupItems());
    }

    [HttpGet]
    [Route("[action]")]
    public ActionResult<IEnumerable<LookupItem>> Search([FromQuery] LookupItemsEnum? item)
    {
        if (item is null)
        {
            return BadRequest("Lookup item cannot be null");
        }

        return Ok(lookUpService.LookupItem(item));
    }
}
