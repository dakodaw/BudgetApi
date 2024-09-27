using Budget.Models;

namespace Budget.DB.Incomes;

public interface IReceiptRecordProvider
{
    IEnumerable<ReceiptRecord> List();
    ReceiptRecord Get(Guid id);
    Guid Add(ReceiptRecord inputReceiptRecord);
    void Update(ReceiptRecord inputReceiptRecord);
    void Delete(Guid id);
}

