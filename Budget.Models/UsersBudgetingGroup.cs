namespace Budget.Models;

public class UsersBudgetingGroup
{
    public Guid Id { get; set; }
    public int UserId { get; set; }
    public int GroupId { get; set; }
    public bool IsGroupAdmin { get; set; }
}
