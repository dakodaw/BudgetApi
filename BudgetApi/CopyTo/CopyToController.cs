using Budget.Models.ExceptionTypes;
using BudgetApi.CopyTo.Models;
using BudgetApi.CopyTo.Services;
using BudgetApi.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
namespace BudgetApi.CopyTo
{
    [Authorize]
    [ApiController]
    [Route("group/{groupId}/[controller]")]

    public class CopyToController : ControllerBase
    {
        private readonly IBudgetCopyToService _budgetCopyToService;
        private readonly IBudgetAuthorizationService _authorizationService;

        public CopyToController(IBudgetCopyToService budgetCopyService, IBudgetAuthorizationService authorizationService)
        {
            _budgetCopyToService = budgetCopyService;
            _authorizationService = authorizationService;
        }

        // Initial thoughts: just pass in monthYear
        // Other options: 
        //  1. Pass in list of budgetIds to copy
        //  2. Pass in list of budgets that are based on retreived old budgets
        //      This option would pass list into other controller, maybe budget. this controller wouldn't do anything
        [Route("{monthYear}")]
        [HttpPost]
        public ActionResult CopyBudgetFromMonth(int groupId, DateTime monthYear, [FromBody] CopyFromRequest request)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                _budgetCopyToService.CopyFrom(monthYear, request);
                return Ok();
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
