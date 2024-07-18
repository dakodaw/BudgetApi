using Budget.DB.Budget;
using Budget.DB.BudgetTypes;
using BudgetApi.Budgeting.Models;
using BudgetApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetApi.BudgetTypes
{
    public class BudgetTypeService: IBudgetTypeService
    {
        //BudgetEntities _db;
        IBudgetTypeProvider _budgetTypeProvider;

        public BudgetTypeService(IBudgetProvider budgetProvider, IBudgetTypeProvider budgetTypeProvider)
        {
            //_db = db;
            _budgetTypeProvider = budgetTypeProvider;
        }

        public List<BudgetType> GetBudgetTypes()
        {
            return _budgetTypeProvider.GetBudgetTypes()
                .OrderBy(i => i.BudgetTypeName)
                .ToList();
        }

        public bool AddUpdateBudgetType(BudgetType budgetType, int budgetTypeId = -1)
        {
            return _budgetTypeProvider.AddUpdateBudgetType(budgetType, budgetTypeId);
        }

        public int AddBudgetType(BudgetType budgetType)
        {
            return _budgetTypeProvider.AddBudgetType(budgetType);
        }

        public void UpdateBudgetType(BudgetType budgetType)
        {
            _budgetTypeProvider.UpdateBudgetType(budgetType);
        }

        public void DeleteBudgetTypeEntry(int budgetTypeId)
        {
            _budgetTypeProvider.DeleteBudgetTypeEntry(budgetTypeId);
        }

        public BudgetType GetBudgetType(int budgetTypeId)
        {
            return _budgetTypeProvider.GetBudgetType(budgetTypeId);
        }
    }
}
