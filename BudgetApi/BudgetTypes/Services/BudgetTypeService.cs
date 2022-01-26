using BudgetApi.Budgeting.Models;
using BudgetApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetApi.BudgetTypes
{
    public class BudgetTypeService: IBudgetTypeService
    {
        BudgetEntities _db;

        public BudgetTypeService(BudgetEntities db)
        {
            _db = db;
        }

        public List<BudgetType> GetBudgetTypes()
        {
            var budgetTypes = (from bt in _db.BudgetTypes
                               select new BudgetType
                               {
                                   BudgetTypeId = bt.Id,
                                   BudgetTypeName = bt.BudgetType1
                               }).ToList();
            return budgetTypes.OrderBy(i => i.BudgetTypeName).ToList();
        }

        public bool AddUpdateBudgetType(BudgetTypeEntity budgetType, int budgetTypeId = -1)
        {
            if (budgetTypeId == -1)
            {
                try
                {
                    _db.BudgetTypes.Add(new BudgetTypeEntity
                    {
                        BudgetType1 = budgetType.BudgetType1
                    });
                    _db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                try
                {
                    _db.BudgetTypes.Find(budgetTypeId).BudgetType1 = budgetType.BudgetType1;
                    _db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
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
