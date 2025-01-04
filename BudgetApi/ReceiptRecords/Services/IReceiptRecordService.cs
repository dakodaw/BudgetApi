using Budget.Models;
using System;
using System.Collections.Generic;

namespace BudgetApi.ReceiptRecords.Services;

public interface IReceiptRecordService
{
    IEnumerable<ReceiptRecord> List(int groupId);
    ReceiptRecord Get(Guid id);
    ReceiptRecord Add(ReceiptRecord record);
    //void Update(ReceiptRecord record);
    //void Delete(ReceiptRecord record);
}
