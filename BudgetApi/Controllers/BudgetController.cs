using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BudgetTest.Models;

namespace BudgetTest.Controllers
{
    public class BudgetController : ApiController
    {
        BudgetTestEntities _db = new BudgetTestEntities();
        // GET: Budget
        [HttpGet]
        public List<BudgetTypes> GetBudgetTypes([FromUri] bool getBudgetTypes)
        {
            var budgetTypes = (from bt in _db.BudgetTypes
                               select new BudgetTypes
                               {
                                   BudgetTypeId = bt.Id,
                                   BudgetTypeName = bt.BudgetType1
                               }).ToList();
            return budgetTypes.OrderBy(i=>i.BudgetTypeName).ToList();
        }

        [HttpGet]
        public List<BudgetWithPurchaseInfo> GetBudgetLines([FromUri] bool getBudgetLines, [FromUri] DateTime monthYear)
        {
            decimal totalBudgeted = (decimal)0;
            decimal totalSpent = (decimal)0;
            var budgetPurchases = _db.Purchases.Where(i => i.Date.Month == monthYear.Month && i.Date.Year == monthYear.Year).ToList();
            var budgetLines = (from b in _db.Budgets.Where(i=>i.Date.Month == monthYear.Month && i.Date.Year == monthYear.Date.Year)
                               join bt in _db.BudgetTypes on b.BudgetTypeId equals bt.Id
                               select new BudgetWithPurchaseInfo
                               {
                                   BudgetLineId = b.Id,
                                   BudgetType = new BudgetTypes
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
                    foreach(var myI in applicableincome)
                    {
                        var purchasesToReimburse = applicablePurchases.Where(i => i.Id == myI.PurchaseId);
                        if(purchasesToReimburse.Count() >0)
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
            foreach(var p in budgetPurchases)
            {
                var budge = budgetLines.Where(i => i.BudgetType.BudgetTypeId == p.PurchaseTypeId).ToList();
                if(budge.Count() == 0)
                {
                    unBudgetedPurchases.Add(new BudgetWithPurchaseInfo
                    {
                        BudgetType = new BudgetTypes
                        {
                            BudgetTypeName = _db.BudgetTypes.Where(i => i.Id == p.PurchaseTypeId).FirstOrDefault().BudgetType1
                        },
                        Amount = 0,
                        PurchaseAmount = p.Amount
                    });
                }
            }
            var test = unBudgetedPurchases.GroupBy(i => i.BudgetType.BudgetTypeName).ToList();
            foreach(var t in test)
            {
                budgetLines.Add(new BudgetWithPurchaseInfo
                {
                    BudgetType = new BudgetTypes
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
                BudgetType = new BudgetTypes
                {
                    BudgetTypeName = BudgetTypeStatics.Totals
                },
                Amount = totalBudgeted,
                PurchaseAmount = totalSpent
            });
            return budgetLines;
        }

        [HttpPost]
        public bool AddUpdateBudget([FromUri] bool addUpdateBudget, [FromBody] Budget inputBudget, [FromUri] int budgetId = -1)
        {
            if (budgetId == -1)
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
            else
            {
                bool success = false;
                //Get the Budget from the Database with a given id
                //Update the Budget that matches the one from the database
                var selectedBudgetEntry = _db.Budgets.Where(i => i.Id == budgetId).FirstOrDefault();
                _db.Budgets.Where(i => i.Id == budgetId).FirstOrDefault().Amount = inputBudget.Amount;
                _db.Budgets.Where(i => i.Id == budgetId).FirstOrDefault().BudgetTypeId = inputBudget.BudgetTypeId;
                _db.Budgets.Where(i => i.Id == budgetId).FirstOrDefault().BudgetType = inputBudget.BudgetType;
                _db.Budgets.Where(i => i.Id == budgetId).FirstOrDefault().Date = inputBudget.Date;
                
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
        }

        [HttpGet]
        public bool DeleteBudgetEntry([FromUri] bool deleteBudgetEntry, [FromUri] int budgetId)
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

        [HttpGet]
        public BudgetInfo GetExistingBudget([FromUri] bool getExistingBudget, [FromUri] int budgetId)
        {
            var existingPurchase = (from b in _db.Budgets.Where(i => i.Id == budgetId) 
                                    select new BudgetInfo
                                    {
                                        Amount = b.Amount,
                                        BudgetDate = b.Date,
                                        BudgetType = new BudgetTypes
                                        {
                                            BudgetTypeId = b.BudgetType.Id,
                                            BudgetTypeName = b.BudgetType.BudgetType1
                                        }
                                    }).FirstOrDefault();
            existingPurchase.BudgetMonthYear = existingPurchase.BudgetDate.ToString("yyyy-MM");
            return existingPurchase;
        }

        [HttpPost]
        public double ScenarioCheck([FromUri] bool scenarioCheck, [FromBody] ScenarioInput scenarioInput)
        {
            var applicableBudget = _db.Budgets.Where(i => i.Date >= scenarioInput.startMonth && i.Date <= scenarioInput.endMonth).ToList();
            double amountPlannedToSpend = 0.00;
            foreach(var budg in applicableBudget)
            {
                amountPlannedToSpend += (double)budg.Amount;
            }
            var income = _db.IncomeSources.Where(i => i.ActiveJob == true && i.EstimatedIncome != null).ToList();
            double amountPlannedToEarn = 0.00;
            foreach(var inc in income)
            {
                int multiplyNumber = 1;
                //Check if the income is biweekly or monthly
                //  If the income is monthly, multiply the estimated income by the number of months
                if(inc.PayFrequency.Contains(PaymentFrequency.Monthly))
                {
                    multiplyNumber = Convert.ToInt32((scenarioInput.endMonth.AddMonths(1) - scenarioInput.startMonth).TotalDays / 30);
                }
                //  If the income is biweekly, multiply the estimated income by the number of weeks divided by two
                else if(inc.PayFrequency.Contains(PaymentFrequency.Biweekly))
                {
                    multiplyNumber = Convert.ToInt32((scenarioInput.endMonth.AddMonths(1) - scenarioInput.startMonth).TotalDays / 14);
                }
                else if (inc.PayFrequency.Contains(PaymentFrequency.TwiceAMonth))
                {
                    multiplyNumber = Convert.ToInt32((scenarioInput.endMonth.AddMonths(1) - scenarioInput.startMonth));
                }
                amountPlannedToEarn += (double)(inc.EstimatedIncome * multiplyNumber);
            }

            return (scenarioInput.initialAmount + amountPlannedToEarn - amountPlannedToSpend);
        }

        [HttpPost]
        public bool AddUpdateBudgetType([FromUri] bool addUpdateBudgetType, [FromBody] BudgetType budgetType, [FromUri] int budgetTypeId = -1)
        {
            if (budgetTypeId == -1)
            {
                try
                {
                    _db.BudgetTypes.Add(new BudgetType
                    {
                        BudgetType1 = budgetType.BudgetType1
                    });
                    _db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                try
                {
                    _db.BudgetTypes.Find(budgetTypeId).BudgetType1 = budgetType.BudgetType1;
                    _db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        [HttpGet]
        public bool DeleteBudgetTypeEntry([FromUri] bool deleteBudgetTypeEntry, [FromUri] int budgetTypeId)
        {
            try
            {
                var toDelete = _db.BudgetTypes.Find(budgetTypeId);
                _db.BudgetTypes.Remove(toDelete);
                _db.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        [HttpGet]
        public BudgetTypes GetBudgetType([FromUri] bool getBudgetType, [FromUri] int budgetTypeId)
        {
            return (from b in _db.BudgetTypes
                    where b.Id == budgetTypeId
                    select new BudgetTypes
                    {
                        BudgetTypeId = b.Id,
                        BudgetTypeName = b.BudgetType1
                    }).FirstOrDefault();
        }
    }
}