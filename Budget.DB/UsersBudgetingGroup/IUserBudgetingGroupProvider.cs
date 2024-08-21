using Budget.Models;

namespace Budget.DB.BudgetingGroups;

public interface IUserBudgetingGroupProvider
{
    Guid AddUserBudgetingGroup(UsersBudgetingGroup user);
    void UpdateUserBudgetingGroup(UsersBudgetingGroup user);
    UsersBudgetingGroup GetUserBudgetingGroup(Guid id);
    IEnumerable<UsersBudgetingGroup> GetUserBudgetingGroupsByUserId(int userId);
    IEnumerable<UsersBudgetingGroup> GetUserBudgetingGroups();
    void DeleteUserBudgetingGroup(Guid id);
}
