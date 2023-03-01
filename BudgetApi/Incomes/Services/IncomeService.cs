using Budget.DB;
using Budget.DB.Budget;
using Budget.DB.Incomes;
using Budget.Models;
using BudgetApi.Incomes.Models;
using BudgetApi.Models;
using BudgetApi.Purchases.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetApi.Incomes.Services
{
    public class IncomeService: IIncomeService
    {
        IIncomeProvider _incomeProvider;
        IIncomeSourceProvider _incomeSourceProvider;
        IPurchaseProvider _purchaseProvider;
        IBudgetProvider _budgetProvider;

        public IncomeService(
            IIncomeProvider incomeProvider,
            IIncomeSourceProvider incomeSourceProvider,
            IPurchaseProvider purchaseProvider,
            IBudgetProvider budgetProvider)
        {
            _incomeProvider = incomeProvider;
            _incomeSourceProvider = incomeSourceProvider;
            _purchaseProvider = purchaseProvider;
            _budgetProvider = budgetProvider;
        }

        public List<IncomeSourceLine> GetIncomeTypes()
        {
            var incomeSources = _incomeSourceProvider.GetIncomeSources()
                .Select(it => new IncomeSourceLine
                {
                    IncomeSourceId = it.Id,
                    IncomeSource = it.SourceName,
                    Position = it.PositionName,
                    JobOf = it.JobOf,
                    IsCurrentJob = it.ActiveJob
                }).ToList();
            return incomeSources.OrderBy(i => i.IncomeSource).ToList();
        }

        public List<IncomeLine> GetIncomeLines(DateTime monthYear)
        {
            // TODO: Probably fix this. It may have problems with mixing contexts.
            return (from i in _incomeProvider.GetIncomes(monthYear)
                    join it in _incomeSourceProvider.GetIncomeSources() on i.SourceId equals it.Id
                    select new IncomeLine
                    {
                        IncomeId = i.Id,
                        IncomeSource = new IncomeSourceLine
                        {
                            IncomeSourceId = it.Id,
                            IncomeSource = it.SourceName,
                            JobOf = it.JobOf,
                            IsCurrentJob = it.ActiveJob,
                            Position = it.PositionName
                        },
                        IncomeDate = i.Date,
                        Details = i.SourceDetails,
                        Amount = i.Amount,
                        IsReimbursement = i.IsReimbursement,
                        IsCash = i.IsCash,
                        PurchaseId = i.PurchaseId
                    }).ToList();
        }

        public List<IncomeSourceLine> GetIncomeSources()
        {
            return _incomeSourceProvider.GetIncomeSources()
                .Where(i => i.ActiveJob == true).Select(it => 
                    new IncomeSourceLine
                    {
                        IncomeSourceId = it.Id,
                        IncomeSource = it.SourceName,
                        JobOf = it.JobOf,
                        IsCurrentJob = it.ActiveJob,
                        Position = it.PositionName
                    }).ToList();
        }

        public List<IncomeSources> GetFullIncomeSources()
        {
            var incomeSourceLines = (from it in _incomeSourceProvider.GetIncomeSources()
                                        .Where(i => i.ActiveJob == true)
                                     select new IncomeSources
                                     {
                                         IncomeSourceId = it.Id,
                                         IncomeSource = it.SourceName,
                                         JobOf = it.JobOf,
                                         IsCurrentJob = it.ActiveJob,
                                         Position = it.PositionName,
                                         PayFrequency = it.PayFrequency,
                                         EstimatedIncome = it.EstimatedIncome.Value
                                     }).ToList();

            return incomeSourceLines;
        }

        public List<ApplicablePurchase> GetApplicablePurchases(DateTime monthYear)
        {
            var applicablePurchases = (from it in _purchaseProvider.GetPurchasesByMonthYear(monthYear)
                                       join pt in _budgetProvider.GetBudgetTypes() on it.PurchaseTypeId equals pt.Id
                                       select new ApplicablePurchase
                                       {
                                           Id = it.Id,
                                           PurchaseType = pt.BudgetType1,
                                           Amount = it.Amount
                                       }).ToList();
            return applicablePurchases;
        }

        public bool AddUpdateIncome(Income inputIncome, int incomeId = -1)
        {
            return _incomeProvider.AddUpdateIncome(inputIncome, incomeId);
        }

        public bool DeleteIncomeEntry(int incomeId)
        {
            return _incomeProvider.DeleteIncomeEntry(incomeId);
        }

        public bool AddUpdateJob(IncomeSource inputJob, int incomeSourceId = -1)
        {
            return _incomeSourceProvider.AddUpdateJob(inputJob, incomeSourceId);
        }

        public bool DeleteJobEntry(int incomeSourceId)
        {
            return _incomeSourceProvider.DeleteIncomeSource(incomeSourceId);
        }

        public IncomeSources GetIncomeSource(int incomeSourceId)
        {
            var incomeSources = _incomeSourceProvider.GetIncomeSources().ToList();
            var incomeToReturn = (from ins in incomeSources
                                  where ins.Id == incomeSourceId
                                  select new IncomeSources
                                  {
                                      IncomeSource = ins.SourceName,
                                      IsCurrentJob = ins.ActiveJob,
                                      JobOf = ins.JobOf,
                                      PayFrequency = ins.PayFrequency,
                                      Position = ins.PositionName
                                  }).FirstOrDefault();
            if (incomeSources.Find(x => x.Id == incomeSourceId).EstimatedIncome != null)
                incomeToReturn.EstimatedIncome = (decimal)incomeSources
                    .Find(x => x.Id == incomeSourceId).EstimatedIncome;

            return incomeToReturn;
        }

        public IncomeLine GetExistingIncome(int incomeId)
        {
            var income = _incomeProvider.GetExistingIncome(incomeId);

            return new IncomeLine
            {
                IncomeId = income.Id,
                Amount = income.Amount,
                Details = income.SourceDetails,
                IncomeDate = income.Date,
                IncomeSourceId = income.SourceId,
                IsCash = income.IsCash,
                IsReimbursement = income.IsReimbursement,
            };
        }
    }
}
