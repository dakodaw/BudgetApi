using BudgetApi.Models;
using BudgetApi.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BudgetApi.BudgetTypes
{
    [ApiController]
    [Route("[controller]")]
    public class BudgetTypeController : ControllerBase
    {
        IBudgetTypeService _budgetService;
        ISchedulerService _schedulerService;

        public BudgetTypeController(IBudgetTypeService budgetService, ISchedulerService schedulerService)
        {
            _budgetService = budgetService;
            _schedulerService = schedulerService;
        }

        [Route("")]
        [HttpPost]
        public bool AddBudgetType([FromBody] BudgetTypeEntity budgetType)
        {
            return _budgetService.AddUpdateBudgetType(budgetType);
        }

        [Route("{budgetTypeId}")]
        [HttpPut]
        public bool UpdateBudgetType([FromBody] BudgetTypeEntity budgetType, int budgetTypeId = -1)
        {
            return _budgetService.AddUpdateBudgetType(budgetType, budgetTypeId);
        }

        [Route("{budgetTypeId}")]
        [HttpDelete]
        public bool DeleteBudgetTypeEntry(int budgetTypeId)
        {
            return _budgetService.DeleteBudgetTypeEntry(budgetTypeId);
        }

        [Route("{budgetTypeId}")]
        [HttpGet]
        public BudgetType GetBudgetType(int budgetTypeId)
        {
            return _budgetService.GetBudgetType(budgetTypeId);
        }
    }
}
