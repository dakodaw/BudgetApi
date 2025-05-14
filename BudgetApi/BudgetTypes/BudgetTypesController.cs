using Budget.Models.ExceptionTypes;
using BudgetApi.Models;
using BudgetApi.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetApi.BudgetTypes
{
    [Authorize]
    [ApiController]
    [Route("group/{groupId}/[controller]")]
    public class BudgetTypesController : ControllerBase
    {
        private readonly IBudgetTypeService _budgetService;
        private readonly IBudgetAuthorizationService _authorizationService;


        public BudgetTypesController(IBudgetTypeService budgetService, IBudgetAuthorizationService authorizationService)
        {
            _budgetService = budgetService;
            _authorizationService = authorizationService;
        }

        [Route("")]
        [HttpPost]
        public async Task<ActionResult<int>> AddBudgetType(int groupId, [FromBody] BudgetType budgetType)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _budgetService.AddBudgetType(groupId, budgetType);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [Route("{budgetTypeId}")]
        [HttpPut]
        public async Task<ActionResult> UpdateBudgetType([FromBody] BudgetType budgetType, int groupId, int budgetTypeId = -1)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                await _budgetService.UpdateBudgetType(budgetType);
                return Ok();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [Route("{budgetTypeId}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteBudgetTypeEntry(int groupId, int budgetTypeId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                await _budgetService.DeleteBudgetTypeEntry(budgetTypeId);
                return Ok();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [Route("{budgetTypeId}")]
        [HttpGet]
        public async Task<ActionResult<BudgetType>> GetBudgetType(int groupId, int budgetTypeId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _budgetService.GetBudgetType(budgetTypeId);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [Route("")]
        [HttpGet]
        public async Task<ActionResult<List<BudgetType>>> GetBudgetTypes(int groupId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _budgetService.GetBudgetTypes();
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
