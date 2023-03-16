using Budget.DB.Budget;
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
        IBudgetProvider _budgetProvider;

        public BudgetTypeService(IBudgetProvider budgetProvider)
        {
            //_db = db;
            _budgetProvider = budgetProvider;
        }

        public List<BudgetType> GetBudgetTypes()
        {
            return _budgetProvider.GetBudgetTypes()
                .OrderBy(i => i.BudgetTypeName)
                .ToList();
        }

        public bool AddUpdateBudgetType(BudgetTypeEntity budgetType, int budgetTypeId = -1)
        {
            return _budgetProvider.AddUpdateBudgetType(budgetType, budgetTypeId);
        }

        public bool DeleteBudgetTypeEntry(int budgetTypeId)
        {
            return _budgetProvider.DeleteBudgetTypeEntry(budgetTypeId);
        }

        public BudgetType GetBudgetType(int budgetTypeId)
        {
            return _budgetProvider.GetBudgetType(budgetTypeId);
        }
    }
}
