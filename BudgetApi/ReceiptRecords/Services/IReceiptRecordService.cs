using Budget.Models;
using System;
using System.Collections.Generic;

namespace BudgetApi.ReceiptRecords.Services;

public interface IReceiptRecordService
{
    IEnumerable<ReceiptRecord> List(int groupId, DateTime? monthYear = null);
    ReceiptRecord Get(Guid id);
    ReceiptRecord Add(int groupId, ReceiptRecord record);
    //void Update(ReceiptRecord record);
    //void Delete(ReceiptRecord record);
}
