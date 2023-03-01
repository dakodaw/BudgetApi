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

    public IEnumerable<IncomeSource> GetIncomeSources()
    {
        return (from it in _db.IncomeSources
                select new IncomeSource
                {
                    Id = it.Id,
                    SourceName = it.SourceName,
                    PositionName = it.PositionName,
                    JobOf = it.JobOf,
                    ActiveJob = it.ActiveJob,
                    EstimatedIncome = it.EstimatedIncome,
                    PayFrequency = it.PayFrequency
                }).ToList();
    }

    public IncomeSource GetIncomeSource(int incomeSourceId)
    {
        var incomeSource = _db.IncomeSources
            .Where(i => i.Id == incomeSourceId)
            .FirstOrDefault();

        return new IncomeSource
        {
            ActiveJob = incomeSource.ActiveJob,
            EstimatedIncome = incomeSource.EstimatedIncome,
            Id = incomeSource.Id,
            JobOf = incomeSource.JobOf,
            PayFrequency = incomeSource.PayFrequency,
            PositionName = incomeSource.PositionName,
            SourceName = incomeSource.SourceName
        };
    }

    public bool AddUpdateJob(IncomeSource inputJob, int incomeSourceId = -1)
    {
        if (incomeSourceId == -1)
        {
            var jobToAddUpdate = new IncomeSourceEntity
            {
                ActiveJob = true,
                EstimatedIncome = inputJob.EstimatedIncome,
                JobOf = inputJob.JobOf,
                PayFrequency = inputJob.PayFrequency,
                PositionName = inputJob.PositionName,
                SourceName = inputJob.SourceName
            };

            try
            {
                _db.IncomeSources.Add(jobToAddUpdate);
                _db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        else
        {
            var jobToAddUpdate = new IncomeSource
            {
                ActiveJob = true,
                EstimatedIncome = inputJob.EstimatedIncome,
                JobOf = inputJob.JobOf,
                PayFrequency = inputJob.PayFrequency,
                PositionName = inputJob.PositionName,
                SourceName = inputJob.SourceName
            };
            try
            {
                _db.IncomeSources.Find(incomeSourceId).ActiveJob = jobToAddUpdate.ActiveJob;
                _db.IncomeSources.Find(incomeSourceId).EstimatedIncome = jobToAddUpdate.EstimatedIncome;
                _db.IncomeSources.Find(incomeSourceId).JobOf = jobToAddUpdate.JobOf;
                _db.IncomeSources.Find(incomeSourceId).PayFrequency = jobToAddUpdate.PayFrequency;
                _db.IncomeSources.Find(incomeSourceId).PositionName = jobToAddUpdate.PositionName;
                _db.IncomeSources.Find(incomeSourceId).SourceName = jobToAddUpdate.SourceName;
                _db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }

    public bool DeleteIncomeSource(int incomeSourceId)
    {
        try
        {
            var toDelete = _db.IncomeSources.FirstOrDefault(x => x.Id == incomeSourceId);
            _db.IncomeSources.Remove(toDelete);
            _db.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}

