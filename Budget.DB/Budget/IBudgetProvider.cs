using BudgetApi.Models;

namespace Budget.DB.Budget;

public interface IBudgetProvider
{
    IEnumerable<BudgetType> GetBudgetTypes();
    BudgetType GetBudgetType(int budgetTypeId);
    bool AddBudget(BudgetEntry inputBudget);
    bool UpdateBudget(BudgetEntry inputBudget);
    bool DeleteBudgetEntry(int budgetId);
    IEnumerable<BudgetEntry> GetBudgetEntries(DateTime monthYear);
    BudgetEntry GetBudgetEntry(int budgetId);
    bool AddBudgetEntries(IEnumerable<BudgetEntry> budgetEntries);
}

