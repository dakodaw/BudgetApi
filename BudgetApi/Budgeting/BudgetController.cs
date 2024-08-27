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

namespace BudgetApi.Budgeting
{
    [Authorize]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetService _budgetService;
        private readonly IBudgetAuthorizationService _authorizationService;

        public BudgetController(IBudgetService budgetService, IBudgetAuthorizationService authorizationService)
        {
            _budgetService = budgetService;
            _authorizationService = authorizationService;
        }

        [Route("group/{groupId}/[controller]/getBudgetLines")]
        [HttpGet]
        public ActionResult<List<BudgetWithPurchaseInfo>> GetBudgetLines(int groupId, [FromQuery] DateTime monthYear)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return _budgetService.GetBudgetLines(monthYear);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [Route("group/{groupId}/[controller]")]
        [HttpPost]
        public ActionResult<int> AddBudget(int groupId, [FromBody] BudgetEntry inputBudget)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return _budgetService.AddBudget(inputBudget);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [Route("group/{groupId}/[controller]/{budgetId}")]
        [HttpPut]
        public ActionResult UpdateBudget(int budgetId, int groupId, [FromBody] BudgetEntry inputBudget)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                _budgetService.UpdateBudget(inputBudget);
                return Ok();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [Route("group/{groupId}/[controller]/{budgetId}")]
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

        [Route("group/{groupId}/[controller]/{budgetId}")]
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

        [Route("group/{groupId}/[controller]/scenarioCheck")]
        [HttpPost]
        public ActionResult<decimal> ScenarioCheck([FromBody] ScenarioInput scenarioInput, int groupId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return _budgetService.ScenarioCheck(scenarioInput);
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
