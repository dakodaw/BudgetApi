using BudgetApi.GiftCards.Models;
using BudgetApi.GiftCards.Services;
using BudgetApi.Models;
using BudgetApi.Purchases.Models;
using BudgetApi.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BudgetApi.GiftCards
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class GiftCardController : ControllerBase
    {
        IGiftCardService _giftCardService;

        public GiftCardController(IGiftCardService giftCardService)
        {
            _giftCardService = giftCardService;
        }

        [HttpGet]
        [Route("{giftCardId}")]
        public GiftCard GetGiftCard(int giftCardId)
        {
            return _giftCardService.GetGiftCard(giftCardId);
        }

        [HttpPost]
        [Route("")]
        public int AddGiftCard([FromBody] GiftCard inputGiftCard)
        {
            return _giftCardService.AddGiftCard(inputGiftCard);
        }

        [HttpPut]
        [Route("{giftCardId}")]
        public void UpdateGiftCard([FromBody] GiftCard inputGiftCard, int giftCardId)
        {
            _giftCardService.UpdateGiftCard(inputGiftCard);
        }

        [HttpDelete]
        [Route("{giftCardId}")]
        public void DeleteGiftCard(int giftCardId)
        {
            _giftCardService.DeleteGiftCardEntry(giftCardId);
        }

        [HttpGet]
        [Route("getGiftCardLines")]
        public List<GiftCardSelectLine> GetGiftCardLines()
        {
            return _giftCardService.GetGiftCardLines();
        }

        [HttpGet]
        [Route("getGiftCardLinesIncludingZeros")]
        public List<GiftCardSelectLine> GetGiftCardLinesIncludingZeros()
        {
            return _giftCardService.GetGiftCardLinesIncludingZeros();
        }

        [HttpGet]
        [Route("getGiftCardBalance")]
        public decimal GetGiftCardBalance([FromQuery] int giftCardId)
        {
            return _giftCardService.GetGiftCardBalance(giftCardId);
        }

        [HttpGet]
        [Route("getPurchaseLines")]
        public List<PurchaseLine> GetPurchaseLines([FromQuery] DateTime monthYear)
        {
            return _giftCardService.GetPurchaseLines(monthYear);
        }

        [HttpGet]
        [Route("getBalanceAndHistory")]
        public GiftCardHistoryBalance GetBalanceAndHistory([FromQuery] int giftCardId)
        {
            return _giftCardService.GetBalanceAndHistory(giftCardId);
        }

        [Obsolete("Please use Post and Put to add and update gift card instead")]
        [HttpPost]
        [Route("addUpdateGiftCard")]
        public bool AddUpdateGiftCard([FromBody] GiftCard inputGiftCard, [FromQuery] int giftCardId = -1)
        {
            return _giftCardService.AddUpdateGiftCard(inputGiftCard, giftCardId);
        }

        [Obsolete("Please use delete at the base gift card instead")]
        [HttpGet]
        [Route("deleteGiftCardEntry")]
        public bool DeleteGiftCardEntry([FromQuery] int giftCardId)
        {
            return _giftCardService.DeleteGiftCardObsolete(giftCardId);
        }

        [HttpGet]
        [Route("getAllBalanceAndHistory")]
        public List<GiftCardHistoryBalance> GetAllBalanceAndHistory()
        {
            return _giftCardService.GetAllBalanceAndHistory();
        }

        [Obsolete("Please use get at the base route to get gift card instead")]
        [HttpGet]
        [Route("getGiftCard")]
        public GiftCard GetGiftCardEntry([FromQuery] int giftCardId)
        {
            return _giftCardService.GetGiftCard(giftCardId);
        }
    }
}
