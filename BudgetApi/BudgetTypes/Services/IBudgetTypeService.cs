using BudgetApi.Models;
using System.Collections.Generic;

namespace BudgetApi.BudgetTypes
{
    public interface IBudgetTypeService
    {
        List<BudgetType> GetBudgetTypes();
        bool AddUpdateBudgetType(int groupId, BudgetType budgetType, int budgetTypeId = -1);
        int AddBudgetType(int groupId, BudgetType budgetType);
        void UpdateBudgetType(BudgetType budgetType);
        void DeleteBudgetTypeEntry(int budgetTypeId);
        BudgetType GetBudgetType(int budgetTypeId);
    }
}
