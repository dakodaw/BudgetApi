using Budget.Models;

namespace Budget.DB.Incomes;

public interface IIncomeSourceProvider
{
    IEnumerable<IncomeSource> GetIncomeTypes();
}

