using Budget.DB;
using Budget.DB.Budget;
using BudgetApi.Models;
using BudgetApi.Purchases.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetApi.Purchases.Services;

public class PurchasesService: IPurchasesService
{
    IPurchaseProvider _purchaseProvider;
    IBudgetProvider _budgetProvider;

    public PurchasesService(
        IPurchaseProvider purchaseProvider,
        IBudgetProvider budgetProvider)
    {
        _purchaseProvider = purchaseProvider;
        _budgetProvider = budgetProvider;
    }

    public List<PurchaseLine> GetPurchaseLines(DateTime monthYear)
    {
        var purchases = (from p in _purchaseProvider.GetPurchasesByMonthYear(monthYear)
                            .Where(i => i.PaymentType == PurchaseTypeNames.Normal)
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
                             GiftCardId = p.GiftCardId,
                             IsReimbursement = p.FutureReimbursement
                         }).ToList();
        // Not sure if this still needs to happen.
        //foreach (var purchase in purchases)
        //{
        //    if (purchase.PaymentType == PurchaseTypeNames.GiftCard)
        //    {
        //        purchase.GiftCardId = (int)_db.Purchases.Where(i => i.Id == purchase.Id).FirstOrDefault().GiftCardId;
        //    }
        //}
        return purchases.OrderBy(i => i.PurchaseType.BudgetTypeName).ToList();
    }

    public bool AddUpdatePurchase(Purchase inputPurchase, int purchaseId = -1)
    {
        return _purchaseProvider.AddUpdatePurchase(inputPurchase, purchaseId);
    }

    public int AddPurchase(Purchase inputPurchase)
    {
        return _purchaseProvider.AddPurchase(inputPurchase);
    }

    public void UpdatePurchase(Purchase inputPurchase)
    {
        _purchaseProvider.UpdatePurchase(inputPurchase);
    }

    public void DeletePurchaseEntry(int purchaseId)
    {
        _purchaseProvider.DeletePurchaseEntry(purchaseId);
    }
    public bool DeletePurchaseEntryObsolete(int purchaseId)
    {
        return _purchaseProvider.DeletePurchaseEntryObsolete(purchaseId);
    }

    public PurchaseLine GetExistingPurchase(int purchaseId)
    {
        var p = _purchaseProvider.GetPurchase(purchaseId);
        return new PurchaseLine
        {
            Amount = p.Amount,
            Date = p.Date,
            GiftCardId = p.GiftCardId,
            PurchaseType = new BudgetType
            {
                BudgetTypeId = p.PurchaseTypeId,
            },
            Description = p.Description,
            PaymentType = p.PaymentType,
            IsReimbursement = p.FutureReimbursement
        };
    }
}
