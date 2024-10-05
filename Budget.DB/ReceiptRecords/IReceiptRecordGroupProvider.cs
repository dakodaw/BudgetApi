using Budget.Models;

namespace Budget.DB.ReceiptRecordGroups;

public interface IReceiptRecordGroupProvider
{
    IEnumerable<ReceiptRecordGroup> List(Guid? recordGroupId);
    Guid Add(ReceiptRecordGroup inputReceiptRecordGroup);
    void Update(ReceiptRecordGroup inputReceiptRecordGroup);
    void Delete(Guid receiptRecordGroupSourceId);
    ReceiptRecordGroup Get(Guid receiptRecordGroupId);
}

