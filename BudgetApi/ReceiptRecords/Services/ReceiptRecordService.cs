using Budget.DB.Incomes;
using Budget.DB.ReceiptRecordGroups;
using Budget.Models;
using BudgetApi.Models;
using BudgetApi.Purchases.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetApi.ReceiptRecords.Services;

public class ReceiptRecordService: IReceiptRecordService
{
    private readonly IReceiptRecordProvider _receiptRecordProvider;
    private readonly IReceiptRecordGroupProvider _receiptRecordGroupProvider;
    private readonly IPurchasesService _purchaseService;

    public ReceiptRecordService(
        IReceiptRecordProvider receiptRecordProvider,
        IReceiptRecordGroupProvider receiptRecordGroupProvider,
        IPurchasesService purchasesService)
    {
        _receiptRecordProvider = receiptRecordProvider;
        _receiptRecordGroupProvider = receiptRecordGroupProvider;
        _purchaseService = purchasesService;
    }

    public async Task<IEnumerable<ReceiptRecord>> List(int groupId, DateTime? monthYear = null)
    {
        var baseRecords = (await _receiptRecordProvider.List(groupId, monthYear)).ToList();

        for (int i = 0; i < baseRecords.Count(); i++)
        {
            var receiptRecordGroups = await GetReceiptRecordWithPurchases(baseRecords[i]);
            baseRecords[i].ReceiptRecordGroups = receiptRecordGroups;
        }

        return baseRecords;
    }

    public async Task<ReceiptRecord> Get(Guid id)
    {
        var receiptRecord = await _receiptRecordProvider.Get(id);
        var receiptRecordGroups = await GetReceiptRecordWithPurchases(receiptRecord);
        receiptRecord.ReceiptRecordGroups = receiptRecordGroups;

        return receiptRecord;
    }

    public async Task Delete(Guid id)
    {
        var receiptRecord = await Get(id);
        
        await DeleteRecordGroups(receiptRecord.ReceiptRecordGroups);

        await _receiptRecordProvider.Delete(id);
    }

    private async Task DeleteRecordGroups(IEnumerable<ReceiptRecordGroup> receiptRecordGroups)
    {
        foreach (var group in receiptRecordGroups)
        {
            await DeletePurchases(group.Purchases);
            await _receiptRecordGroupProvider.Delete(group.Id.Value);
        }
    }

    private async Task DeletePurchases(IEnumerable<Purchase> purchases)
    {
        foreach (var purchase in purchases)
        {
            await _purchaseService.DeletePurchaseEntry(purchase.Id);
        }
    }

    private async Task<IEnumerable<ReceiptRecordGroup>> GetReceiptRecordWithPurchases(ReceiptRecord receiptRecord)
    {
        var receiptRecordGroups = (await _receiptRecordGroupProvider.List(receiptRecord.Id)).ToList();
        
        for (int i = 0;i < receiptRecordGroups.Count(); i++)
        {
            var receiptRecordGroupId = receiptRecordGroups[i].Id;
            var purchases = await _purchaseService.GetReceiptRecordGroupPurchases(receiptRecordGroupId.Value);
            receiptRecordGroups[i].Purchases = purchases;
        }

        return receiptRecordGroups;
    }

    public async Task<ReceiptRecord> Add(int groupId, ReceiptRecord record)
    {
        var recordGroups = record.ReceiptRecordGroups;
        var recordId = await _receiptRecordProvider.Add(groupId, record);

        recordGroups.ToList().ForEach(x => x.ReceiptRecordId = recordId);

        foreach (var group in recordGroups)
        {
            var receiptRecordGroupId = await _receiptRecordGroupProvider.Add(group);
            var purchases = group.Purchases;
            purchases.ToList().ForEach(x => 
            {
                x.BudgetingGroupId = record.BudgetingGroupId;
                x.ReceiptRecordGroupId = receiptRecordGroupId;
                x.PurchaseTypeId = group.BudgetTypeId;
                x.BudgetingGroupId = groupId;
            });

            await AddPurchases(purchases);
        }

        return await Get(recordId);
    }

    public async Task<ReceiptRecord> Update(int groupId, ReceiptRecord record)
    {
        var recordGroups = record.ReceiptRecordGroups;
        var existingReceiptRecord = await Get(record.Id);

        await _receiptRecordProvider.Update(record);
        var incomingRecordGroupIds = recordGroups.Select(x => x.Id);

        recordGroups.ToList().ForEach(x => x.ReceiptRecordId = record.Id);

        // Remove record groups that were removed - This line isn't doing what it's supposed to
        var removedRecords = existingReceiptRecord.ReceiptRecordGroups
            .Where(x => !incomingRecordGroupIds.Contains(x.Id));
        
        await DeleteRecordGroups(removedRecords);

        // Add or update new ones.
        foreach (var group in recordGroups)
        {
            // Check if it's new or not. If it's new, add it, if not, update it.
            if (!group.Id.HasValue)
            {
                var receiptRecordGroupId = _receiptRecordGroupProvider.Add(group);
            }
            else
            {
                // TODO: Put this in a receiptRecordGroupService
                var incomingPurchases = group.Purchases;
                var existingPurchases = existingReceiptRecord.ReceiptRecordGroups
                    .FirstOrDefault(x => x.Id == group.Id)?.Purchases ?? [];

                incomingPurchases.ToList().ForEach(x =>
                {
                    x.BudgetingGroupId = record.BudgetingGroupId;
                    x.ReceiptRecordGroupId = group.Id;
                    x.PurchaseTypeId = group.BudgetTypeId;
                    x.BudgetingGroupId = groupId;
                });

                await UpdatePurchases(existingPurchases, incomingPurchases);
                await _receiptRecordGroupProvider.Update(group);
            }
        }

        return await Get(record.Id);
    }

    private async Task AddPurchases(IEnumerable<Purchase> purchases)
    {
        foreach (var purchase in purchases)
        {
            await _purchaseService.AddPurchase(purchase);
        }
    }

    private async Task UpdatePurchases(IEnumerable<Purchase> existingPurchases, IEnumerable<Purchase> incomingPurchases)
    {
        var incomingPurchaseIds = incomingPurchases.Select(x => x.Id);
        var purchasesToRemove = existingPurchases
            .Where(x => !incomingPurchaseIds.Contains(x.Id));

        await DeletePurchases(purchasesToRemove);
        
        foreach(var purchase in incomingPurchases)
        {
            // need to verify this is what it looks like with new purchases.
            if (purchase.Id == 0)
            {
                await _purchaseService.AddPurchase(purchase);
            }
            else
            {
                await _purchaseService.UpdatePurchase(purchase);
            }
        }
    }

    //private void AddPurchases(IEnumerable<Purchase> groups)
    //{

    //}

    //public void Update(ReceiptRecord record)
    //{
    //}

    //public void Delete(ReceiptRecord record)
    //{
    //}
}
