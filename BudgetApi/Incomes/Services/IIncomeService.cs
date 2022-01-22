using BudgetApi.Models;
using BudgetApi.Shared;
using System;
using System.Collections.Generic;

namespace BudgetApi.Incomes.Services
{
    public interface IIncomeService
    {
        List<IncomeSourceLine> GetIncomeTypes();
        List<IncomeLine> GetIncomeLines(DateTime monthYear);
        List<IncomeSourceLine> GetIncomeSources();
        List<IncomeSources> GetFullIncomeSources();
        List<ApplicablePurchase> GetApplicablePurchases(DateTime monthYear);
        bool AddUpdateIncome(Income inputIncome, int incomeId = -1);
        bool DeleteIncomeEntry(int incomeId);
        bool AddUpdateJob(IncomeSource inputJob, int incomeSourceId = -1);
        bool DeleteJobEntry(int incomeSourceId);
        IncomeSources GetIncomeSource(int incomeSourceId);
        IncomeLine GetExistingIncome(int incomeId);
    }
}
