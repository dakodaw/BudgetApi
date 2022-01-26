using BudgetApi.Budgeting.Models;
using BudgetApi.Models;
using System;
using System.Collections.Generic;

namespace BudgetApi.Budgeting.Services
{
    public interface IBudgetService
    {
        List<BudgetWithPurchaseInfo> GetBudgetLines(DateTime monthYear);
        bool AddUpdateBudget(Budget inputBudget, int budgetId = -1);
        bool DeleteBudgetEntry(int budgetId);
        BudgetInfo GetExistingBudget(int budgetId);
        double ScenarioCheck(ScenarioInput scenarioInput);
    }
}
