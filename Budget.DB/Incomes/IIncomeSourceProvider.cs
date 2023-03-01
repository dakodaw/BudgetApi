using Budget.Models;

namespace Budget.DB.Incomes;

public interface IIncomeSourceProvider
{
    IEnumerable<IncomeSource> GetIncomeSources();
    IncomeSource GetIncomeSource(int incomeSourceId);
    bool AddUpdateJob(IncomeSource inputJob, int incomeSourceId = -1);
    bool DeleteIncomeSource(int incomeSourceId);
}

