using Budget.Models;

namespace Budget.DB.Incomes;

public interface IIncomeSourceProvider
{
    IEnumerable<IncomeSource> GetIncomeSources(bool includeInactiveJobs = false);
    IncomeSource GetIncomeSource(int incomeSourceId);
    bool AddUpdateJob(IncomeSource inputJob, int incomeSourceId = -1);
    int AddIncomeSource(IncomeSource inputJob);
    void UpdateIncomeSource(IncomeSource inputJob);
    void DeleteIncomeSource(int incomeSourceId);
}

