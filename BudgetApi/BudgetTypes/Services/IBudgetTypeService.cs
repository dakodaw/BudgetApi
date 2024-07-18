using BudgetApi.Models;
using System.Collections.Generic;

namespace BudgetApi.BudgetTypes
{
    public interface IBudgetTypeService
    {
        List<BudgetType> GetBudgetTypes();
        bool AddUpdateBudgetType(BudgetTypeEntity budgetType, int budgetTypeId = -1);
        int AddBudgetType(BudgetType budgetType);
        bool UpdateBudgetType(BudgetType budgetType);
        bool DeleteBudgetTypeEntry(int budgetTypeId);
        BudgetType GetBudgetType(int budgetTypeId);
    }
}
