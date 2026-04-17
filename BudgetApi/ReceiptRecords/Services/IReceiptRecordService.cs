using Budget.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BudgetApi.ReceiptRecords.Services;

public interface IReceiptRecordService
{
    Task<IEnumerable<ReceiptRecord>> List(int groupId, DateTime? monthYear = null);
    Task<ReceiptRecord> Get(Guid id);
    Task<ReceiptRecord> Add(int groupId, ReceiptRecord record);
    Task<ReceiptRecord> Update(int groupId, ReceiptRecord record);
    Task Delete(Guid id);
}
