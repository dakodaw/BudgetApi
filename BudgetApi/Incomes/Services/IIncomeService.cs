using Budget.Models;
using BudgetApi.Incomes.Models;
using BudgetApi.Purchases.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BudgetApi.Incomes.Services
{
    public interface IIncomeService
    {
        Task<List<IncomeSource>> GetIncomeTypes();
        Task<List<IncomeLine>> GetIncomeLines(int groupId, DateTime monthYear);
        Task<List<IncomeSource>> GetIncomeSources();
        Task<List<IncomeSource>> GetFullIncomeSources();
        Task<List<ApplicablePurchase>> GetApplicablePurchases(int groupId, DateTime monthYear);
        Task<bool> AddUpdateIncome(int groupId, Income inputIncome, int incomeId = -1);
        Task<bool> DeleteIncomeEntry(int incomeId);
        Task<bool> AddUpdateJob(IncomeSource inputJob, int incomeSourceId = -1);
        Task<bool> DeleteJobEntry(int incomeSourceId);
        Task<IncomeSource> GetIncomeSource(int incomeSourceId);
        Task<IncomeLine> GetExistingIncome(int incomeId);
        Task<int> AddIncome(int groupId, Income inputIncome);
        Task<bool> UpdateIncome(Income inputIncome);
    }
}
