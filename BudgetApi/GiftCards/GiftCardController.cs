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
        public ActionResult<GiftCard> GetGiftCard(int groupId, int giftCardId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return _giftCardService.GetGiftCard(giftCardId);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("")]
        public ActionResult<int> AddGiftCard(int groupId, [FromBody] GiftCard inputGiftCard)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return _giftCardService.AddGiftCard(inputGiftCard);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpPut]
        [Route("{giftCardId}")]
        public ActionResult UpdateGiftCard(int groupId, int giftCardId, [FromBody] GiftCard inputGiftCard)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                _giftCardService.UpdateGiftCard(inputGiftCard);
                return Ok();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpDelete]
        [Route("{giftCardId}")]
        public ActionResult DeleteGiftCard(int groupId, int giftCardId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                _giftCardService.DeleteGiftCardEntry(giftCardId);
                return Ok();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("getGiftCardLines")]
        public ActionResult<List<GiftCardSelectLine>> GetGiftCardLines(int groupId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return _giftCardService.GetGiftCardLines();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("getGiftCardLinesIncludingZeros")]
        public ActionResult<List<GiftCardSelectLine>> GetGiftCardLinesIncludingZeros(int groupId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return _giftCardService.GetGiftCardLinesIncludingZeros();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("getGiftCardBalance")]
        public ActionResult<decimal> GetGiftCardBalance(int groupId, [FromQuery] int giftCardId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return _giftCardService.GetGiftCardBalance(giftCardId);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("getPurchaseLines")]
        public ActionResult<List<PurchaseLine>> GetPurchaseLines(int groupId, [FromQuery] DateTime monthYear)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return _giftCardService.GetPurchaseLines(monthYear);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("getBalanceAndHistory")]
        public ActionResult<GiftCardHistoryBalance> GetBalanceAndHistory(int groupId, [FromQuery] int giftCardId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return _giftCardService.GetBalanceAndHistory(giftCardId);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [Obsolete("Please use Post and Put to add and update gift card instead")]
        [HttpPost]
        [Route("addUpdateGiftCard")]
        public ActionResult<bool> AddUpdateGiftCard(int groupId, [FromBody] GiftCard inputGiftCard, [FromQuery] int giftCardId = -1)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return _giftCardService.AddUpdateGiftCard(inputGiftCard, giftCardId);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [Obsolete("Please use delete at the base gift card instead")]
        [HttpGet]
        [Route("deleteGiftCardEntry")]
        public ActionResult<bool> DeleteGiftCardEntry(int groupId, [FromQuery] int giftCardId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return _giftCardService.DeleteGiftCardObsolete(giftCardId);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("getAllBalanceAndHistory")]
        public ActionResult<List<GiftCardHistoryBalance>> GetAllBalanceAndHistory(int groupId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return _giftCardService.GetAllBalanceAndHistory();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [Obsolete("Please use get at the base route to get gift card instead")]
        [HttpGet]
        [Route("getGiftCard")]
        public ActionResult<GiftCard> GetGiftCardEntry(int groupId, [FromQuery] int giftCardId)
        {
            try
            {
                if (!_authorizationService.IsUserInGroup(ExternalLoginId, groupId))
                {
                    return Unauthorized();
                }

                return _giftCardService.GetGiftCard(giftCardId);
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
