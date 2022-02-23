using BudgetApi.Budgeting.Models;
using BudgetApi.BudgetTypes;
using BudgetApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetApi.Budgeting.Services
{
    public class BudgetService: IBudgetService
    {
        BudgetEntities _db;

        public BudgetService(BudgetEntities db)
        {
            _db = db;
        }

        public List<BudgetWithPurchaseInfo> GetBudgetLines(DateTime monthYear)
        {
            decimal totalBudgeted = (decimal)0;
            decimal totalSpent = (decimal)0;
            var budgetPurchases = _db.Purchases.Where(i => i.Date.Month == monthYear.Month && i.Date.Year == monthYear.Year).ToList();
            var budgetLines = (from b in _db.Budgets.Where(i => i.Date.Month == monthYear.Month && i.Date.Year == monthYear.Date.Year)
                               join bt in _db.BudgetTypes on b.BudgetTypeId equals bt.Id
                               select new BudgetWithPurchaseInfo
                               {
                                   BudgetLineId = b.Id,
                                   BudgetType = new BudgetType
                                   {
                                       BudgetTypeId = bt.Id,
                                       BudgetTypeName = bt.BudgetType1
                                   },
                                   BudgetDate = b.Date,
                                   Amount = b.Amount
                               }).ToList();
            totalBudgeted = budgetLines.Sum(i => i.Amount);

            //Get the income that is marked as a reimbursement for the month/year
            var applicableincome = _db.Incomes.Where(i => i.Date.Month == monthYear.Month && i.Date.Year == monthYear.Year).ToList();
            //go through all budgetlines and find the matching purchase amount
            foreach (var budget in budgetLines)
            {
                var applicablePurchases = budgetPurchases.Where(i => i.PurchaseTypeId == budget.BudgetType.BudgetTypeId);
                decimal reimbursement = (decimal)0;

                if (applicableincome != null)
                {
                    //Find any income that is a reimbursement and has a matching purchase id from this month/year
                    //var incomeToReimburse = applicableincome.Where(i => budgetPurchases.Any(j => j.Id == i.PurchaseId)).ToList();
                    foreach (var myI in applicableincome)
                    {
                        var purchasesToReimburse = applicablePurchases.Where(i => i.Id == myI.PurchaseId);
                        if (purchasesToReimburse.Count() > 0)
                        {
                            reimbursement += myI.Amount;
                        }
                    }
                    budget.PurchaseAmount = (budgetPurchases.Where(i => i.PurchaseTypeId == budget.BudgetType.BudgetTypeId && i.PaymentType == "Normal").Sum(i => i.Amount) - reimbursement);
                    totalSpent += budget.PurchaseAmount;
                }
                else
                {
                    budget.PurchaseAmount = budgetPurchases.Where(i => i.PurchaseTypeId == budget.BudgetType.BudgetTypeId && i.PaymentType == "Normal").Sum(i => i.Amount);
                    totalSpent += budget.PurchaseAmount;
                }
            }
            List<BudgetWithPurchaseInfo> unBudgetedPurchases = new List<BudgetWithPurchaseInfo>();
            foreach (var p in budgetPurchases)
            {
                var budge = budgetLines.Where(i => i.BudgetType.BudgetTypeId == p.PurchaseTypeId).ToList();
                if (budge.Count() == 0)
                {
                    unBudgetedPurchases.Add(new BudgetWithPurchaseInfo
                    {
                        BudgetType = new BudgetType
                        {
                            BudgetTypeName = _db.BudgetTypes.Where(i => i.Id == p.PurchaseTypeId).FirstOrDefault().BudgetType1
                        },
                        Amount = 0,
                        PurchaseAmount = p.Amount
                    });
                }
            }
            var test = unBudgetedPurchases.GroupBy(i => i.BudgetType.BudgetTypeName).ToList();
            foreach (var t in test)
            {
                budgetLines.Add(new BudgetWithPurchaseInfo
                {
                    BudgetType = new BudgetType
                    {
                        BudgetTypeName = t.FirstOrDefault().BudgetType.BudgetTypeName
                    },
                    Amount = 0,
                    PurchaseAmount = t.Sum(i => i.PurchaseAmount)
                });
                totalSpent += t.Sum(i => i.PurchaseAmount);
            }
            budgetLines = budgetLines.OrderBy(i => i.BudgetType.BudgetTypeName).ToList();
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

        public bool AddBudget(Budget inputBudget)
        {
            bool success = false;
            _db.Budgets.Add(inputBudget);
            _db.SaveChanges();
            try
            {
                var checkBudget = _db.Budgets.Where(i => i.Amount == inputBudget.Amount && i.Date == inputBudget.Date).FirstOrDefault();
                success = true;
            }
            catch
            {

            }
            return success;
        }

        public bool AddBudgetLines(IEnumerable<Budget> inputBudgetLines)
        {
            bool success = false;
            try
            {
                _db.Budgets.AddRange(inputBudgetLines);
                _db.SaveChanges();

                success = true;
            }
            catch
            {

            }
            return success;
        }

        public bool UpdateBudget(Budget inputBudget, int budgetId)
        {
            bool success = false;
            //Get the Budget from the Database with a given id
            //Update the Budget that matches the one from the database
            var selectedBudgetEntry = _db.Budgets.Where(i => i.Id == budgetId).FirstOrDefault();
            _db.Budgets.Where(i => i.Id == budgetId).FirstOrDefault().Amount = inputBudget.Amount;
            _db.Budgets.Where(i => i.Id == budgetId).FirstOrDefault().BudgetTypeId = inputBudget.BudgetTypeId;
            _db.Budgets.Where(i => i.Id == budgetId).FirstOrDefault().BudgetType = inputBudget.BudgetType;
            _db.Budgets.Where(i => i.Id == budgetId).FirstOrDefault().Date = inputBudget.Date;

            //// Alternate approach
            //_db.Entry(selectedBudgetEntry).CurrentValues.SetValues(new
            //{
            //    Amount = inputBudget.Amount,
            //    BudgetTypeId = selectedBudgetEntry.BudgetTypeId,
            //    BudgetType = selectedBudgetEntry?.BudgetType,
            //    Date = inputBudget.Date
            //});

            //Save Changes
            _db.SaveChanges();
            try
            {
                var checkBudget = _db.Budgets.Where(i => i.Amount == inputBudget.Amount && i.Date == inputBudget.Date).FirstOrDefault();
                success = true;
            }
            catch
            {

            }
            return success;
        }

        public bool DeleteBudgetEntry(int budgetId)
        {
            bool success = true;
            try
            {
                var toDelete = _db.Budgets.Find(budgetId);
                _db.Budgets.Remove(toDelete);
                _db.SaveChanges();
            }
            catch
            {

            }
            return success;
        }

        public BudgetInfo GetExistingBudget(int budgetId)
        {
            var existingPurchase = (from b in _db.Budgets.Where(i => i.Id == budgetId)
                                    select new BudgetInfo
                                    {
                                        Amount = b.Amount,
                                        BudgetDate = b.Date,
                                        BudgetType = new BudgetType
                                        {
                                            BudgetTypeId = b.BudgetType.Id,
                                            BudgetTypeName = b.BudgetType.BudgetType1
                                        }
                                    }).FirstOrDefault();
            existingPurchase.BudgetMonthYear = existingPurchase.BudgetDate.ToString("yyyy-MM");
            return existingPurchase;
        }

        public decimal ScenarioCheck(ScenarioInput scenarioInput)
        {
            var applicableBudget = _db.Budgets
                .Where(i => 
                    i.Date >= scenarioInput.startMonth &&
                    i.Date <= scenarioInput.endMonth)
                .ToList();

            decimal amountPlannedToSpend = default;
            foreach (var budgetItem in applicableBudget)
            {
                amountPlannedToSpend += budgetItem.Amount;
            }
            
            var income = _db.IncomeSources.Where(i => i.ActiveJob == true && i.EstimatedIncome != null).ToList();
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
