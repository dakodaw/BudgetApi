using BudgetApi.GiftCards.Models;
using BudgetApi.Models;
using BudgetApi.Purchases.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BudgetApi.GiftCards.Services;

public interface IGiftCardService
{
    Task<List<GiftCardSelectLine>> GetGiftCardLines(int groupId);
    Task<List<GiftCardSelectLine>> GetGiftCardLinesIncludingZeros(int groupId);
    Task<decimal> GetGiftCardBalance(int giftCardId);
    Task<List<PurchaseLine>> GetPurchaseLines(int groupId, DateTime monthYear);
    Task<GiftCardHistoryBalance> GetBalanceAndHistory(int giftCardId);
    Task<bool> AddUpdateGiftCard(int groupId, GiftCard inputGiftCard, int giftCardId = -1);
    Task DeleteGiftCardEntry(int giftCardId);
    Task<bool> DeleteGiftCardObsolete(int giftCardId);
    Task<List<GiftCardHistoryBalance>> GetAllBalanceAndHistory(int groupId);
    Task<GiftCard> GetGiftCard(int giftCardId);
    Task UpdateGiftCard(GiftCard inputGiftCard);
    Task<int> AddGiftCard(int groupId, GiftCard inputGiftCard);
}
