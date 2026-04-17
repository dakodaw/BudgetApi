using BudgetApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Budget.DB.BudgetTypes
{
    public class BudgetTypeProvider : IBudgetTypeProvider
    {
        BudgetEntities _db;

        public BudgetTypeProvider(BudgetEntities db)
        {
            _db = db;
        }

        public async Task<IEnumerable<BudgetType>> GetBudgetTypes()
        {
            return (await _db.BudgetTypes.ToListAsync()).Select(x => new BudgetType
            {
                BudgetTypeId = x.Id,
                BudgetTypeName = x.BudgetType
            });
        }

        public async Task<BudgetType> GetBudgetType(int budgetTypeId)
        {
            var matchingType = await _db.BudgetTypes
                .Where(i => i.Id == budgetTypeId).FirstOrDefaultAsync();

            return new BudgetType
            {
                BudgetTypeId = matchingType.Id,
                BudgetTypeName = matchingType.BudgetType
            };
        }

        public async Task<bool> AddUpdateBudgetType(int groupId, BudgetType budgetType, int budgetTypeId = -1)
        {
            if (budgetTypeId == -1)
            {
                try
                {
                    await AddBudgetType(groupId, budgetType);

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
                    await UpdateBudgetType(budgetType);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public async Task<int> AddBudgetType(int groupId, BudgetType budgetType)
        {
            try
            {
                var newBudgetType = new BudgetTypeEntity
                {
                    BudgetType = budgetType.BudgetTypeName,
                    BudgetingGroupId = groupId
                };

                await _db.BudgetTypes.AddAsync(newBudgetType);
                await _db.SaveChangesAsync();

                return newBudgetType.Id;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add Budget Type", ex);
            }
        }

        public async Task UpdateBudgetType(BudgetType budgetType)
        {
            try
            {
                var foundBudgetType = await _db.BudgetTypes.FindAsync(budgetType.BudgetTypeId);
                foundBudgetType.BudgetType = budgetType.BudgetTypeName;

                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update BudgetType", ex);
            }
        }

        public async Task DeleteBudgetTypeEntry(int budgetTypeId)
        {
            try
            {
                var toDelete = await _db.BudgetTypes.FindAsync(budgetTypeId);
                _db.BudgetTypes.Remove(toDelete);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Delete Budget Type", ex);
            }
        }
    }
}
