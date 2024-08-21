namespace BudgetApi.BudgetGroups;

public class AddUserToGroupRequest
{
    public string ExternalLoginId { get; set; }
    public int GroupId { get; set; }
    public bool IsGroupAdmin { get; set; }
}
