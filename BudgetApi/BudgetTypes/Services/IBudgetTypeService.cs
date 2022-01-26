using BudgetApi.Models;
using System.Collections.Generic;

namespace BudgetApi.BudgetTypes
{
    public interface IBudgetTypeService
    {
        List<BudgetType> GetBudgetTypes();
        bool AddUpdateBudgetType(BudgetTypeEntity budgetType, int budgetTypeId = -1);
        bool DeleteBudgetTypeEntry(int budgetTypeId);
        BudgetType GetBudgetType(int budgetTypeId);
    }
}
