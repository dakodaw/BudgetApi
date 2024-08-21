using Budget.Models;

namespace Budget.DB.BudgetingGroups;

public interface IBudgetingGroupProvider
{
    int AddBudgetingGroup(BudgetingGroup user);
    void UpdateBudgetingGroup(BudgetingGroup user);
    BudgetingGroup GetBudgetingGroup(int id);
    //BudgetingGroup GetBudgetingGroupByExternalLoginId(string loginId);
    IEnumerable<BudgetingGroup> GetBudgetingGroups();
    void DeleteBudgetingGroup(int id);
}
