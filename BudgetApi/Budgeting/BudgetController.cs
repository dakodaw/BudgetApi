using BudgetApi.Budgeting.Services;
using BudgetApi.Models;
using BudgetApi.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BudgetApi.Budgeting
{
    [ApiController]
    [Route("[controller]")]
    public class BudgetController : ControllerBase
    {
        IBudgetService _budgetService;

        public BudgetController(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }
        // GET: Budget
        [Route("getBudgetTypes")]
        [HttpGet]
        public List<BudgetTypes> GetBudgetTypes()
        {
            return _budgetService.GetBudgetTypes();
        }

        [Route("getBudgetLines")]
        [HttpGet]
        public List<BudgetWithPurchaseInfo> GetBudgetLines([FromQuery] DateTime monthYear)
        {
            return _budgetService.GetBudgetLines(monthYear);
        }

        [Route("addUpdateBudget")]
        [HttpPost]
        public bool AddUpdateBudget([FromBody] Budget inputBudget, [FromQuery] int budgetId = -1)
        {
            return _budgetService.AddUpdateBudget(inputBudget, budgetId);
        }

        [Route("deleteBudgetEntry")]
        [HttpGet]
        public bool DeleteBudgetEntry([FromQuery] int budgetId)
        {
            return _budgetService.DeleteBudgetEntry(budgetId);
        }

        [Route("getExistingBudget")]
        [HttpGet]
        public BudgetInfo GetExistingBudget([FromQuery] int budgetId)
        {
            return _budgetService.GetExistingBudget(budgetId);
        }

        [Route("scenarioCheck")]
        [HttpPost]
        public double ScenarioCheck([FromBody] ScenarioInput scenarioInput)
        {
            return _budgetService.ScenarioCheck(scenarioInput);
        }

        [Route("addUpdateBudgetType")]
        [HttpPost]
        public bool AddUpdateBudgetType([FromBody] BudgetType budgetType, [FromQuery] int budgetTypeId = -1)
        {
            return _budgetService.AddUpdateBudgetType(budgetType, budgetTypeId);
        }

        [Route("deleteBudgetTypeEntry")]
        [HttpGet]
        public bool DeleteBudgetTypeEntry([FromQuery] int budgetTypeId)
        {
            return _budgetService.DeleteBudgetTypeEntry(budgetTypeId);
        }

        [Route("getBudgetType")]
        [HttpGet]
        public BudgetTypes GetBudgetType([FromQuery] int budgetTypeId)
        {
            return _budgetService.GetBudgetType(budgetTypeId);
        }
    }
}
