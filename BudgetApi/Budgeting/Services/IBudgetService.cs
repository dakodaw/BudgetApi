using BudgetApi.Budgeting.Models;
using BudgetApi.Models;
using System;
using System.Collections.Generic;

namespace BudgetApi.Budgeting.Services
{
    public interface IBudgetService
    {
        List<BudgetWithPurchaseInfo> GetBudgetLines(DateTime monthYear);
        int AddBudget(BudgetEntry inputBudget);
        void UpdateBudget(BudgetEntry inputBudget);
        bool AddBudgetLines(IEnumerable<BudgetEntry> inputBudgetLines);
        void DeleteBudgetEntry(int budgetId);
        BudgetInfo GetExistingBudget(int budgetId);
        decimal ScenarioCheck(ScenarioInput scenarioInput);
    }
}
