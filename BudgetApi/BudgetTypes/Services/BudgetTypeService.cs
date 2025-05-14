using Budget.DB.Budget;
using Budget.DB.BudgetTypes;
using BudgetApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetApi.BudgetTypes
{
    public class BudgetTypeService: IBudgetTypeService
    {
        IBudgetTypeProvider _budgetTypeProvider;

        public BudgetTypeService(IBudgetProvider budgetProvider, IBudgetTypeProvider budgetTypeProvider)
        {
            _budgetTypeProvider = budgetTypeProvider;
        }

        public async Task<List<BudgetType>> GetBudgetTypes()
        {
            return (await _budgetTypeProvider.GetBudgetTypes())
                .OrderBy(i => i.BudgetTypeName)
                .ToList();
        }

        public async Task<bool> AddUpdateBudgetType(int groupId, BudgetType budgetType, int budgetTypeId = -1)
        {
            return await _budgetTypeProvider.AddUpdateBudgetType(groupId, budgetType, budgetTypeId);
        }

        public async Task<int> AddBudgetType(int groupId, BudgetType budgetType)
        {
            return await _budgetTypeProvider.AddBudgetType(groupId, budgetType);
        }

        public async Task UpdateBudgetType(BudgetType budgetType)
        {
            await _budgetTypeProvider.UpdateBudgetType(budgetType);
        }

        public async Task DeleteBudgetTypeEntry(int budgetTypeId)
        {
            await _budgetTypeProvider.DeleteBudgetTypeEntry(budgetTypeId);
        }

        public async Task<BudgetType> GetBudgetType(int budgetTypeId)
        {
            return await _budgetTypeProvider.GetBudgetType(budgetTypeId);
        }
    }
}
