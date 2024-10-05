using BudgetApi.Models;
using BudgetApi.Purchases.Models;
using BudgetApi.Shared;
using System;
using System.Collections.Generic;

namespace BudgetApi.Purchases.Services
{
    public interface IPurchasesService
    {
        public List<PurchaseLine> GetPurchaseLines(DateTime monthYear);
        public IEnumerable<Purchase> GetReceiptRecordGroupPurchases(Guid receiptRecordGroupId);
        public bool AddUpdatePurchase(Purchase inputPurchase, int purchaseId = -1);
        public int AddPurchase(Purchase inputPurchase);
        public void UpdatePurchase(Purchase inputPurchase);
        public void DeletePurchaseEntry(int purchaseId);
        public bool DeletePurchaseEntryObsolete(int purchaseId);
        public PurchaseLine GetExistingPurchase(int purchaseId);
    }
}
