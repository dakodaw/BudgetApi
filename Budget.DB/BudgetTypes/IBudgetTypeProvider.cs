using BudgetApi.Models;

namespace Budget.DB.BudgetTypes;

public interface IBudgetTypeProvider
{
    IEnumerable<BudgetType> GetBudgetTypes();
    BudgetType GetBudgetType(int budgetTypeId);
    bool AddUpdateBudgetType(int groupId, BudgetType budgetType, int budgetTypeId = -1);
    int AddBudgetType(int groupId, BudgetType budgetType);
    void UpdateBudgetType(BudgetType budgetType);
    void DeleteBudgetTypeEntry(int budgetTypeId);
}

