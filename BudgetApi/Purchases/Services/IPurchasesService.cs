using BudgetApi.Models;
using BudgetApi.Purchases.Models;
using BudgetApi.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BudgetApi.Purchases.Services;

public interface IPurchasesService
{
    Task<List<PurchaseLine>> GetPurchaseLines(int groupId, DateTime monthYear);
    Task<IEnumerable<Purchase>> GetReceiptRecordGroupPurchases(Guid receiptRecordGroupId);
    Task<bool> AddUpdatePurchase(Purchase inputPurchase, int purchaseId = -1);
    Task<int> AddPurchase(Purchase inputPurchase);
    Task UpdatePurchase(Purchase inputPurchase);
    Task DeletePurchaseEntry(int purchaseId);
    Task<bool> DeletePurchaseEntryObsolete(int purchaseId);
    Task<PurchaseLine> GetExistingPurchase(int purchaseId);
}
