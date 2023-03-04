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

        public BudgetType GetBudgetType(int budgetTypeId)
        {
            var matchingType = _db.BudgetTypes
                .Where(i => i.Id == budgetTypeId).FirstOrDefault();

            return new BudgetType
            {
                BudgetTypeId = matchingType.Id,
                BudgetTypeName = matchingType.BudgetType1
            };
        }

        public BudgetEntry GetBudgetEntry(int budgetId)
        {
            return (from b in _db.Budgets.Where(i => i.Id == budgetId)
                    select new BudgetEntry
                    {
                        Amount = b.Amount,
                        Date = b.Date,
                        BudgetTypeId = b.BudgetTypeId,
                        Id = b.Id
                    }).FirstOrDefault();
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

        public bool UpdateBudget(BudgetEntry inputBudget)
        {
            var budgetId = inputBudget.Id;
            bool success = false;
            //Get the Budget from the Database with a given id
            //Update the Budget that matches the one from the database
            var selectedBudgetEntry = _db.Budgets.Where(i => i.Id == budgetId).FirstOrDefault();
            _db.Budgets.Where(i => i.Id == budgetId).FirstOrDefault().Amount = inputBudget.Amount;
            _db.Budgets.Where(i => i.Id == budgetId).FirstOrDefault().BudgetTypeId = inputBudget.BudgetTypeId;
            _db.Budgets.Where(i => i.Id == budgetId).FirstOrDefault().Date = inputBudget.Date;

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
            try
            {
                var checkBudget = _db.Budgets
                    .Where(i => i.Amount == inputBudget.Amount && i.Date == inputBudget.Date)
                    .FirstOrDefault();

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

        public bool AddBudgetEntries(IEnumerable<BudgetEntry> budgetEntries)
        {
            bool success = false;
            try
            {
                _db.Budgets.AddRange(budgetEntries.Select(x => new BudgetEntity
                {
                    Amount = x.Amount,
                    BudgetTypeId = x.BudgetTypeId,
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

