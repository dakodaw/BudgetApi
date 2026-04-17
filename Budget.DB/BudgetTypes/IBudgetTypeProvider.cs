using BudgetApi.Models;

namespace Budget.DB.BudgetTypes;

public interface IBudgetTypeProvider
{
    Task<IEnumerable<BudgetType>> GetBudgetTypes();
    Task<BudgetType> GetBudgetType(int budgetTypeId);
    Task<bool> AddUpdateBudgetType(int groupId, BudgetType budgetType, int budgetTypeId = -1);
    Task<int> AddBudgetType(int groupId, BudgetType budgetType);
    Task UpdateBudgetType(BudgetType budgetType);
    Task DeleteBudgetTypeEntry(int budgetTypeId);
}

