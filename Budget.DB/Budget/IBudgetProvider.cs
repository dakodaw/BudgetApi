using BudgetApi.Models;

namespace Budget.DB.Budget;

public interface IBudgetProvider
{
    IEnumerable<BudgetType> GetBudgetTypes(int groupId);
    BudgetType GetBudgetType(int budgetTypeId);
    bool AddUpdateBudgetType(BudgetTypeEntity budgetType, int budgetTypeId = -1);
    bool DeleteBudgetTypeEntry(int budgetTypeId);
    int AddBudget(int groupId, BudgetEntry inputBudget);
    void UpdateBudget(BudgetEntry inputBudget);
    void DeleteBudgetEntry(int budgetId);
    IEnumerable<BudgetEntry> GetBudgetEntries(int groupId, DateTime monthYear);
    IEnumerable<BudgetEntry> GetBudgetEntriesInTimeSpan(int groupId, DateTime startMonth, DateTime endMonth);
    BudgetEntry GetBudgetEntry(int budgetId);
    bool AddBudgetEntries(int groupId, IEnumerable<BudgetEntry> budgetEntries);
}

