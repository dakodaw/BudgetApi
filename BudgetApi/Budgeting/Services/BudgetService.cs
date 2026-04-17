using Budget.DB;
using Budget.DB.Budget;
using Budget.DB.Incomes;
using BudgetApi.Budgeting.Models;
using BudgetApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetApi.Budgeting.Services
{
    public class BudgetService: IBudgetService
    {
        IBudgetProvider _budgetProvider;
        IPurchaseProvider _purchaseProvider;
        IIncomeProvider _incomeProvider;
        IIncomeSourceProvider _incomeSourceProvider;

        public BudgetService(
            IBudgetProvider budgetProvider,
            IPurchaseProvider purchaseProvider,
            IIncomeProvider incomeProvider,
            IIncomeSourceProvider incomeSourceProvider)
        {
            _budgetProvider = budgetProvider;
            _purchaseProvider = purchaseProvider;
            _incomeProvider = incomeProvider;
            _incomeSourceProvider = incomeSourceProvider;
        }

        public async Task<List<BudgetWithPurchaseInfo>> GetBudgetLines(int groupId, DateTime monthYear)
        {
            var budgetLinesToReturn = new List<BudgetWithPurchaseInfo>();

            var budgetTypes = await _budgetProvider.GetBudgetTypes(groupId);
            var budgetingEntries = await _budgetProvider.GetBudgetEntries(groupId, monthYear);
            var budgetEntries = await _purchaseProvider.GetPurchasesByMonthYear(monthYear);
            var budgetPurchases = budgetEntries.GroupBy(x => x.PurchaseTypeId);

            foreach(var budgetEntry in budgetingEntries)
            {
                // Get the group that matches the budgetEntry
                var groupPurchasesSum = budgetPurchases
                    .FirstOrDefault(x => x.Key == budgetEntry.BudgetTypeId)?
                    .Sum(purchase => purchase.Amount) ?? 0;

                budgetLinesToReturn.Add(new BudgetWithPurchaseInfo
                {
                    Amount = budgetEntry.Amount,
                    PurchaseAmount = groupPurchasesSum,
                    BudgetDate = budgetEntry.Date,
                    BudgetMonthYear = budgetEntry.Date.ToString("yyyy-MM"),
                    BudgetType = new BudgetType
                    {
                        BudgetTypeId = budgetEntry?.BudgetTypeId ?? 0,
                        BudgetTypeName = budgetTypes.FirstOrDefault(x => x.BudgetTypeId == budgetEntry.BudgetTypeId).BudgetTypeName
                    },
                    BudgetLineId = budgetEntry.Id
                });
            }

            var unbudgetedPurchases = budgetPurchases
                .Where(purchase => !budgetingEntries
                                        .Select(x => x.BudgetTypeId)
                                        .ToList()
                                        .Contains(purchase.Key));

            foreach(var unbudgetedPurchaseGroup in unbudgetedPurchases)
            {
                var unbudgetedPurchaseAmount = unbudgetedPurchaseGroup
                    .Sum(purchase => purchase.Amount);

                var purchase = unbudgetedPurchaseGroup.First();
                budgetLinesToReturn.Add(new BudgetWithPurchaseInfo
                {
                    Amount = 0,
                    PurchaseAmount = unbudgetedPurchaseAmount,
                    BudgetDate = purchase.Date,
                    BudgetMonthYear = purchase.Date.ToString("yyyy-MM"),
                    BudgetType = new BudgetType
                    {
                        BudgetTypeId = purchase.PurchaseTypeId,
                        BudgetTypeName = budgetTypes.FirstOrDefault(x => x.BudgetTypeId == purchase.PurchaseTypeId).BudgetTypeName
                    },
                    BudgetLineId = purchase.Id
                });
            }

            return budgetLinesToReturn;
        }

        public async Task<int> AddBudget(int groupId, BudgetEntry inputBudget)
        {
            return await _budgetProvider.AddBudget(groupId, inputBudget);
        }

        public async Task<bool> AddBudgetLines(int groupId, IEnumerable<BudgetEntry> inputBudgetLines)
        {
            return await _budgetProvider.AddBudgetEntries(groupId, inputBudgetLines);
        }

        public async Task UpdateBudget(BudgetEntry inputBudget)
        {
            await _budgetProvider.UpdateBudget(inputBudget);
        }

        public async Task DeleteBudgetEntry(int budgetId)
        {
            await _budgetProvider.DeleteBudgetEntry(budgetId);
        }

        public async Task<BudgetInfo> GetExistingBudget(int budgetId)
        {
            var existingBudget = await _budgetProvider.GetBudgetEntry(budgetId);
            return new BudgetInfo
            {
                Amount = existingBudget.Amount,
                BudgetDate = existingBudget.Date,
                BudgetType = new BudgetType
                {
                    BudgetTypeId = existingBudget.BudgetTypeId,
                    BudgetTypeName = existingBudget.BudgetType.BudgetTypeName
                },
                BudgetMonthYear = existingBudget.Date.ToString("yyyy-MM")
            };
        }

        public async Task<decimal> ScenarioCheck(int groupId, ScenarioInput scenarioInput)
        {
            var applicableBudget = (await _budgetProvider
                .GetBudgetEntriesInTimeSpan(groupId, scenarioInput.startMonth, scenarioInput.endMonth))
                .ToList();

            decimal amountPlannedToSpend = default;
            foreach (var budgetItem in applicableBudget)
            {
                amountPlannedToSpend += budgetItem.Amount;
            }
            
            var income = (await _incomeSourceProvider.GetIncomeSources())
                .Where(i => i.EstimatedIncome != null)
                .ToList();

            decimal amountPlannedToEarn = default;
            foreach (var inc in income)
            {
                var payFrequency = inc.PayFrequency.Trim();
                var endOfEndMonth = scenarioInput.endMonth.AddMonths(1);

                //Check if the income is biweekly or monthly
                var multiplyNumber = (decimal)GetMultiplyNumber(inc.PayFrequency, scenarioInput.startMonth, endOfEndMonth);

                if(inc.EstimatedIncome.HasValue)
                    amountPlannedToEarn += inc.EstimatedIncome.Value * multiplyNumber;
            }

            return ((decimal)scenarioInput.initialAmount) + amountPlannedToEarn - amountPlannedToSpend;
        }

        private int GetNumberOfMonths(DateTime startDate, DateTime endDate)
        {
            var monthDifference = endDate.Month - startDate.Month;
            var yearDifference = endDate.Year - startDate.Year;

            // Twice a month should be 2 * number of months
            return yearDifference == 0
                ? monthDifference
                : (yearDifference * 12) + monthDifference;
        }

        private double GetMultiplyNumber(string paymentFrequency, DateTime startMonth, DateTime endMonth)
        {
            return paymentFrequency switch
            {
                PaymentFrequency.Monthly => (endMonth - startMonth).TotalDays / 30,
                PaymentFrequency.Biweekly => (endMonth - startMonth).TotalDays / 14,
                PaymentFrequency.TwiceAMonth => GetNumberOfMonths(startMonth, endMonth) * 2,
                _ => 1
            };
        }
    }
}
