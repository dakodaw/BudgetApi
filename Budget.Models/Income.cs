namespace Budget.Models;

public class Income
{
    public int Id { get; set; }
    public int SourceId { get; set; }
    public string SourceDetails { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public bool IsReimbursement { get; set; }
    public int? PurchaseId { get; set; }
    public bool IsCash { get; set; }
}
