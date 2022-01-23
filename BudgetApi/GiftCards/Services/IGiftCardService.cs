using BudgetApi.GiftCards.Models;
using BudgetApi.Models;
using BudgetApi.Purchases.Models;
using BudgetApi.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetApi.GiftCards.Services
{
    public interface IGiftCardService
    {
        List<GiftCardSelectLine> GetGiftCardLines();
        List<GiftCardSelectLine> GetGiftCardLinesIncludingZeros();
        decimal GetGiftCardBalance(int giftCardId);
        List<PurchaseLine> GetPurchaseLines(DateTime monthYear);
        GiftCardHistoryBalance GetBalanceAndHistory(int giftCardId);
        bool AddUpdateGiftCard(GiftCard inputGiftCard, int giftCardId = -1);
        bool DeleteGiftCardEntry(int giftCardId);
        List<GiftCardHistoryBalance> GetAllBalanceAndHistory();
        GiftCard GetGiftCard(int giftCardId);
    }
}
