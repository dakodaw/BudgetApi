using BudgetApi.GiftCards.Models;
using BudgetApi.Models;
using BudgetApi.Purchases.Models;
using System;
using System.Collections.Generic;

namespace BudgetApi.GiftCards.Services;

public interface IGiftCardService
{
    List<GiftCardSelectLine> GetGiftCardLines();
    List<GiftCardSelectLine> GetGiftCardLinesIncludingZeros();
    decimal GetGiftCardBalance(int giftCardId);
    List<PurchaseLine> GetPurchaseLines(DateTime monthYear);
    GiftCardHistoryBalance GetBalanceAndHistory(int giftCardId);
    bool AddUpdateGiftCard(GiftCard inputGiftCard, int giftCardId = -1);
    void DeleteGiftCardEntry(int giftCardId);
    bool DeleteGiftCardObsolete(int giftCardId);
    List<GiftCardHistoryBalance> GetAllBalanceAndHistory();
    GiftCard GetGiftCard(int giftCardId);
    void UpdateGiftCard(GiftCard inputGiftCard);
    int AddGiftCard(GiftCard inputGiftCard);
}
