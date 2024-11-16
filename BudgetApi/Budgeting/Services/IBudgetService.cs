using BudgetApi.Budgeting.Models;
using BudgetApi.Models;
using System;
using System.Collections.Generic;

namespace BudgetApi.Budgeting.Services
{
    public interface IBudgetService
    {
        List<BudgetWithPurchaseInfo> GetBudgetLines(int groupId, DateTime monthYear);
        int AddBudget(int groupId, BudgetEntry inputBudget);
        void UpdateBudget(BudgetEntry inputBudget);
        bool AddBudgetLines(int groupId, IEnumerable<BudgetEntry> inputBudgetLines);
        void DeleteBudgetEntry(int budgetId);
        BudgetInfo GetExistingBudget(int budgetId);
        decimal ScenarioCheck(int groupId, ScenarioInput scenarioInput);
    }
}
