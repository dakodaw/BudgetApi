using Budget.Models.ExceptionTypes;
using BudgetApi.Budgeting.Models;
using BudgetApi.Budgeting.Services;
using BudgetApi.Models;
using BudgetApi.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetApi.Budgeting
{
    [Authorize]
    [ApiController]
    [Route("group/{groupId}/[controller]")]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetService _budgetService;
        private readonly IBudgetAuthorizationService _authorizationService;

        public BudgetController(IBudgetService budgetService, IBudgetAuthorizationService authorizationService)
        {
            _budgetService = budgetService;
            _authorizationService = authorizationService;
        }

        [Route("getBudgetLines")]
        [HttpGet]
        public async Task<ActionResult<List<BudgetWithPurchaseInfo>>> GetBudgetLines(int groupId, [FromQuery] DateTime monthYear)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _budgetService.GetBudgetLines(groupId, monthYear);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [Route("")]
        [HttpPost]
        public async Task<ActionResult<int>> AddBudget(int groupId, [FromBody] BudgetEntry inputBudget)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _budgetService.AddBudget(groupId, inputBudget);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [Route("{budgetId}")]
        [HttpPut]
        public async Task<ActionResult> UpdateBudget(int budgetId, int groupId, [FromBody] BudgetEntry inputBudget)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                await _budgetService.UpdateBudget(inputBudget);
                return Ok();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [Route("{budgetId}")]
        [HttpDelete]
        public ActionResult DeleteBudgetEntry(int budgetId, int groupId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                _budgetService.DeleteBudgetEntry(budgetId);
                return Ok();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [Route("{budgetId}")]
        [HttpGet]
        public ActionResult<BudgetInfo> GetExistingBudget(int budgetId, int groupId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return Ok(_budgetService.GetExistingBudget(budgetId));
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [Route("scenarioCheck")]
        [HttpPost]
        public async Task<ActionResult<decimal>> ScenarioCheck([FromBody] ScenarioInput scenarioInput, int groupId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _budgetService.ScenarioCheck(groupId, scenarioInput);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        private string ExternalLoginId => HttpContext
            .User.Claims.FirstOrDefault(x => x.Type == "user_id")?.Value;
    }
}
