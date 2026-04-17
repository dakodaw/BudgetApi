using Budget.DB;
using Budget.DB.Budget;
using BudgetApi.Models;
using BudgetApi.Purchases.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

    public async Task<List<PurchaseLine>> GetPurchaseLines(int groupId, DateTime monthYear)
    {
        var purchases = (from p in (await _purchaseProvider.GetPurchasesByMonthYear(monthYear))
                            .Where(i => i.PaymentType == PurchaseTypeNames.Normal)
                         join t in (await _budgetProvider.GetBudgetTypes(groupId)) on p.PurchaseTypeId equals t.BudgetTypeId
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

    public async Task<IEnumerable<Purchase>> GetReceiptRecordGroupPurchases(Guid receiptRecordGroupId)
    {
        return await _purchaseProvider.GetPurchasesByReceiptRecordGroup(receiptRecordGroupId);
    }

    public async Task<bool> AddUpdatePurchase(Purchase inputPurchase, int purchaseId = -1)
    {
        return await _purchaseProvider.AddUpdatePurchase(inputPurchase, purchaseId);
    }

    public async Task<int> AddPurchase(Purchase inputPurchase)
    {
        return await _purchaseProvider.AddPurchase(inputPurchase);
    }

    public async Task UpdatePurchase(Purchase inputPurchase)
    {
        await _purchaseProvider.UpdatePurchase(inputPurchase);
    }

    public async Task DeletePurchaseEntry(int purchaseId)
    {
        await _purchaseProvider.DeletePurchaseEntry(purchaseId);
    }
    public async Task<bool> DeletePurchaseEntryObsolete(int purchaseId)
    {
        return await _purchaseProvider.DeletePurchaseEntryObsolete(purchaseId);
    }

    public async Task<PurchaseLine> GetExistingPurchase(int purchaseId)
    {
        var p = await _purchaseProvider.GetPurchase(purchaseId);
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
