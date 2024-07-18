using Budget.Models;
using BudgetApi.Incomes.Models;
using BudgetApi.Purchases.Models;
using System;
using System.Collections.Generic;

namespace BudgetApi.Incomes.Services
{
    public interface IIncomeService
    {
        List<IncomeSourceLine> GetIncomeTypes();
        List<IncomeLine> GetIncomeLines(DateTime monthYear);
        List<IncomeSourceLine> GetIncomeSources();
        List<IncomeSource> GetFullIncomeSources();
        List<ApplicablePurchase> GetApplicablePurchases(DateTime monthYear);
        bool AddUpdateIncome(Income inputIncome, int incomeId = -1);
        bool DeleteIncomeEntry(int incomeId);
        bool AddUpdateJob(IncomeSourceEntity inputJob, int incomeSourceId = -1);
        bool DeleteJobEntry(int incomeSourceId);
        IncomeSource GetIncomeSource(int incomeSourceId);
        IncomeLine GetExistingIncome(int incomeId);
        bool UpdateIncome(Income inputIncome, int incomeId);
        int AddIncome(Income inputIncome);
    }
}
