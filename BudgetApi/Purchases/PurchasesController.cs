using Budget.Models.ExceptionTypes;
using BudgetApi.Models;
using BudgetApi.Purchases.Models;
using BudgetApi.Purchases.Services;
using BudgetApi.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetApi.Purchases
{
    [Authorize]
    [ApiController]
    [Route("group/{groupId}/[controller]")]
    public class PurchasesController : Controller
    {
        private readonly IPurchasesService _purchasesService;
        private readonly IBudgetAuthorizationService _authorizationService;

        public PurchasesController(IPurchasesService purchasesService, IBudgetAuthorizationService authorizationService)
        {
            _purchasesService = purchasesService;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        [Route("{purchaseId}")]
        public async Task<ActionResult<PurchaseLine>> GetPurchase(int groupId, int purchaseId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _purchasesService.GetExistingPurchase(purchaseId);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<int>> AddPurchase(int groupId, [FromBody] Purchase incomePurchase)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _purchasesService.AddPurchase(incomePurchase);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpPut]
        [Route("{purchaseId}")]
        public async Task<ActionResult> UpdatePurchase(int groupId, int purchaseId, [FromBody] Purchase incomePurchase)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                await _purchasesService.UpdatePurchase(incomePurchase);
                return Ok();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpDelete]
        [Route("{purchaseId}")]
        public async Task<ActionResult> DeletePurchase(int groupId, int purchaseId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                await _purchasesService.DeletePurchaseEntry(purchaseId);
                return Ok();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("getPurchaseLines")]
        public async Task<ActionResult<List<PurchaseLine>>> GetPurchaseLines(int groupId, [FromQuery] DateTime monthYear)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _purchasesService.GetPurchaseLines(groupId, monthYear);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [Obsolete("Please use the http post and http put on the base route instead")]
        [HttpPost]
        [Route("addUpdatePurchase")]
        public async Task<ActionResult<bool>> AddUpdatePurchase(int groupId, [FromBody] Purchase inputPurchase, [FromQuery] int purchaseId = -1)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _purchasesService.AddUpdatePurchase(inputPurchase, purchaseId);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }


        [Obsolete("Please use the http delete on the base route instead")]
        [HttpGet]
        [Route("deletePurchaseEntry")]
        public async Task<ActionResult<bool>> DeletePurchaseEntry(int groupId, [FromQuery] int purchaseId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _purchasesService.DeletePurchaseEntryObsolete(purchaseId);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [Obsolete("Please use the http get on the base route instead")]
        [HttpGet]
        [Route("getExistingPurchase")]
        public async Task<ActionResult<PurchaseLine>> GetExistingPurchase(int groupId, [FromQuery] int purchaseId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _purchasesService.GetExistingPurchase(purchaseId);
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
