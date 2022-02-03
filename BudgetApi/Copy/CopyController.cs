using BudgetApi.Budgeting.Models;
using BudgetApi.Budgeting.Services;
using BudgetApi.Copy.Services;
using BudgetApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
namespace BudgetApi.Copy
{
    [ApiController]
    [Route("[controller]")]
    public class CopyController : ControllerBase
    {
        private readonly IBudgetCopyService _budgetCopyService;
        public CopyController(IBudgetCopyService budgetCopyService)
        {
            _budgetCopyService = budgetCopyService;
        }

        // Initial thoughts: just pass in monthYear
        // Other options: 
        //  1. Pass in list of budgetIds to copy
        //  2. Pass in list of budgets that are based on retreived old budgets
        //      This option would pass list into other controller, maybe budget. this controller wouldn't do anything
        [Route("{monthYear}")]
        [HttpGet]
        public void CopyBudgetFromMonth(DateTime monthYear)
        {
            _budgetCopyService.CopyBudgetFromPreviousMonth(monthYear);
        }
    }
}
