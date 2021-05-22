using BudgetApi.Models;
using BudgetApi.Shared;
using System;
using System.Collections.Generic;

namespace BudgetApi.Budgeting.Services
{
    public interface IBudgetService
    {
        List<BudgetTypes> GetBudgetTypes();
        List<BudgetWithPurchaseInfo> GetBudgetLines(DateTime monthYear);
        bool AddUpdateBudget(Budget inputBudget, int budgetId = -1);
        bool DeleteBudgetEntry(int budgetId);
        BudgetInfo GetExistingBudget(int budgetId);
        double ScenarioCheck(ScenarioInput scenarioInput);
        bool AddUpdateBudgetType(BudgetType budgetType, int budgetTypeId = -1);
        bool DeleteBudgetTypeEntry(int budgetTypeId);
        BudgetTypes GetBudgetType(int budgetTypeId);
    }
}
