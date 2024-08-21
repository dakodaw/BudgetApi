namespace Budget.DB;

public class UsersBudgetingGroupEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int UserId { get; set; }
    public int GroupId { get; set; }
    public bool IsGroupAdmin { get; set; }


    public virtual BudgetingGroupEntity BudgetingGroup { get; set; }
    public virtual UserEntity User { get; set; }
}
