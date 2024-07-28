using BudgetApi.Budgeting.Models;
using BudgetApi.Budgeting.Services;
using BudgetApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BudgetApi.Budgeting
{
    [Authorize]
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
        public int AddBudget([FromBody] BudgetEntry inputBudget)
        {
            return _budgetService.AddBudget(inputBudget);
        }

        [Route("{budgetId}")]
        [HttpPut]
        public void UpdateBudget([FromBody] BudgetEntry inputBudget, int budgetId)
        {
            _budgetService.UpdateBudget(inputBudget);
        }

        [Route("{budgetId}")]
        [HttpDelete]
        public void DeleteBudgetEntry(int budgetId)
        {
            _budgetService.DeleteBudgetEntry(budgetId);
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
