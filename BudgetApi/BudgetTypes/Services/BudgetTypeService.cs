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

        public int AddBudgetType(BudgetType budgetType)
        {
            try
            {
                var newBudgetType = new BudgetTypeEntity
                {
                    BudgetType1 = budgetType.BudgetTypeName
                };

                _db.BudgetTypes.Add(newBudgetType);
                _db.SaveChanges();

                return newBudgetType.Id;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add Budget Type", ex);
            }
        }

        public bool UpdateBudgetType(BudgetType budgetType)
        {
            try
            {
                var foundBudgetType = _db.BudgetTypes.Find(budgetType.BudgetTypeId);
                foundBudgetType.BudgetType1 = budgetType.BudgetTypeName;

                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update BudgetType", ex);
            }
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
