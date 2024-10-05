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

    public ReceiptRecord Get(Guid id)
    {
        var receiptRecord = _receiptRecordProvider.Get(id);
        receiptRecord.ReceiptRecords = _receiptRecordGroupProvider.List(id);
        foreach(var record in receiptRecord.ReceiptRecords)
        {
            record.Purchases = _purchaseService.GetReceiptRecordGroupPurchases(record.ReceiptRecordId);
        }

        return receiptRecord;
    }

    public ReceiptRecord Add(ReceiptRecord record)
    {
        var recordGroups = record.ReceiptRecords;
        var recordId = _receiptRecordProvider.Add(record);

        foreach (var group in recordGroups)
        {
            _receiptRecordGroupProvider.Add(group);
            AddPurchases(group.Purchases);
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
