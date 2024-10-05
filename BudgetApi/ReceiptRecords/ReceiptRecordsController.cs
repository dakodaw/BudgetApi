using Budget.Models;
using Budget.Models.ExceptionTypes;
using BudgetApi.Purchases.Models;
using BudgetApi.ReceiptRecords.Services;
using BudgetApi.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

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
        [Route("{receiptRecordId}")]
        public ActionResult<ReceiptRecord> GetReceiptRecord(int groupId, Guid receiptRecordId)
        {
            try
            {
                // TODO: Need to still add groupIds on purchases, etc.
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return _receiptRecordService.Get(receiptRecordId);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("{receiptRecordId}")]
        public ActionResult<ReceiptRecord> AddReceiptRecord(int groupId, [FromBody] ReceiptRecord inputRecord)
        {
            try
            {
                // TODO: Need to still add groupIds on purchases, etc.
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }
                return _receiptRecordService.Add(inputRecord);
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
