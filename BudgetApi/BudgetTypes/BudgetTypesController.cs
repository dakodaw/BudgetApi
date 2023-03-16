using BudgetApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BudgetApi.BudgetTypes
{
    [ApiController]
    [Route("[controller]")]
    public class BudgetTypesController : ControllerBase
    {
        IBudgetTypeService _budgetService;

        public BudgetTypesController(IBudgetTypeService budgetService)
        {
            _budgetService = budgetService;
        }

        [Route("")]
        [HttpGet]
        public List<BudgetType> GetBudgetTypes()
        {
            return _budgetService.GetBudgetTypes();
        }
    }
}
