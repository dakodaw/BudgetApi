namespace BudgetApi.Shared;

public interface IBudgetAuthorizationService
{
    bool IsUserSystemAdmin(string externalLoginId);
    bool IsUserGroupAdmin(string externalLoginId, int groupId);
    bool IsUserInGroup(string externalLoginId, int groupId);
}
