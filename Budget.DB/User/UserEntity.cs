namespace Budget.DB;

public class UserEntity
{
    public int Id { get; set; }
    public string UserSSOLoginId { get; set; }
    public int BudgetingGroupId { get; set; }
    public bool IsGroupAdmin { get; set; }
    public bool IsSystemAdmin { get; set; }
    public virtual BudgetingGroupEntity BudgetingGroup { get; set; }
}
