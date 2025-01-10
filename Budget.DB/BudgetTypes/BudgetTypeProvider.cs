using BudgetApi.Models;

namespace Budget.DB.BudgetTypes
{
    public class BudgetTypeProvider : IBudgetTypeProvider
    {
        BudgetEntities _db;

        public BudgetTypeProvider(BudgetEntities db)
        {
            _db = db;
        }

        public IEnumerable<BudgetType> GetBudgetTypes()
        {
            return _db.BudgetTypes.Select(x => new BudgetType
            {
                BudgetTypeId = x.Id,
                BudgetTypeName = x.BudgetType
            });
        }

        public BudgetType GetBudgetType(int budgetTypeId)
        {
            var matchingType = _db.BudgetTypes
                .Where(i => i.Id == budgetTypeId).FirstOrDefault();

            return new BudgetType
            {
                BudgetTypeId = matchingType.Id,
                BudgetTypeName = matchingType.BudgetType
            };
        }

        public bool AddUpdateBudgetType(int groupId, BudgetType budgetType, int budgetTypeId = -1)
        {
            if (budgetTypeId == -1)
            {
                try
                {
                    AddBudgetType(groupId, budgetType);

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
                    UpdateBudgetType(budgetType);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public int AddBudgetType(int groupId, BudgetType budgetType)
        {
            try
            {
                var newBudgetType = new BudgetTypeEntity
                {
                    BudgetType = budgetType.BudgetTypeName,
                    BudgetingGroupId = groupId
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

        public void UpdateBudgetType(BudgetType budgetType)
        {
            try
            {
                var foundBudgetType = _db.BudgetTypes.Find(budgetType.BudgetTypeId);
                foundBudgetType.BudgetType = budgetType.BudgetTypeName;

                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update BudgetType", ex);
            }
        }

        public void DeleteBudgetTypeEntry(int budgetTypeId)
        {
            try
            {
                var toDelete = _db.BudgetTypes.Find(budgetTypeId);
                _db.BudgetTypes.Remove(toDelete);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Delete Budget Type", ex);
            }
        }
    }
}
