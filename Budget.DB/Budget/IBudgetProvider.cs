using BudgetApi.Models;

namespace Budget.DB.Budget;

public interface IBudgetProvider
{
    IEnumerable<BudgetType> GetBudgetTypes();
    bool AddBudget(BudgetEntry inputBudget);
    IEnumerable<BudgetEntry> GetBudgetEntries(DateTime monthYear);
}

