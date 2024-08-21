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

    public IEnumerable<IncomeSource> GetIncomeSources(bool includeInactiveJobs = false)
    {
        var jobs = (from it in _db.IncomeSources
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

        return includeInactiveJobs
            ? jobs
            : jobs.Where(x => x.ActiveJob = !includeInactiveJobs);
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
            try
            {
                AddIncomeSource(inputJob);
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
                UpdateIncomeSource(inputJob);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    public int AddIncomeSource(IncomeSource inputJob)
    {
        try
        {
            var jobToAdd = new IncomeSourceEntity
            {
                ActiveJob = true,
                EstimatedIncome = inputJob.EstimatedIncome,
                JobOf = inputJob.JobOf,
                PayFrequency = inputJob.PayFrequency,
                PositionName = inputJob.PositionName,
                SourceName = inputJob.SourceName
            };

            _db.IncomeSources.Add(jobToAdd);
            _db.SaveChanges();

            return jobToAdd.Id;
        }
        catch(Exception ex) 
        {
            throw new Exception("Failed to Add Income Source", ex);
        }
    }
    public void UpdateIncomeSource(IncomeSource inputJob)
    {
        try
        {
            var jobToAddUpdate = _db.IncomeSources.Find(inputJob.Id);
            jobToAddUpdate.ActiveJob = inputJob.ActiveJob;
            jobToAddUpdate.EstimatedIncome = inputJob.EstimatedIncome;
            jobToAddUpdate.JobOf = inputJob.JobOf;
            jobToAddUpdate.PayFrequency = inputJob.PayFrequency;
            jobToAddUpdate.PositionName = inputJob.PositionName;
            jobToAddUpdate.SourceName = inputJob.SourceName;
            
            _db.SaveChanges();
        }
        catch(Exception ex)
        {
            throw new Exception("Failed to update Income Source", ex);
        }
    }

    public void DeleteIncomeSource(int incomeSourceId)
    {
        try
        {
            var toDelete = _db.IncomeSources.FirstOrDefault(x => x.Id == incomeSourceId);
            _db.IncomeSources.Remove(toDelete);
            _db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to Delete Income Source {incomeSourceId}", ex);
        }
    }
}

