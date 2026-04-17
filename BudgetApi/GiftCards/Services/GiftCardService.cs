using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Budget.DB;
using Budget.DB.Budget;
using Budget.DB.GiftCards;
using BudgetApi.GiftCards.Models;
using BudgetApi.Models;
using BudgetApi.Purchases.Models;
using BudgetApi.Shared.Custom;

namespace BudgetApi.GiftCards.Services;

public class GiftCardService: IGiftCardService
{
    IPurchaseProvider _purchaseProvider;
    IGiftCardProvider _giftCardProvider;
    IBudgetProvider _budgetProvider;

    public GiftCardService(
        IPurchaseProvider purchaseProvider,
        IGiftCardProvider giftCardProvider,
        IBudgetProvider budgetProvider)
    {
        _purchaseProvider = purchaseProvider;
        _giftCardProvider = giftCardProvider;
        _budgetProvider = budgetProvider;
    }

    // GET api/<controller>
    public async Task<List<GiftCardSelectLine>> GetGiftCardLines(int groupId)
    {
        var giftCardLines = new List<GiftCardSelectLine>();
        var giftCards = await _giftCardProvider.GetAllGiftCards(groupId);
        foreach (var giftCard in giftCards)
        {
            var remaining = await GetGiftCardBalance(giftCard.Id);
            if (remaining > 0)
            {
                var card = new GiftCardSelectLine
                {
                    Id = giftCard.Id,
                    Place = giftCard.Place,
                    Last4ofCardNumber = giftCard.CardNumber.GetLast(4),
                    RemainingAmount = remaining
                };

                giftCardLines.Add(card);
            }
        }
        return giftCardLines.OrderBy(i => i.Place).ToList();
    }

    public async Task<List<GiftCardSelectLine>> GetGiftCardLinesIncludingZeros(int groupId)
    {
        var giftCardLines = new List<GiftCardSelectLine>();
        var giftCards = await _giftCardProvider.GetAllGiftCards(groupId);

        foreach (var giftCard in giftCards)
        {
            var remaining = await GetGiftCardBalance(giftCard.Id);
            if (remaining > 0)
            {
                var card = new GiftCardSelectLine
                {
                    Id = giftCard.Id,
                    Place = giftCard.Place,
                    Last4ofCardNumber = giftCard.CardNumber.GetLast(4),
                    RemainingAmount = remaining
                };

                giftCardLines.Add(card);
            }
            else
            {
                var card = new GiftCardSelectLine
                {
                    Id = giftCard.Id,
                    Place = giftCard.Place,
                    Last4ofCardNumber = giftCard.CardNumber.GetLast(4),
                };

                giftCardLines.Add(card);
            }
        }

        return giftCardLines.OrderBy(i => i.Place).ToList();
    }

    public async Task<decimal> GetGiftCardBalance(int giftCardId)
    {
        var history = await _purchaseProvider.GetGiftCardPurchases(giftCardId);
        var giftCard = await _giftCardProvider.GetGiftCard(giftCardId);
        var initialBalance = giftCard?.InitialAmount ?? 0;
        decimal currentBalance = initialBalance;

        foreach (var purchase in history)
        {
            currentBalance = currentBalance - purchase.Amount;
        }

        return currentBalance;
    }

    public async Task<List<PurchaseLine>> GetPurchaseLines(int groupId, DateTime monthYear)
    {
        //var giftCardPurchases = _purchaseProvider.GetAllGiftCardPurchases();
        var monthGiftCardPurchases = await _purchaseProvider.GetMonthGiftCardPurchases(monthYear);
        var budgetTypes = await _budgetProvider.GetBudgetTypes(groupId);

        var purchases = (from p in monthGiftCardPurchases
                         join t in budgetTypes on p.PurchaseTypeId equals t.BudgetTypeId
                         select new PurchaseLine
                         {
                             PurchaseType = new BudgetType
                             {
                                 BudgetTypeId = p.PurchaseTypeId,
                                 BudgetTypeName = t.BudgetTypeName
                             },
                             Description = p.Description,
                             Date = p.Date,
                             Amount = p.Amount,
                             Id = p.Id,
                             PaymentType = p.PaymentType,
                             //GiftCardId = p.GiftCardId,
                             IsReimbursement = p.FutureReimbursement
                         }).ToList();

        foreach (var purchase in purchases)
        {
            if (purchase.PaymentType == PurchaseTypeNames.GiftCard)
            {
                var foundPurchase = await _purchaseProvider
                    .GetPurchase(purchase.Id);

                purchase.GiftCardId = foundPurchase != default
                    ? foundPurchase.GiftCardId
                    : 0;
            }
        }

        return purchases;
    }

    public async Task<GiftCardHistoryBalance> GetBalanceAndHistory(int giftCardId)
    {
        return new GiftCardHistoryBalance
        {
            Balance = await GetGiftCardBalance(giftCardId),
            History = (await _purchaseProvider.GetGiftCardPurchases(giftCardId)).ToList()
        };
    }

    public async Task<bool> AddUpdateGiftCard(int groupId, GiftCard inputGiftCard, int giftCardId = -1)
    {
        return await _giftCardProvider.AddUpdateGiftCard(groupId, inputGiftCard, giftCardId);
    }

    public async Task<int> AddGiftCard(int groupId, GiftCard inputGiftCard)
    {
        return await _giftCardProvider.AddGiftCard(groupId, inputGiftCard);
    }

    public async Task UpdateGiftCard(GiftCard inputGiftCard)
    {
        await _giftCardProvider.UpdateGiftCard(inputGiftCard);
    }

    public async Task DeleteGiftCardEntry(int giftCardId)
    {
        await _giftCardProvider.DeleteGiftCardEntry(giftCardId);
    }

    public async Task<bool> DeleteGiftCardObsolete(int giftCardId)
    {
        try
        {
            await DeleteGiftCardEntry(giftCardId);
            return true;
        }
        catch { return false; }
    }

    public async Task<List<GiftCardHistoryBalance>> GetAllBalanceAndHistory(int groupId)
    {
        var balance = new List<GiftCardHistoryBalance>();
        foreach (var giftCard in await _giftCardProvider.GetAllGiftCards(groupId))
        {
            balance.Add(new GiftCardHistoryBalance
            {
                Balance = await GetGiftCardBalance(giftCard.Id),
                History = (await _purchaseProvider.GetGiftCardPurchases(giftCard.Id)).ToList(),
                Place = giftCard.Place,
                CardNo = giftCard.CardNumber,
                AccessCode = giftCard.AccessCode
            });
        }
        return balance;
    }

    public async Task<GiftCard> GetGiftCard(int giftCardId)
    {
        return await _giftCardProvider.GetGiftCard(giftCardId);
    }
}
