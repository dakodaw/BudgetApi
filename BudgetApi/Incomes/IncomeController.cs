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
        public ActionResult<int> AddIncome(int groupId, [FromBody] Income inputIncome)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return _incomeService.AddIncome(inputIncome);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpPut]
        [Route("{incomeId}")]
        public ActionResult<bool> UpdateIncome(int groupId, [FromBody] Income inputIncome, int incomeId = -1)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                // TODO: Handle Not found and incomeId of less than 1 passed through
                return _incomeService.UpdateIncome(inputIncome);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpDelete]
        [Route("{incomeId}")]
        public ActionResult<bool> DeleteIncome(int groupId, int incomeId = -1)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                // TODO: Handle Not found and incomeId of less than 1 passed through
                return _incomeService.DeleteIncomeEntry(incomeId);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("getIncomeTypes")]
        public ActionResult<List<IncomeSource>> GetIncomeTypes(int groupId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return _incomeService.GetIncomeTypes();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("getIncomeLines")]
        public ActionResult<List<IncomeLine>> GetIncomeLines(int groupId, [FromQuery] DateTime monthYear)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return _incomeService.GetIncomeLines(monthYear);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("getIncomeSources")]
        public ActionResult<List<IncomeSource>> GetIncomeSources(int groupId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return _incomeService.GetIncomeSources();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("getFullIncomeSources")]
        public ActionResult<List<IncomeSource>> GetFullIncomeSources(int groupId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return _incomeService.GetFullIncomeSources();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("getApplicablePurchases")]
        public ActionResult<List<ApplicablePurchase>> GetApplicablePurchases(int groupId, [FromQuery] DateTime monthYear)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return _incomeService.GetApplicablePurchases(monthYear);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [Obsolete("The base route using post, and put will be used moving forward")]
        [HttpPost]
        [Route("addUpdateIncome")]
        public ActionResult<bool> AddUpdateIncome(int groupId, [FromBody] Income inputIncome, [FromQuery] int incomeId = -1)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return _incomeService.AddUpdateIncome(inputIncome, incomeId);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [Obsolete("The base route using delete will be used moving forward")]
        [HttpGet]
        [Route("deleteIncomeEntry")]
        public ActionResult<bool> DeleteIncomeEntry(int groupId, [FromQuery] int incomeId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return _incomeService.DeleteIncomeEntry(incomeId);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("addUpdateJob")]
        public ActionResult<bool> AddUpdateJob(int groupId, [FromBody] IncomeSource inputJob, [FromQuery] int incomeSourceId = -1)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return _incomeService.AddUpdateJob(inputJob, incomeSourceId);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("deleteJobEntry")]
        public ActionResult<bool> DeleteJobEntry(int groupId, [FromQuery] int incomeSourceId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return _incomeService.DeleteJobEntry(incomeSourceId);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("getIncomeSource")]
        public ActionResult<IncomeSource> GetIncomeSource(int groupId, [FromQuery] int incomeSourceId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return _incomeService.GetIncomeSource(incomeSourceId);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("getExistingIncome")]
        public ActionResult<IncomeLine> GetExistingIncome(int groupId, [FromQuery] int incomeId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return _incomeService.GetExistingIncome(incomeId);
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
