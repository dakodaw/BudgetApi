using Budget.DB.BudgetTypes;
using BudgetApi.Models;

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

		public IEnumerable<BudgetType> GetBudgetTypes()
		{
			return _budgetTypeProvider.GetBudgetTypes();
        }

        public BudgetType GetBudgetType(int budgetTypeId)
        {
            return _budgetTypeProvider.GetBudgetType(budgetTypeId);
        }

        public bool AddUpdateBudgetType(BudgetTypeEntity budgetType, int budgetTypeId = -1)
        {
            if (budgetTypeId == -1)
            {
                try
                {
                    _budgetTypeProvider.AddBudgetType(new BudgetType
                    {
                        BudgetTypeId = budgetType.Id,
                        BudgetTypeName = budgetType.BudgetType1
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
                    _budgetTypeProvider.UpdateBudgetType(new BudgetType
                    {
                        BudgetTypeId = budgetType.Id,
                        BudgetTypeName = budgetType.BudgetType1
                    });
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
                _budgetTypeProvider.DeleteBudgetTypeEntry(budgetTypeId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void DeleteBudgetEntry(int budgetId)
        {
            try
            {
                var toDelete = _db.Budgets.Find(budgetId);
                _db.Budgets.Remove(toDelete);
                _db.SaveChanges();
            }
            catch(Exception ex) 
            {
                throw new Exception("Failed to Delete Budget entry", ex);
            }
        }

        public BudgetEntry GetBudgetEntry(int budgetId)
        {
            return (from b in _db.Budgets.Where(i => i.Id == budgetId)
                    select new BudgetEntry
                    {
                        Amount = b.Amount,
                        Date = b.Date,
                        BudgetTypeId = b.BudgetTypeId,
                        BudgetingGroupId = b.BudgetingGroupId,
                        Id = b.Id
                    }).FirstOrDefault();
        }

		public int AddBudget(BudgetEntry inputBudget)
		{
            try
            {
                var newBudgetEntry = new BudgetEntity
                {
                    Amount = inputBudget.Amount,
                    BudgetTypeId = inputBudget.BudgetTypeId,
                    BudgetingGroupId = inputBudget.Id,
                    Date = inputBudget.Date,
                    Id = inputBudget.Id
                };
                _db.Budgets.Add(newBudgetEntry);
                _db.SaveChanges();
                return newBudgetEntry.Id;
            }
            catch(Exception ex)
            {
                throw new Exception("Failed To Add Budget Entry", ex);
            }
        }

        public void UpdateBudget(BudgetEntry inputBudget)
        {
            try
            {
                var budgetId = inputBudget.Id;
                //Get the Budget from the Database with a given id
                //Update the Budget that matches the one from the database
                var selectedBudgetEntry = _db.Budgets.Where(i => i.Id == budgetId).FirstOrDefault();
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
                _db.SaveChanges();
            }
            catch (Exception ex) 
            {
                throw new Exception("Failed to Update Budget Entry", ex);
            }
        }

        public IEnumerable<BudgetEntry> GetBudgetEntries(DateTime monthYear)
        {
            return (from b in _db.Budgets.Where(i => i.Date.Month == monthYear.Month && i.Date.Year == monthYear.Date.Year)
             join bt in _db.BudgetTypes on b.BudgetTypeId equals bt.Id
             select new BudgetEntry
             {
                 Id = b.Id,
                 BudgetTypeId = b.BudgetTypeId,
                 BudgetingGroupId = b.BudgetingGroupId,
                 Date = b.Date,
                 Amount = b.Amount
             }).ToList();
        }

        public IEnumerable<BudgetEntry> GetBudgetEntriesInTimeSpan(DateTime startMonth, DateTime endMonth)
        {
            return _db.Budgets
                .Where(i =>
                    i.Date >= startMonth &&
                    i.Date <= endMonth).Select(x => new BudgetEntry
                    {
                        Amount = x.Amount,
                        BudgetTypeId = x.BudgetTypeId,
                        BudgetingGroupId = x.BudgetingGroupId,
                        Date = x.Date,
                        Id = x.Id
                    });
        }

        public bool AddBudgetEntries(IEnumerable<BudgetEntry> budgetEntries) // TODO: Revisit this with the resulting ids
        {
            bool success = false;
            try
            {
                _db.Budgets.AddRange(budgetEntries.Select(x => new BudgetEntity
                {
                    Amount = x.Amount,
                    BudgetTypeId = x.BudgetTypeId,
                    BudgetingGroupId = x.BudgetingGroupId,
                    Id = x.Id,
                    Date = x.Date
                }));
                _db.SaveChanges();

                success = true;
            }
            catch
            {

            }
            return success;
        }
    }
}
