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

    public IEnumerable<ReceiptRecord> List(int groupId)
    {
        var baseRecords = _receiptRecordProvider.List(groupId);
        foreach(var record in baseRecords)
        {
            HydrateReceiptRecord(record);
        }

        return baseRecords;
    }

    public ReceiptRecord Get(Guid id)
    {
        var receiptRecord = _receiptRecordProvider.Get(id);
        HydrateReceiptRecord(receiptRecord);

        return receiptRecord;
    }

    private void HydrateReceiptRecord(ReceiptRecord receiptRecord)
    {
        receiptRecord.ReceiptRecordGroups = _receiptRecordGroupProvider.List(receiptRecord.Id);
        foreach (var record in receiptRecord.ReceiptRecordGroups)
        {
            record.Purchases = _purchaseService.GetReceiptRecordGroupPurchases(record.ReceiptRecordId);
        }
    }

    public ReceiptRecord Add(ReceiptRecord record)
    {
        var recordGroups = record.ReceiptRecordGroups;
        var recordId = _receiptRecordProvider.Add(record);

        recordGroups.ToList().ForEach(x => x.ReceiptRecordId = recordId);

        foreach (var group in recordGroups)
        {
            var groupId = _receiptRecordGroupProvider.Add(group);
            var purchases = group.Purchases;
            purchases.ToList().ForEach(x => 
            {
                x.BudgetingGroupId = record.BudgetingGroupId;
                x.ReceiptRecordGroupId = groupId;
                x.PurchaseTypeId = group.BudgetTypeId;
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
