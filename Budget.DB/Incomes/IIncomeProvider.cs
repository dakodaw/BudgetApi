using Budget.Models;

namespace Budget.DB.Incomes;

public interface IIncomeProvider
{
    Task<IEnumerable<IncomeSource>> GetIncomeSources(int groupId, bool isActive = true);
    Task<IEnumerable<Income>> GetIncomes(int groupId, DateTime monthYear);
    Task<bool> AddUpdateIncome(int groupId, Income inputIncome, int incomeId = -1);
    Task<int> AddIncome(int groupId, Income inputIncome);
    Task<bool> UpdateIncome(Income inputIncome);
    Task<bool> DeleteIncomeEntry(int incomeId);
    Task<bool> AddUpdateJob(IncomeSource inputJob, int incomeSourceId = -1);
    Task<bool> DeleteJobEntry(int incomeSourceId);
    Task<IncomeSource> GetIncomeSource(int incomeSourceId);
    Task<Income> GetExistingIncome(int incomeId);
}

