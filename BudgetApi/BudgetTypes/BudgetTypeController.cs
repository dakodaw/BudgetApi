using BudgetApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BudgetApi.BudgetTypes
{
    [ApiController]
    [Route("[controller]")]
    public class BudgetTypeController : ControllerBase
    {
        IBudgetTypeService _budgetService;

        public BudgetTypeController(IBudgetTypeService budgetService)
        {
            _budgetService = budgetService;
        }

        // GET: Budget
        [Route("getBudgetTypes")]
        [HttpGet]
        public List<BudgetType> GetBudgetTypes()
        {
            return _budgetService.GetBudgetTypes();
        }

        [Route("addUpdateBudgetType")]
        [HttpPost]
        public bool AddUpdateBudgetType([FromBody] BudgetTypeEntity budgetType, [FromQuery] int budgetTypeId = -1)
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
        public BudgetType GetBudgetType([FromQuery] int budgetTypeId)
        {
            return _budgetService.GetBudgetType(budgetTypeId);
        }
    }
}
