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

        [Route("")]
        [HttpPost]
        public bool AddBudget([FromBody] BudgetEntry inputBudget)
        {
            return _budgetService.AddBudget(inputBudget);
        }

        [Route("{budgetId}")]
        [HttpPut]
        public bool UpdateBudget([FromBody] BudgetEntry inputBudget, int budgetId)
        {
            // TODO: Does this need budgetId?
            return _budgetService.UpdateBudget(inputBudget);
        }

        [Route("{budgetId}")]
        [HttpDelete]
        public bool DeleteBudgetEntry(int budgetId)
        {
            return _budgetService.DeleteBudgetEntry(budgetId);
        }

        [Route("{budgetId}")]
        [HttpGet]
        public BudgetInfo GetExistingBudget(int budgetId)
        {
            return _budgetService.GetExistingBudget(budgetId);
        }

        [Route("scenarioCheck")]
        [HttpPost]
        public decimal ScenarioCheck([FromBody] ScenarioInput scenarioInput)
        {
            return _budgetService.ScenarioCheck(scenarioInput);
        }
    }
}
