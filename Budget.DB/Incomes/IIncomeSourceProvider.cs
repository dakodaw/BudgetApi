using Budget.Models;

namespace Budget.DB.Incomes;

public interface IIncomeSourceProvider
{
    Task<IEnumerable<IncomeSource>> GetIncomeSources(bool includeInactiveJobs = false);
    Task<IncomeSource> GetIncomeSource(int incomeSourceId);
    Task<bool> AddUpdateJob(IncomeSource inputJob, int incomeSourceId = -1);
    Task<int> AddIncomeSource(IncomeSource inputJob);
    Task UpdateIncomeSource(IncomeSource inputJob);
    Task DeleteIncomeSource(int incomeSourceId);
}

