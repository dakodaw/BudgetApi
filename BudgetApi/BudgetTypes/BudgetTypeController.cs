using BudgetApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetApi.BudgetTypes;

[Authorize]
[ApiController]
[Route("[controller]")]
public class BudgetTypeController : ControllerBase
{
    IBudgetTypeService _budgetService;

    public BudgetTypeController(IBudgetTypeService budgetService)
    {
        _budgetService = budgetService;
    }

    [Route("")]
    [HttpPost]
    public int AddBudgetType([FromBody] BudgetType budgetType)
    {
        return _budgetService.AddBudgetType(budgetType);
    }

    [Route("{budgetTypeId}")]
    [HttpPut]
    public void UpdateBudgetType([FromBody] BudgetType budgetType, int budgetTypeId = -1)
    {
        _budgetService.UpdateBudgetType(budgetType);
    }

    [Route("{budgetTypeId}")]
    [HttpDelete]
    public void DeleteBudgetTypeEntry(int budgetTypeId)
    {
        _budgetService.DeleteBudgetTypeEntry(budgetTypeId);
    }

    [Route("{budgetTypeId}")]
    [HttpGet]
    public BudgetType GetBudgetType(int budgetTypeId)
    {
        return _budgetService.GetBudgetType(budgetTypeId);
    }
}
