using BudgetApi.Budgeting.Models;
using BudgetApi.Models;
using System;
using System.Collections.Generic;

namespace BudgetApi.Budgeting.Services
{
    public interface IBudgetService
    {
        List<BudgetWithPurchaseInfo> GetBudgetLines(DateTime monthYear);
        bool AddBudget(Budget inputBudget);
        bool UpdateBudget(Budget inputBudget, int budgetId);
        bool AddBudgetLines(IEnumerable<Budget> inputBudgetLines);
        bool DeleteBudgetEntry(int budgetId);
        BudgetInfo GetExistingBudget(int budgetId);
        double ScenarioCheck(ScenarioInput scenarioInput);
    }
}
