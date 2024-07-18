using Budget.DB;
using Budget.DB.Budget;
using Budget.DB.Incomes;
using BudgetApi.Budgeting.Models;
using BudgetApi.BudgetTypes;
using BudgetApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

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

        public List<BudgetWithPurchaseInfo> GetBudgetLines(DateTime monthYear)
        {
            // Should refactor all of this. It's a lot.
            decimal totalBudgeted = (decimal)0;
            decimal totalSpent = (decimal)0;
            var budgetEntries = _purchaseProvider.GetPurchasesByMonthYear(monthYear);
            var budgetLines = (from b in budgetEntries
                               join bt in _budgetProvider.GetBudgetTypes() on b.PurchaseTypeId equals bt.BudgetTypeId
                               select new BudgetWithPurchaseInfo
                               {
                                   BudgetLineId = b.Id,
                                   BudgetType = new BudgetType
                                   {
                                       BudgetTypeId = bt.BudgetTypeId,
                                       BudgetTypeName = bt.BudgetTypeName
                                   },
                                   BudgetDate = b.Date,
                                   Amount = b.Amount
                               }).ToList();

            totalBudgeted = budgetLines.Sum(i => i.Amount);

            //Get the income that is marked as a reimbursement for the month/year
            var applicableincome = _incomeProvider.GetIncomes(monthYear);
            //go through all budgetlines and find the matching purchase amount
            foreach (var budget in budgetLines)
            {
                var applicablePurchases = budgetEntries
                    .Where(i => i.PurchaseTypeId == budget.BudgetType.BudgetTypeId);

                decimal reimbursement = (decimal)0;

                if (applicableincome != null)
                {
                    //Find any income that is a reimbursement and has a matching purchase id from this month/year
                    //var incomeToReimburse = applicableincome.Where(i => budgetEntries.Any(j => j.Id == i.PurchaseId)).ToList();
                    foreach (var myI in applicableincome)
                    {
                        var purchasesToReimburse = applicablePurchases
                            .Where(i => i.Id == myI.PurchaseId);

                        if (purchasesToReimburse.Count() > 0)
                        {
                            reimbursement += myI.Amount;
                        }
                    }
                    budget.PurchaseAmount = budgetEntries
                        .Where(i => i.PurchaseTypeId == budget.BudgetType.BudgetTypeId && i.PaymentType == "Normal")
                        .Sum(i => i.Amount) - reimbursement;
                    totalSpent += budget.PurchaseAmount;
                }
                else
                {
                    budget.PurchaseAmount = budgetEntries
                        .Where(i => i.PurchaseTypeId == budget.BudgetType.BudgetTypeId && i.PaymentType == "Normal")
                        .Sum(i => i.Amount);
                    totalSpent += budget.PurchaseAmount;
                }
            }

            AddUnbudgetedPurchases(ref totalSpent, budgetEntries, ref budgetLines);

            // Adds a line with a total - more a UI concern I think.
            budgetLines.Add(new BudgetWithPurchaseInfo
            {
                BudgetType = new BudgetType
                {
                    BudgetTypeName = BudgetTypeStatics.Totals
                },
                Amount = totalBudgeted,
                PurchaseAmount = totalSpent
            });
            return budgetLines;
        }

        private void AddUnbudgetedPurchases(ref decimal totalSpent, IEnumerable<Purchase> budgetPurchases, ref List<BudgetWithPurchaseInfo> budgetLines)
        {
            var unBudgetedPurchases = new List<BudgetWithPurchaseInfo>();
            foreach (var p in budgetPurchases)
            {
                var budge = budgetLines.Where(i => i.BudgetType.BudgetTypeId == p.PurchaseTypeId).ToList();
                if (budge.Count() == 0)
                {
                    unBudgetedPurchases.Add(new BudgetWithPurchaseInfo
                    {
                        BudgetType = _budgetProvider.GetBudgetType(p.PurchaseTypeId),
                        Amount = 0,
                        PurchaseAmount = p.Amount
                    });
                }
            }
            var groupedPurchases = unBudgetedPurchases
                .GroupBy(i => i.BudgetType.BudgetTypeName).ToList();

            foreach (var t in groupedPurchases)
            {
                budgetLines.Add(new BudgetWithPurchaseInfo
                {
                    BudgetType = new BudgetType
                    {
                        BudgetTypeId = -1,
                        BudgetTypeName = t.FirstOrDefault().BudgetType.BudgetTypeName
                    },
                    Amount = 0,
                    PurchaseAmount = t.Sum(i => i.PurchaseAmount)
                });
                totalSpent += t.Sum(i => i.PurchaseAmount);
            }
            budgetLines = budgetLines.OrderBy(i => i.BudgetType.BudgetTypeName).ToList();
        }

        public bool AddBudget(BudgetEntry inputBudget)
        {
            return _budgetProvider.AddBudget(inputBudget);
        }

        public bool AddBudgetLines(IEnumerable<BudgetEntry> inputBudgetLines)
        {
            return _budgetProvider.AddBudgetEntries(inputBudgetLines);
        }

        public bool UpdateBudget(BudgetEntry inputBudget)
        {
            return _budgetProvider.UpdateBudget(inputBudget);
        }

        public bool DeleteBudgetEntry(int budgetId)
        {
            return _budgetProvider.DeleteBudgetEntry(budgetId);
        }

        public BudgetInfo GetExistingBudget(int budgetId)
        {
            var existingBudget = _budgetProvider.GetBudgetEntry(budgetId);
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

        public decimal ScenarioCheck(ScenarioInput scenarioInput)
        {
            var applicableBudget = _budgetProvider
                .GetBudgetEntriesInTimeSpan(scenarioInput.startMonth, scenarioInput.endMonth)
                .ToList();

            decimal amountPlannedToSpend = default;
            foreach (var budgetItem in applicableBudget)
            {
                amountPlannedToSpend += budgetItem.Amount;
            }
            
            var income = _incomeSourceProvider.GetIncomeSources()
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
