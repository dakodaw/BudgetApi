using BudgetApi.Models;

namespace Budget.DB.Budget
{
	public class BudgetProvider: IBudgetProvider
	{
        BudgetEntities _db;

        public BudgetProvider(BudgetEntities db)
		{
			_db = db;
		}

		public IEnumerable<BudgetType> GetBudgetTypes()
		{
			return _db.BudgetTypes.Select(x => new BudgetType
			{
				BudgetTypeId = x.Id,
				BudgetTypeName = x.BudgetType1
			});
        }

		public bool AddBudget(BudgetEntry inputBudget)
		{
            bool success = false;
            _db.Budgets.Add(new BudgetEntity
            {
                Amount = inputBudget.Amount,
                BudgetTypeId = inputBudget.BudgetTypeId,
                Date = inputBudget.Date,
                Id = inputBudget.Id
            });
            _db.SaveChanges();
            try
            {
                var checkBudget = _db.Budgets.Where(i => i.Amount == inputBudget.Amount && i.Date == inputBudget.Date).FirstOrDefault();
                success = true;
            }
            catch
            {

            }
            return success;
        }

        public IEnumerable<BudgetEntry> GetBudgetEntries(DateTime monthYear)
        {
            return (from b in _db.Budgets.Where(i => i.Date.Month == monthYear.Month && i.Date.Year == monthYear.Date.Year)
             join bt in _db.BudgetTypes on b.BudgetTypeId equals bt.Id
             select new BudgetEntry
             {
                 Id = b.Id,
                 BudgetTypeId = b.BudgetTypeId,
                 Date = b.Date,
                 Amount = b.Amount
             }).ToList();
        }

    }
}

