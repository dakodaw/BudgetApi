using Budget.Models;

namespace Budget.DB.Incomes;

public interface IIncomeProvider
{
    IEnumerable<IncomeSource> GetIncomeSources(bool isActive = true);
    IEnumerable<Income> GetIncomes(DateTime monthYear);
    bool AddUpdateIncome(Income inputIncome, int incomeId = -1);
    int AddIncome(Income inputIncome);
    bool UpdateIncome(Income inputIncome);
    bool DeleteIncomeEntry(int incomeId);
    bool AddUpdateJob(IncomeSource inputJob, int incomeSourceId = -1);
    bool DeleteJobEntry(int incomeSourceId);
    IncomeSource GetIncomeSource(int incomeSourceId);
    Income GetExistingIncome(int incomeId);
}

