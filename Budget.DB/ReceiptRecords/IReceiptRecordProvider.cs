using Budget.Models;

namespace Budget.DB.Incomes;

public interface IReceiptRecordProvider
{
    IEnumerable<ReceiptRecord> List(int groupId, DateTime? monthYear = null);
    ReceiptRecord Get(Guid id);
    Guid Add(int groupId, ReceiptRecord inputReceiptRecord);
    void Update(ReceiptRecord inputReceiptRecord);
    void Delete(Guid id);
}

