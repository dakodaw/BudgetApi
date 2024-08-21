namespace Budget.DB;

public class UserEntity
{
    public int Id { get; set; }
    public string UserSSOLoginId { get; set; }
    public bool IsSystemAdmin { get; set; }
}
