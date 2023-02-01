using Budget.Models;
using BudgetApi.Models;

namespace Budget.DB.Incomes;

public class IncomeSourceProvider: IIncomeSourceProvider
{
    BudgetEntities _db;

	public IncomeSourceProvider(BudgetEntities db)
	{
        _db = db;
	}

    public IEnumerable<IncomeSource> GetIncomeTypes()
    {
        return (from it in _db.IncomeSources
                select new IncomeSource
                {
                    Id = it.Id,
                    SourceName = it.SourceName,
                    PositionName = it.PositionName,
                    JobOf = it.JobOf,
                    ActiveJob = it.ActiveJob
                }).ToList();
    }
}

