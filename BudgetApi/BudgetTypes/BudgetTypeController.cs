using Budget.Models.ExceptionTypes;
using BudgetApi.Models;
using BudgetApi.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.RegularExpressions;

namespace BudgetApi.BudgetTypes;

[Authorize]
[ApiController]
//[Route("[controller]")]
public class BudgetTypeController : ControllerBase
{
    private readonly IBudgetTypeService _budgetService;
    private readonly IBudgetAuthorizationService _authorizationService;


    public BudgetTypeController(IBudgetTypeService budgetService, IBudgetAuthorizationService authorizationService)
    {
        _budgetService = budgetService;
        _authorizationService = authorizationService;
    }

    [Route("group/{groupId}/[controller]")]
    [HttpPost]
    public ActionResult<int> AddBudgetType(int groupId, [FromBody] BudgetType budgetType)
    {
        try
        {
            if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
            {
                return Unauthorized();
            }

            return _budgetService.AddBudgetType(budgetType);
        }
        catch (UserNotFoundException)
        {
            return Unauthorized();
        }
    }

    [Route("group/{groupId}/[controller]/{budgetTypeId}")]
    [HttpPut]
    public ActionResult UpdateBudgetType([FromBody] BudgetType budgetType, int groupId, int budgetTypeId = -1)
    {
        try
        {
            if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
            {
                return Unauthorized();
            }

            _budgetService.UpdateBudgetType(budgetType);
            return Ok();
        }
        catch (UserNotFoundException)
        {
            return Unauthorized();
        }
    }

    [Route("group/{groupId}/[controller]/{budgetTypeId}")]
    [HttpDelete]
    public ActionResult DeleteBudgetTypeEntry(int groupId, int budgetTypeId)
    {
        try
        {
            if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
            {
                return Unauthorized();
            }

            _budgetService.DeleteBudgetTypeEntry(budgetTypeId);
            return Ok();
        }
        catch (UserNotFoundException)
        {
            return Unauthorized();
        }
    }

    [Route("group/{groupId}/[controller]/{budgetTypeId}")]
    [HttpGet]
    public ActionResult<BudgetType> GetBudgetType(int groupId, int budgetTypeId)
    {
        try
        {
            if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
            {
                return Unauthorized();
            }

            return _budgetService.GetBudgetType(budgetTypeId);
        }
        catch (UserNotFoundException)
        {
            return Unauthorized();
        }
    }
    private string ExternalLoginId => HttpContext
            .User.Claims.FirstOrDefault(x => x.Type == "user_id")?.Value;
}
