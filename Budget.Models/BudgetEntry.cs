namespace BudgetApi.Models;

public partial class BudgetEntry
{
    public int Id { get; set; }
    public int BudgetTypeId { get; set; }
    public System.DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public int BudgetingGroupId { get; set; }

    public virtual BudgetType BudgetType { get; set; }
}
