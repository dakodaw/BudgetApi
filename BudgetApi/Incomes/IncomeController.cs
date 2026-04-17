using Budget.Models;
using Budget.Models.ExceptionTypes;
using BudgetApi.Incomes.Models;
using BudgetApi.Incomes.Services;
using BudgetApi.Purchases.Models;
using BudgetApi.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetApi.Incomes
{
    [Authorize]
    [ApiController]
    [Route("group/{groupId}/[controller]")]
    public class IncomeController : ControllerBase
    {
        private readonly IIncomeService _incomeService;
        private readonly IBudgetAuthorizationService _authorizationService;

        public IncomeController(IIncomeService incomeService, IBudgetAuthorizationService authorizationService)
        {
            _incomeService = incomeService;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<int>> AddIncome(int groupId, [FromBody] Income inputIncome)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _incomeService.AddIncome(groupId, inputIncome);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpPut]
        [Route("{incomeId}")]
        public async Task<ActionResult<bool>> UpdateIncome(int groupId, [FromBody] Income inputIncome, int incomeId = -1)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                // TODO: Handle Not found and incomeId of less than 1 passed through
                return await _incomeService.UpdateIncome(inputIncome);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpDelete]
        [Route("{incomeId}")]
        public async Task<ActionResult<bool>> DeleteIncome(int groupId, int incomeId = -1)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                // TODO: Handle Not found and incomeId of less than 1 passed through
                return await _incomeService.DeleteIncomeEntry(incomeId);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("getIncomeTypes")]
        public async Task<ActionResult<List<IncomeSource>>> GetIncomeTypes(int groupId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _incomeService.GetIncomeTypes();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("getIncomeLines")]
        public async Task<ActionResult<List<IncomeLine>>> GetIncomeLines(int groupId, [FromQuery] DateTime monthYear)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _incomeService.GetIncomeLines(groupId, monthYear);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("getIncomeSources")]
        public async Task<ActionResult<List<IncomeSource>>> GetIncomeSources(int groupId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _incomeService.GetIncomeSources();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("getFullIncomeSources")]
        public async Task<ActionResult<List<IncomeSource>>> GetFullIncomeSources(int groupId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _incomeService.GetFullIncomeSources();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("getApplicablePurchases")]
        public async Task<ActionResult<List<ApplicablePurchase>>> GetApplicablePurchases(int groupId, [FromQuery] DateTime monthYear)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _incomeService.GetApplicablePurchases(groupId, monthYear);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [Obsolete("The base route using post, and put will be used moving forward")]
        [HttpPost]
        [Route("addUpdateIncome")]
        public async Task<ActionResult<bool>> AddUpdateIncome(int groupId, [FromBody] Income inputIncome, [FromQuery] int incomeId = -1)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _incomeService.AddUpdateIncome(groupId, inputIncome, incomeId);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [Obsolete("The base route using delete will be used moving forward")]
        [HttpGet]
        [Route("deleteIncomeEntry")]
        public async Task<ActionResult<bool>> DeleteIncomeEntry(int groupId, [FromQuery] int incomeId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _incomeService.DeleteIncomeEntry(incomeId);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("addUpdateJob")]
        public async Task<ActionResult<bool>> AddUpdateJob(int groupId, [FromBody] IncomeSource inputJob, [FromQuery] int incomeSourceId = -1)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _incomeService.AddUpdateJob(inputJob, incomeSourceId);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("deleteJobEntry")]
        public async Task<ActionResult<bool>> DeleteJobEntry(int groupId, [FromQuery] int incomeSourceId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _incomeService.DeleteJobEntry(incomeSourceId);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("getIncomeSource")]
        public async Task<ActionResult<IncomeSource>> GetIncomeSource(int groupId, [FromQuery] int incomeSourceId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _incomeService.GetIncomeSource(incomeSourceId);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("getExistingIncome")]
        public async Task<ActionResult<IncomeLine>> GetExistingIncome(int groupId, [FromQuery] int incomeId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _incomeService.GetExistingIncome(incomeId);
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
