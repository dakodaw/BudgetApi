using Budget.Models;

namespace Budget.DB.Incomes;

public interface IReceiptRecordProvider
{
    Task<IEnumerable<ReceiptRecord>> List(int groupId, DateTime? monthYear = null);
    Task<ReceiptRecord> Get(Guid id);
    Task<Guid> Add(int groupId, ReceiptRecord inputReceiptRecord);
    Task Update(ReceiptRecord inputReceiptRecord);
    Task Delete(Guid id);
}

