using Budget.Models;
using BudgetApi.Incomes.Models;
using BudgetApi.Purchases.Models;
using System;
using System.Collections.Generic;

namespace BudgetApi.Incomes.Services
{
    public interface IIncomeService
    {
        List<IncomeSource> GetIncomeTypes();
        List<IncomeLine> GetIncomeLines(int groupId, DateTime monthYear);
        List<IncomeSource> GetIncomeSources();
        List<IncomeSource> GetFullIncomeSources();
        List<ApplicablePurchase> GetApplicablePurchases(int groupId, DateTime monthYear);
        bool AddUpdateIncome(int groupId, Income inputIncome, int incomeId = -1);
        bool DeleteIncomeEntry(int incomeId);
        bool AddUpdateJob(IncomeSource inputJob, int incomeSourceId = -1);
        bool DeleteJobEntry(int incomeSourceId);
        IncomeSource GetIncomeSource(int incomeSourceId);
        IncomeLine GetExistingIncome(int incomeId);
        int AddIncome(int groupId, Income inputIncome);
        bool UpdateIncome(Income inputIncome);
    }
}
