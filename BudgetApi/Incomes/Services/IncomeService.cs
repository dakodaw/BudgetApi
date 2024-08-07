using Budget.DB;
using Budget.DB.Budget;
using Budget.DB.Incomes;
using Budget.Models;
using BudgetApi.Incomes.Models;
using BudgetApi.Models;
using BudgetApi.Purchases.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public List<IncomeSource> GetIncomeTypes()
        {
            var incomeSources = _incomeSourceProvider.GetIncomeSources().ToList();
            return incomeSources.OrderBy(i => i.SourceName).ToList();
        }

        public List<IncomeLine> GetIncomeLines(DateTime monthYear)
        {
            // TODO: Probably fix this. It may have problems with mixing contexts.
            return (from i in _incomeProvider.GetIncomes(monthYear)
                    join it in _incomeSourceProvider.GetIncomeSources() on i.SourceId equals it.Id
                    select new IncomeLine
                    {
                        IncomeId = i.Id,
                        IncomeSource = it,
                        IncomeDate = i.Date,
                        Details = i.SourceDetails,
                        Amount = i.Amount,
                        IsReimbursement = i.IsReimbursement,
                        IsCash = i.IsCash,
                        PurchaseId = i.PurchaseId
                    }).ToList();
        }

        public List<IncomeSource> GetIncomeSources()
        {
            return _incomeSourceProvider.GetIncomeSources()
                .Where(i => i.ActiveJob == true).ToList();
        }

        public List<IncomeSource> GetFullIncomeSources()
        {
            var incomeSourceLines = (from it in _incomeSourceProvider.GetIncomeSources()
                                        .Where(i => i.ActiveJob == true)
                                     select new IncomeSource
                                     {
                                         Id = it.Id,
                                         SourceName = it.SourceName,
                                         JobOf = it.JobOf,
                                         ActiveJob = it.ActiveJob,
                                         PositionName = it.PositionName,
                                         PayFrequency = it.PayFrequency,
                                         EstimatedIncome = it.EstimatedIncome.Value
                                     }).ToList();

            return incomeSourceLines;
        }

        public List<ApplicablePurchase> GetApplicablePurchases(DateTime monthYear)
        {
            var applicablePurchases = (from it in _purchaseProvider.GetPurchasesByMonthYear(monthYear)
                                       join pt in _budgetProvider.GetBudgetTypes() on it.PurchaseTypeId equals pt.BudgetTypeId
                                       select new ApplicablePurchase
                                       {
                                           Id = it.Id,
                                           PurchaseType = pt.BudgetTypeName,
                                           Amount = it.Amount
                                       }).ToList();
            return applicablePurchases;
        }

        public bool AddUpdateIncome(Income inputIncome, int incomeId = -1)
        {
            return _incomeProvider.AddUpdateIncome(inputIncome, incomeId);
        }

        public int AddIncome(Income inputIncome)
        {
            return _incomeProvider.AddIncome(inputIncome);
        }

        public bool UpdateIncome(Income inputIncome)
        {
            return _incomeProvider.UpdateIncome(inputIncome);
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
            try
            {
                _incomeSourceProvider.DeleteIncomeSource(incomeSourceId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IncomeSource GetIncomeSource(int incomeSourceId)
        {
            var incomeSources = _incomeSourceProvider.GetIncomeSources().ToList();
            var incomeToReturn = (from ins in incomeSources
                                  where ins.Id == incomeSourceId
                                  select new IncomeSource
                                  {
                                      SourceName = ins.SourceName,
                                      ActiveJob = ins.ActiveJob,
                                      JobOf = ins.JobOf,
                                      PayFrequency = ins.PayFrequency,
                                      PositionName = ins.PositionName
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
