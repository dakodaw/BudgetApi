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
            try
            {
                var toDelete = _db.BudgetTypes.Find(budgetTypeId);
                _db.BudgetTypes.Remove(toDelete);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public BudgetType GetBudgetType(int budgetTypeId)
        {
            return (from b in _db.BudgetTypes
                    where b.Id == budgetTypeId
                    select new BudgetType
                    {
                        BudgetTypeId = b.Id,
                        BudgetTypeName = b.BudgetType1
                    }).FirstOrDefault();
        }
    }
}
