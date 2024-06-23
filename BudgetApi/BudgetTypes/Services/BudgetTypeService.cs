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
            try
            {
                var toDelete = _db.BudgetTypes.Find(budgetTypeId);
                _db.BudgetTypes.Remove(toDelete);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete Budget Type", ex);
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
