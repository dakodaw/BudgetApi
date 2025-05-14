using BudgetApi.Budgeting.Models;
using BudgetApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BudgetApi.Budgeting.Services;

public interface IBudgetService
{
    Task<List<BudgetWithPurchaseInfo>> GetBudgetLines(int groupId, DateTime monthYear);
    Task<int> AddBudget(int groupId, BudgetEntry inputBudget);
    Task UpdateBudget(BudgetEntry inputBudget);
    Task<bool> AddBudgetLines(int groupId, IEnumerable<BudgetEntry> inputBudgetLines);
    Task DeleteBudgetEntry(int budgetId);
    Task<BudgetInfo> GetExistingBudget(int budgetId);
    Task<decimal> ScenarioCheck(int groupId, ScenarioInput scenarioInput);
}
