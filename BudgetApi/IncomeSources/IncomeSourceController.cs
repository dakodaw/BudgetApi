using Budget.Models;
using Budget.Models.ExceptionTypes;
using BudgetApi.Incomes.Services;
using BudgetApi.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BudgetApi.Incomes
{
    [Authorize]
    [ApiController]
    [Route("group/{groupId}/[controller]")]
    public class IncomeSourceController : ControllerBase
    {
        private readonly IIncomeSourceService _incomeService;
        private readonly IBudgetAuthorizationService _authorizationService;

        public IncomeSourceController(IIncomeSourceService incomeService, IBudgetAuthorizationService authorizationService)
        {
            _incomeService = incomeService;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        [Route("list")]
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

        //[HttpGet]
        //[Route("fullList")]
        //public List<IncomeSources> GetFullIncomeSources()
        //{
        //    return _incomeService.GetFullIncomeSources();
        //}

        [HttpDelete]
        [Route("{sourceId}")]
        public ActionResult DeleteJobEntry(int groupId, int sourceId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                _incomeService.DeleteIncomeSource(sourceId);
                return Ok();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("{sourceId}")]
        public ActionResult<IncomeSource> GetIncomeSource(int groupId, int sourceId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return _incomeService.GetIncomeSource(sourceId);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpPut]
        [Route("{sourceId}")] 
        public ActionResult UpdateIncomeSource(int groupId, [FromBody] IncomeSource incomeSourceToUpdate, int sourceId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                _incomeService.UpdateIncomeSource(incomeSourceToUpdate);
                return Ok();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("")]
        public ActionResult<int> AddIncomeSource(int groupId, [FromBody] IncomeSource incomeSourceToUpdate)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return _incomeService.AddIncomeSource(incomeSourceToUpdate);
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
