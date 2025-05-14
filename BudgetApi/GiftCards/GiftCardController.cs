using Budget.Models.ExceptionTypes;
using BudgetApi.GiftCards.Models;
using BudgetApi.GiftCards.Services;
using BudgetApi.Models;
using BudgetApi.Purchases.Models;
using BudgetApi.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetApi.GiftCards
{
    [Authorize]
    [ApiController]
    [Route("group/{groupId}/[controller]")]
    public class GiftCardController : ControllerBase
    {
        private readonly IGiftCardService _giftCardService;
        private readonly IBudgetAuthorizationService _authorizationService;

        public GiftCardController(IGiftCardService giftCardService, IBudgetAuthorizationService authorizationService)
        {
            _giftCardService = giftCardService;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        [Route("{giftCardId}")]
        public async Task<ActionResult<GiftCard>> GetGiftCard(int groupId, int giftCardId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _giftCardService.GetGiftCard(giftCardId);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<int>> AddGiftCard(int groupId, [FromBody] GiftCard inputGiftCard)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _giftCardService.AddGiftCard(groupId, inputGiftCard);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpPut]
        [Route("{giftCardId}")]
        public async Task<ActionResult> UpdateGiftCard(int groupId, int giftCardId, [FromBody] GiftCard inputGiftCard)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                await _giftCardService.UpdateGiftCard(inputGiftCard);
                return Ok();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpDelete]
        [Route("{giftCardId}")]
        public async Task<ActionResult> DeleteGiftCard(int groupId, int giftCardId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                await _giftCardService.DeleteGiftCardEntry(giftCardId);
                return Ok();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("getGiftCardLines")]
        public async Task<ActionResult<List<GiftCardSelectLine>>> GetGiftCardLines(int groupId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _giftCardService.GetGiftCardLines(groupId);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("getGiftCardLinesIncludingZeros")]
        public async Task<ActionResult<List<GiftCardSelectLine>>> GetGiftCardLinesIncludingZeros(int groupId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _giftCardService.GetGiftCardLinesIncludingZeros(groupId);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("getGiftCardBalance")]
        public async Task<ActionResult<decimal>> GetGiftCardBalance(int groupId, [FromQuery] int giftCardId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _giftCardService.GetGiftCardBalance(giftCardId);
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

                return await _giftCardService.GetPurchaseLines(groupId, monthYear);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("getBalanceAndHistory")]
        public async Task<ActionResult<GiftCardHistoryBalance>> GetBalanceAndHistory(int groupId, [FromQuery] int giftCardId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _giftCardService.GetBalanceAndHistory(giftCardId);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [Obsolete("Please use Post and Put to add and update gift card instead")]
        [HttpPost]
        [Route("addUpdateGiftCard")]
        public async Task<ActionResult<bool>> AddUpdateGiftCard(int groupId, [FromBody] GiftCard inputGiftCard, [FromQuery] int giftCardId = -1)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _giftCardService.AddUpdateGiftCard(groupId, inputGiftCard, giftCardId);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [Obsolete("Please use delete at the base gift card instead")]
        [HttpGet]
        [Route("deleteGiftCardEntry")]
        public async Task<ActionResult<bool>> DeleteGiftCardEntry(int groupId, [FromQuery] int giftCardId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _giftCardService.DeleteGiftCardObsolete(giftCardId);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("getAllBalanceAndHistory")]
        public async Task<ActionResult<List<GiftCardHistoryBalance>>> GetAllBalanceAndHistory(int groupId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _giftCardService.GetAllBalanceAndHistory(groupId);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [Obsolete("Please use get at the base route to get gift card instead")]
        [HttpGet]
        [Route("getGiftCard")]
        public async Task<ActionResult<GiftCard>> GetGiftCardEntry(int groupId, [FromQuery] int giftCardId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return await _giftCardService.GetGiftCard(giftCardId);
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
