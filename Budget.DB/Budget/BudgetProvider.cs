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
				Id = x.Id,
				BudgetType1 = x.BudgetType1
			});
        }
	}
}

