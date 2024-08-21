namespace Budget.Models;

public class User
{
    public int Id { get; set; }
    public string UserSSOLoginId { get; set; }
    public bool IsSystemAdmin { get; set; }
}
