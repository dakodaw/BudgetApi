using BudgetApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BudgetApi.BudgetTypes
{
    public interface IBudgetTypeService
    {
        Task<List<BudgetType>> GetBudgetTypes();
        Task<bool> AddUpdateBudgetType(int groupId, BudgetType budgetType, int budgetTypeId = -1);
        Task<int> AddBudgetType(int groupId, BudgetType budgetType);
        Task UpdateBudgetType(BudgetType budgetType);
        Task DeleteBudgetTypeEntry(int budgetTypeId);
        Task<BudgetType> GetBudgetType(int budgetTypeId);
    }
}
