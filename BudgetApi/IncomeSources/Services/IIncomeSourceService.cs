using Budget.DB;
using Budget.Models;
using BudgetApi.Incomes.Models;
using BudgetApi.Models;
using System.Collections.Generic;

namespace BudgetApi.Incomes.Services
{
    public interface IIncomeSourceService
    {
        bool AddUpdateIncomeSource(IncomeSourceEntity inputIncomeSource, int incomeSourceId = -1);
        int AddIncomeSource(IncomeSourceEntity inputIncomeSource);
        int AddIncomeSource(IncomeSource inputIncomeSource);
        void UpdateIncomeSource(IncomeSourceEntity inputIncomeSource);
        void UpdateIncomeSource(IncomeSource inputIncomeSource);
        bool DeleteIncomeSource(int incomeSourceId);
        IncomeSource GetIncomeSource(int incomeSourceId);
        List<IncomeSource> GetIncomeSources();
    }
}
