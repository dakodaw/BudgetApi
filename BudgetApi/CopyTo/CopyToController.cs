using BudgetApi.Budgeting.Models;
using BudgetApi.Budgeting.Services;
using BudgetApi.CopyTo.Models;
using BudgetApi.CopyTo.Services;
using BudgetApi.Models;
using BudgetApi.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
namespace BudgetApi.CopyTo
{
    [ApiController]
    [Route("[controller]")]
    public class CopyToController : ControllerBase
    {
        private readonly IBudgetCopyToService _budgetCopyToService;
        public CopyToController(IBudgetCopyToService budgetCopyService)
        {
            _budgetCopyToService = budgetCopyService;
        }

        // Initial thoughts: just pass in monthYear
        // Other options: 
        //  1. Pass in list of budgetIds to copy
        //  2. Pass in list of budgets that are based on retreived old budgets
        //      This option would pass list into other controller, maybe budget. this controller wouldn't do anything
        [Route("{monthYear}")]
        [HttpPost]
        public void CopyBudgetFromMonth(DateTime monthYear, [FromBody] CopyFromRequest request)
        {
            _budgetCopyToService.CopyFrom(monthYear, request);
        }
    }
}
