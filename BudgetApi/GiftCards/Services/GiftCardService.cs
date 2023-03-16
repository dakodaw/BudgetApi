using System;
using System.Collections.Generic;
using System.Linq;
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
    public List<GiftCardSelectLine> GetGiftCardLines()
    {
        var giftCardLines = new List<GiftCardSelectLine>();
        var giftCards = _giftCardProvider.GetAllGiftCards();
        foreach (var giftCard in giftCards)
        {
            var remaining = GetGiftCardBalance(giftCard.Id);
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

    public List<GiftCardSelectLine> GetGiftCardLinesIncludingZeros()
    {
        var giftCardLines = new List<GiftCardSelectLine>();
        var giftCards = _giftCardProvider.GetAllGiftCards();

        foreach (var giftCard in giftCards)
        {
            var remaining = GetGiftCardBalance(giftCard.Id);
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

    public decimal GetGiftCardBalance(int giftCardId)
    {
        var history = _purchaseProvider.GetGiftCardPurchases(giftCardId).ToList();
        var giftCard = _giftCardProvider.GetGiftCard(giftCardId);
        var initialBalance = giftCard?.InitialAmount ?? 0;
        decimal currentBalance = initialBalance;
        foreach (var purchase in history)
        {
            currentBalance = currentBalance - purchase.Amount;
        }
        return currentBalance;
    }

    public List<PurchaseLine> GetPurchaseLines(DateTime monthYear)
    {
        var giftCardPurchases = _purchaseProvider.GetAllGiftCardPurchases();
        var purchases = (from p in _purchaseProvider.GetMonthGiftCardPurchases(monthYear)
                         join t in _budgetProvider.GetBudgetTypes() on p.PurchaseTypeId equals t.BudgetTypeId
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
                var foundPurchase = _purchaseProvider
                    .GetPurchase(purchase.Id);
                purchase.GiftCardId = foundPurchase != default
                    ? (int)foundPurchase.GiftCardId
                    : 0;
            }
        }
        return purchases;
    }

    public GiftCardHistoryBalance GetBalanceAndHistory(int giftCardId)
    {
        return new GiftCardHistoryBalance
        {
            Balance = GetGiftCardBalance(giftCardId),
            History = _purchaseProvider.GetGiftCardPurchases(giftCardId).ToList()
        };
    }

    public bool AddUpdateGiftCard(GiftCard inputGiftCard, int giftCardId = -1)
    {
        return _giftCardProvider.AddUpdateGiftCard(inputGiftCard, giftCardId);
    }

    public bool DeleteGiftCardEntry(int giftCardId)
    {
        return _giftCardProvider.DeleteGiftCardEntry(giftCardId);
    }

    public List<GiftCardHistoryBalance> GetAllBalanceAndHistory()
    {
        var balance = new List<GiftCardHistoryBalance>();
        foreach (var giftCard in _giftCardProvider.GetAllGiftCards())
        {
            balance.Add(new GiftCardHistoryBalance
            {
                Balance = GetGiftCardBalance(giftCard.Id),
                History = _purchaseProvider.GetGiftCardPurchases(giftCard.Id).ToList(),
                Place = giftCard.Place,
                CardNo = giftCard.CardNumber,
                AccessCode = giftCard.AccessCode
            });
        }
        return balance;
    }

    public GiftCard GetGiftCard(int giftCardId)
    {
        return _giftCardProvider.GetGiftCard(giftCardId);
    }
}
