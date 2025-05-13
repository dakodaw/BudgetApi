using Budget.DB.Incomes;
using Budget.DB.ReceiptRecordGroups;
using Budget.Models;
using BudgetApi.Models;
using BudgetApi.Purchases.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

    public IEnumerable<ReceiptRecord> List(int groupId, DateTime? monthYear = null)
    {
        var baseRecords = _receiptRecordProvider.List(groupId, monthYear).ToList();

        for (int i = 0; i < baseRecords.Count(); i++)
        {
            var receiptRecordGroups = GetReceiptRecordWithPurchases(baseRecords[i]);
            baseRecords[i].ReceiptRecordGroups = receiptRecordGroups;
        }

        return baseRecords;
    }

    public ReceiptRecord Get(Guid id)
    {
        var receiptRecord = _receiptRecordProvider.Get(id);
        var receiptRecordGroups = GetReceiptRecordWithPurchases(receiptRecord);
        receiptRecord.ReceiptRecordGroups = receiptRecordGroups;

        return receiptRecord;
    }

    private IEnumerable<ReceiptRecordGroup> GetReceiptRecordWithPurchases(ReceiptRecord receiptRecord)
    {
        var receiptRecordGroups = _receiptRecordGroupProvider.List(receiptRecord.Id).ToList();
        
        for (int i = 0;i < receiptRecordGroups.Count(); i++)
        {
            var purchases = _purchaseService.GetReceiptRecordGroupPurchases(receiptRecordGroups[i].Id);
            receiptRecordGroups[i].Purchases = purchases;
        }

        return receiptRecordGroups;
    }

    public ReceiptRecord Add(int groupId, ReceiptRecord record)
    {
        var recordGroups = record.ReceiptRecordGroups;
        var recordId = _receiptRecordProvider.Add(groupId, record);

        recordGroups.ToList().ForEach(x => x.ReceiptRecordId = recordId);

        foreach (var group in recordGroups)
        {
            var receiptRecordGroupId = _receiptRecordGroupProvider.Add(group);
            var purchases = group.Purchases;
            purchases.ToList().ForEach(x => 
            {
                x.BudgetingGroupId = record.BudgetingGroupId;
                x.ReceiptRecordGroupId = receiptRecordGroupId;
                x.PurchaseTypeId = group.BudgetTypeId;
                x.BudgetingGroupId = groupId;
            });

            AddPurchases(purchases);
        }

        return Get(recordId);
    }

    private void AddPurchases(IEnumerable<Purchase> purchases)
    {
        foreach (var purchase in purchases)
        {
            _purchaseService.AddPurchase(purchase);
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
