using BudgetApi.Models;

namespace Budget.DB.Budget;

public interface IBudgetProvider
{
    Task<IEnumerable<BudgetType>> GetBudgetTypes(int groupId);
    Task<BudgetType> GetBudgetType(int budgetTypeId);
    Task<bool> AddUpdateBudgetType(int groupId, BudgetTypeEntity budgetType, int budgetTypeId = -1);
    Task<bool> DeleteBudgetTypeEntry(int budgetTypeId);
    Task<int> AddBudget(int groupId, BudgetEntry inputBudget);
    Task UpdateBudget(BudgetEntry inputBudget);
    Task DeleteBudgetEntry(int budgetId);
    Task<IEnumerable<BudgetEntry>> GetBudgetEntries(int groupId, DateTime monthYear);
    Task<IEnumerable<BudgetEntry>> GetBudgetEntriesInTimeSpan(int groupId, DateTime startMonth, DateTime endMonth);
    Task<BudgetEntry> GetBudgetEntry(int budgetId);
    Task<bool> AddBudgetEntries(int groupId, IEnumerable<BudgetEntry> budgetEntries);
}

