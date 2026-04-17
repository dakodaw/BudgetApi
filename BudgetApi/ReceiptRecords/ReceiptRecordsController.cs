using Budget.Models;
using Budget.Models.ExceptionTypes;
using BudgetApi.Purchases.Models;
using BudgetApi.ReceiptRecords.Services;
using BudgetApi.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BudgetApi.ReceiptRecords
{
    [Authorize]
    [ApiController]
    [Route("group/{groupId}/[controller]")]
    public class ReceiptRecordsController : Controller
    {
        private readonly IReceiptRecordService _receiptRecordService;
        private readonly IBudgetAuthorizationService _authorizationService;
        public ReceiptRecordsController(
            IReceiptRecordService receiptRecordService,
            IBudgetAuthorizationService authorizationService) 
        {
            _receiptRecordService = receiptRecordService;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<ReceiptRecord>>> List(int groupId, [FromQuery] DateTime? monthYear = null)
        {
            try
            {
                // TODO: Need to still add groupIds on purchases, etc.
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return Ok(await _receiptRecordService.List(groupId, monthYear));
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("{receiptRecordId}")]
        public async Task<ActionResult<ReceiptRecord>> GetReceiptRecord(int groupId, Guid receiptRecordId)
        {
            try
            {
                // TODO: Need to still add groupIds on purchases, etc.
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _receiptRecordService.Get(receiptRecordId);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<ReceiptRecord>> AddReceiptRecord(int groupId, [FromBody] ReceiptRecord inputRecord)
        {
            try
            {
                // TODO: Need to still add groupIds on purchases, etc.
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _receiptRecordService.Add(groupId, inputRecord);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpPut]
        [Route("{receiptRecordId}")]
        public async Task<ActionResult<ReceiptRecord>> UpdateReceiptRecord(int groupId, Guid receiptRecordId, [FromBody] ReceiptRecord inputRecord)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _receiptRecordService.Update(groupId, inputRecord);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpDelete]
        [Route("{receiptRecordId}")]
        public async Task<ActionResult> DeleteReceiptRecord(int groupId, Guid receiptRecordId)
        {
            try
            {
                // TODO: Need to still add groupIds on purchases, etc.
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                await _receiptRecordService.Delete(receiptRecordId);
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
