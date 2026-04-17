using Budget.DB.BudgetTypes;
using BudgetApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Budget.DB.Budget
{
	public class BudgetProvider: IBudgetProvider
	{
        BudgetEntities _db;
        IBudgetTypeProvider _budgetTypeProvider;

        public BudgetProvider(BudgetEntities db, IBudgetTypeProvider budgetTypeProvider)
		{
			_db = db;
            _budgetTypeProvider = budgetTypeProvider;
		}

		public async Task<IEnumerable<BudgetType>> GetBudgetTypes(int groupId)
		{
			return await _budgetTypeProvider.GetBudgetTypes();
        }

        public async Task<BudgetType> GetBudgetType(int budgetTypeId)
        {
            return await _budgetTypeProvider.GetBudgetType(budgetTypeId);
        }

        public async Task<bool> AddUpdateBudgetType(int groupId, BudgetTypeEntity budgetType, int budgetTypeId = -1)
        {
            if (budgetTypeId == -1)
            {
                try
                {
                    await _budgetTypeProvider.AddBudgetType(groupId, new BudgetType
                    {
                        BudgetTypeId = budgetType.Id,
                        BudgetTypeName = budgetType.BudgetType
                    });

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
                    await _budgetTypeProvider.UpdateBudgetType(new BudgetType
                    {
                        BudgetTypeId = budgetType.Id,
                        BudgetTypeName = budgetType.BudgetType
                    });

                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public async Task<bool> DeleteBudgetTypeEntry(int budgetTypeId)
        {
            try
            {
                await _budgetTypeProvider.DeleteBudgetTypeEntry(budgetTypeId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task DeleteBudgetEntry(int budgetId)
        {
            try
            {
                var toDelete = await _db.Budgets.FindAsync(budgetId);
                _db.Budgets.Remove(toDelete);
                await _db.SaveChangesAsync();
            }
            catch(Exception ex) 
            {
                throw new Exception("Failed to Delete Budget entry", ex);
            }
        }

        public async Task<BudgetEntry> GetBudgetEntry(int budgetId)
        {
            return await (from b in _db.Budgets.Where(i => i.Id == budgetId)
                    select new BudgetEntry
                    {
                        Amount = b.Amount,
                        Date = b.Date,
                        BudgetTypeId = b.BudgetTypeId,
                        BudgetingGroupId = b.BudgetingGroupId,
                        Id = b.Id
                    }).FirstOrDefaultAsync();
        }

		public async Task<int> AddBudget(int groupId, BudgetEntry inputBudget)
		{
            try
            {
                var newBudgetEntry = new BudgetEntity
                {
                    Amount = inputBudget.Amount,
                    BudgetTypeId = inputBudget.BudgetTypeId,
                    BudgetingGroupId = inputBudget.BudgetingGroupId,
                    Date = inputBudget.Date,
                    Id = inputBudget.Id
                };
                await _db.Budgets.AddAsync(newBudgetEntry);
                await _db.SaveChangesAsync();
                return newBudgetEntry.Id;
            }
            catch(Exception ex)
            {
                throw new Exception("Failed To Add Budget Entry", ex);
            }
        }

        public async Task UpdateBudget(BudgetEntry inputBudget)
        {
            try
            {
                var budgetId = inputBudget.Id;
                //Get the Budget from the Database with a given id
                //Update the Budget that matches the one from the database
                var selectedBudgetEntry = await _db.Budgets.Where(i => i.Id == budgetId).FirstOrDefaultAsync();
                selectedBudgetEntry.Amount = inputBudget.Amount;
                selectedBudgetEntry.BudgetTypeId = inputBudget.BudgetTypeId;
                selectedBudgetEntry.BudgetingGroupId = inputBudget.BudgetingGroupId;
                selectedBudgetEntry.Date = inputBudget.Date;

                //// Alternate approach
                //_db.Entry(selectedBudgetEntry).CurrentValues.SetValues(new
                //{
                //    Amount = inputBudget.Amount,
                //    BudgetTypeId = selectedBudgetEntry.BudgetTypeId,
                //    BudgetType = selectedBudgetEntry?.BudgetType,
                //    Date = inputBudget.Date
                //});

                //Save Changes
                await _db.SaveChangesAsync();
            }
            catch (Exception ex) 
            {
                throw new Exception("Failed to Update Budget Entry", ex);
            }
        }

        public async Task<IEnumerable<BudgetEntry>> GetBudgetEntries(int groupId, DateTime monthYear)
        {
            return await (from b in _db.Budgets.Where(i => i.Date.Month == monthYear.Month && i.Date.Year == monthYear.Date.Year)
             join bt in _db.BudgetTypes on b.BudgetTypeId equals bt.Id
             select new BudgetEntry
             {
                 Id = b.Id,
                 BudgetTypeId = b.BudgetTypeId,
                 BudgetingGroupId = b.BudgetingGroupId,
                 Date = b.Date,
                 Amount = b.Amount
             }).ToListAsync();
        }

        public async Task<IEnumerable<BudgetEntry>> GetBudgetEntriesInTimeSpan(int groupId, DateTime startMonth, DateTime endMonth)
        {
            return await _db.Budgets
                .Where(i =>
                    i.Date >= startMonth &&
                    i.Date <= endMonth).Select(x => new BudgetEntry
                    {
                        Amount = x.Amount,
                        BudgetTypeId = x.BudgetTypeId,
                        BudgetingGroupId = x.BudgetingGroupId,
                        Date = x.Date,
                        Id = x.Id
                    }).ToListAsync();
        }

        public async Task<bool> AddBudgetEntries(int groupId, IEnumerable<BudgetEntry> budgetEntries) // TODO: Revisit this with the resulting ids
        {
            bool success = false;
            try
            {
                await _db.Budgets.AddRangeAsync(budgetEntries.Select(x => new BudgetEntity
                {
                    Amount = x.Amount,
                    BudgetTypeId = x.BudgetTypeId,
                    BudgetingGroupId = x.BudgetingGroupId,
                    Id = x.Id,
                    Date = x.Date
                }));
                
                await _db.SaveChangesAsync();

                success = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Add Budget Range", ex);
            }

            return success;
        }
    }
}
