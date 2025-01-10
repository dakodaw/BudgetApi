using BudgetApi.GiftCards.Models;
using BudgetApi.Models;
using BudgetApi.Purchases.Models;
using System;
using System.Collections.Generic;

namespace BudgetApi.GiftCards.Services;

public interface IGiftCardService
{
    List<GiftCardSelectLine> GetGiftCardLines(int groupId);
    List<GiftCardSelectLine> GetGiftCardLinesIncludingZeros(int groupId);
    decimal GetGiftCardBalance(int giftCardId);
    List<PurchaseLine> GetPurchaseLines(int groupId, DateTime monthYear);
    GiftCardHistoryBalance GetBalanceAndHistory(int giftCardId);
    bool AddUpdateGiftCard(int groupId, GiftCard inputGiftCard, int giftCardId = -1);
    void DeleteGiftCardEntry(int giftCardId);
    bool DeleteGiftCardObsolete(int giftCardId);
    List<GiftCardHistoryBalance> GetAllBalanceAndHistory(int groupId);
    GiftCard GetGiftCard(int giftCardId);
    void UpdateGiftCard(GiftCard inputGiftCard);
    int AddGiftCard(int groupId, GiftCard inputGiftCard);
}
