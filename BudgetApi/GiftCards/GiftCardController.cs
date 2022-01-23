using BudgetApi.GiftCards.Models;
using BudgetApi.GiftCards.Services;
using BudgetApi.Models;
using BudgetApi.Purchases.Models;
using BudgetApi.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BudgetApi.GiftCards
{
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

        [HttpPost]
        [Route("addUpdateGiftCard")]
        public bool AddUpdateGiftCard([FromBody] GiftCard inputGiftCard, [FromQuery] int giftCardId = -1)
        {
            return _giftCardService.AddUpdateGiftCard(inputGiftCard, giftCardId);
        }

        [HttpGet]
        [Route("deleteGiftCardEntry")]
        public bool DeleteGiftCardEntry([FromQuery] int giftCardId)
        {
            return _giftCardService.DeleteGiftCardEntry(giftCardId);
        }

        [HttpGet]
        [Route("getAllBalanceAndHistory")]
        public List<GiftCardHistoryBalance> GetAllBalanceAndHistory()
        {
            return _giftCardService.GetAllBalanceAndHistory();
        }

        [HttpGet]
        [Route("getGiftCard")]
        public GiftCard GetGiftCard([FromQuery] int giftCardId)
        {
            return _giftCardService.GetGiftCard(giftCardId);
        }
    }
}
