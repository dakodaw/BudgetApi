using Budget.Models;
using System;

namespace BudgetApi.ReceiptRecords.Services;

public interface IReceiptRecordService
{
    ReceiptRecord Get(Guid id);
    ReceiptRecord Add(ReceiptRecord record);
    //void Update(ReceiptRecord record);
    //void Delete(ReceiptRecord record);
}
