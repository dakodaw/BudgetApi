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
using System.Threading.Tasks;

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

        public async Task<List<IncomeSource>> GetIncomeTypes()
        {
            var incomeSources = (await _incomeSourceProvider.GetIncomeSources()).ToList();
            return incomeSources.OrderBy(i => i.SourceName).ToList();
        }

        public async Task<List<IncomeLine>> GetIncomeLines(int groupId, DateTime monthYear)
        {
            // TODO: Probably fix this. It may have problems with mixing contexts.
            return (from i in await _incomeProvider.GetIncomes(groupId, monthYear)
                    join it in await _incomeSourceProvider.GetIncomeSources() on i.SourceId equals it.Id
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

        public async Task<List<IncomeSource>> GetIncomeSources()
        {
            return (await _incomeSourceProvider.GetIncomeSources())
                .Where(i => i.ActiveJob == true).ToList();
        }

        public async Task<List<IncomeSource>> GetFullIncomeSources()
        {
            var incomeSourceLines = (from it in (await _incomeSourceProvider.GetIncomeSources())
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

        public async Task<List<ApplicablePurchase>> GetApplicablePurchases(int groupId, DateTime monthYear)
        {
            var applicablePurchases = (from it in await _purchaseProvider.GetPurchasesByMonthYear(monthYear)
                                       join pt in await _budgetProvider.GetBudgetTypes(groupId) on it.PurchaseTypeId equals pt.BudgetTypeId
                                       select new ApplicablePurchase
                                       {
                                           Id = it.Id,
                                           PurchaseType = pt.BudgetTypeName,
                                           Amount = it.Amount
                                       }).ToList();

            return applicablePurchases;
        }

        public async Task<bool> AddUpdateIncome(int groupId, Income inputIncome, int incomeId = -1)
        {
            return await _incomeProvider.AddUpdateIncome(groupId, inputIncome, incomeId);
        }

        public async Task<int> AddIncome(int groupId, Income inputIncome)
        {
            return await _incomeProvider.AddIncome(groupId, inputIncome);
        }

        public async Task<bool> UpdateIncome(Income inputIncome)
        {
            return await _incomeProvider.UpdateIncome(inputIncome);
        }

        public async Task<bool> DeleteIncomeEntry(int incomeId)
        {
            return await _incomeProvider.DeleteIncomeEntry(incomeId);
        }

        public async Task<bool> AddUpdateJob(IncomeSource inputJob, int incomeSourceId = -1)
        {
            return await _incomeSourceProvider.AddUpdateJob(inputJob, incomeSourceId);
        }

        public async Task<bool> DeleteJobEntry(int incomeSourceId)
        {
            try
            {
                await _incomeSourceProvider.DeleteIncomeSource(incomeSourceId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IncomeSource> GetIncomeSource(int incomeSourceId)
        {
            var incomeSources = (await _incomeSourceProvider.GetIncomeSources()).ToList();
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

        public async Task<IncomeLine> GetExistingIncome(int incomeId)
        {
            var income = await _incomeProvider.GetExistingIncome(incomeId);

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
