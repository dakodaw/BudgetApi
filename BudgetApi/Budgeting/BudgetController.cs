using BudgetApi.Budgeting.Models;
using BudgetApi.Budgeting.Services;
using BudgetApi.Models;
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
    }
}
