using Budget.Models;

namespace Budget.DB.ReceiptRecordGroups;

public interface IReceiptRecordGroupProvider
{
    Task<IEnumerable<ReceiptRecordGroup>> List(Guid? recordGroupId);
    Task<Guid> Add(ReceiptRecordGroup inputReceiptRecordGroup);
    Task Update(ReceiptRecordGroup inputReceiptRecordGroup);
    Task Delete(Guid receiptRecordGroupSourceId);
    Task<ReceiptRecordGroup> Get(Guid receiptRecordGroupId);
}

