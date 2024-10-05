namespace Budget.Models;

public class ReceiptRecord
{
    public Guid Id { get; set; }
    public int BudgetingGroupId { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string Location { get; set; }
    public IEnumerable<ReceiptRecordGroup> ReceiptRecords { get; set; } = new List<ReceiptRecordGroup>();
}
