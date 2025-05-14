using Budget.Models;
using BudgetApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Budget.DB.Incomes;

public class IncomeSourceProvider: IIncomeSourceProvider
{
    BudgetEntities _db;

	public IncomeSourceProvider(BudgetEntities db)
	{
        _db = db;
	}

    public async Task<IEnumerable<IncomeSource>> GetIncomeSources(bool includeInactiveJobs = false)
    {
        var jobs = await (from it in _db.IncomeSources
                select new IncomeSource
                {
                    Id = it.Id,
                    SourceName = it.SourceName,
                    PositionName = it.PositionName,
                    JobOf = it.JobOf,
                    ActiveJob = it.ActiveJob,
                    EstimatedIncome = it.EstimatedIncome,
                    PayFrequency = it.PayFrequency
                }).ToListAsync();

        return includeInactiveJobs
            ? jobs
            : jobs.Where(x => x.ActiveJob = !includeInactiveJobs);
    }

    public async Task<IncomeSource> GetIncomeSource(int incomeSourceId)
    {
        var incomeSource = await _db.IncomeSources
            .Where(i => i.Id == incomeSourceId)
            .FirstOrDefaultAsync();

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

    public async Task<bool> AddUpdateJob(IncomeSource inputJob, int incomeSourceId = -1)
    {
        if (incomeSourceId == -1)
        {
            try
            {
                await AddIncomeSource(inputJob);
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
                await UpdateIncomeSource(inputJob);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    public async Task<int> AddIncomeSource(IncomeSource inputJob)
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

            await _db.IncomeSources.AddAsync(jobToAdd);
            await _db.SaveChangesAsync();

            return jobToAdd.Id;
        }
        catch(Exception ex) 
        {
            throw new Exception("Failed to Add Income Source", ex);
        }
    }
    public async Task UpdateIncomeSource(IncomeSource inputJob)
    {
        try
        {
            var jobToAddUpdate = await _db.IncomeSources.FindAsync(inputJob.Id);
            jobToAddUpdate.ActiveJob = inputJob.ActiveJob;
            jobToAddUpdate.EstimatedIncome = inputJob.EstimatedIncome;
            jobToAddUpdate.JobOf = inputJob.JobOf;
            jobToAddUpdate.PayFrequency = inputJob.PayFrequency;
            jobToAddUpdate.PositionName = inputJob.PositionName;
            jobToAddUpdate.SourceName = inputJob.SourceName;
            
            await _db.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            throw new Exception("Failed to update Income Source", ex);
        }
    }

    public async Task DeleteIncomeSource(int incomeSourceId)
    {
        try
        {
            var toDelete = await _db.IncomeSources.FirstOrDefaultAsync(x => x.Id == incomeSourceId);
            _db.IncomeSources.Remove(toDelete);
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to Delete Income Source {incomeSourceId}", ex);
        }
    }
}

