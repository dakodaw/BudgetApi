using BudgetApi.Incomes.Models;
using BudgetApi.Models;
using BudgetApi.Purchases.Models;
using BudgetApi.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetApi.Incomes.Services
{
    public class IncomeService: IIncomeService
    {
        BudgetEntities _db;
        public IncomeService(BudgetEntities db)
        {
            _db = db;
        }

        public List<IncomeSourceLine> GetIncomeTypes()
        {
            var incomeSources = (from it in _db.IncomeSources
                                 select new IncomeSourceLine
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
            var incomeLines = (from i in _db.Incomes.Where(i => i.Date.Month == monthYear.Month && i.Date.Year == monthYear.Year)
                               join it in _db.IncomeSources on i.SourceId equals it.Id
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
                                   IsCash = i.IsCash
                               }).ToList();
            foreach (var line in incomeLines)
            {
                if (line.IsReimbursement)
                {
                    line.PurchaseId = (int)_db.Incomes.Where(i => i.Id == line.IncomeId).FirstOrDefault().PurchaseId;
                }
            }
            return incomeLines;
        }

        public List<IncomeSourceLine> GetIncomeSources()
        {
            var incomeSourceLines = (from it in _db.IncomeSources.Where(i => i.ActiveJob == true)
                                     select new IncomeSourceLine
                                     {
                                         IncomeSourceId = it.Id,
                                         IncomeSource = it.SourceName,
                                         JobOf = it.JobOf,
                                         IsCurrentJob = it.ActiveJob,
                                         Position = it.PositionName
                                     }).ToList();
            return incomeSourceLines;
        }

        public List<IncomeSources> GetFullIncomeSources()
        {
            var incomeSourceLines = (from it in _db.IncomeSources.Where(i => i.ActiveJob == true)
                                     select new IncomeSources
                                     {
                                         IncomeSourceId = it.Id,
                                         IncomeSource = it.SourceName,
                                         JobOf = it.JobOf,
                                         IsCurrentJob = it.ActiveJob,
                                         Position = it.PositionName,
                                         PayFrequency = it.PayFrequency
                                     }).ToList();
            foreach (var isl in incomeSourceLines)
            {
                if (_db.IncomeSources.Where(i => i.Id == isl.IncomeSourceId).FirstOrDefault().EstimatedIncome != null)
                {
                    isl.EstimatedIncome = (decimal)_db.IncomeSources.Where(i => i.Id == isl.IncomeSourceId).FirstOrDefault().EstimatedIncome;
                }
            }
            return incomeSourceLines;
        }

        public List<ApplicablePurchase> GetApplicablePurchases(DateTime monthYear)
        {
            var applicablePurchases = (from it in _db.Purchases.Where(i => i.Date.Month == monthYear.Month && i.Date.Year == monthYear.Year)
                                       join pt in _db.BudgetTypes on it.PurchaseTypeId equals pt.Id
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
            if (incomeId == -1)
            {
                bool success = false;
                _db.Incomes.Add(inputIncome);
                _db.SaveChanges();
                try
                {
                    var checkIncome = _db.Incomes.Where(i => i.Amount == inputIncome.Amount).FirstOrDefault();
                    if (inputIncome.IsReimbursement == true)
                    {
                        _db.Purchases.Where(i => i.Id == inputIncome.PurchaseId).FirstOrDefault().FutureReimbursement = true;
                        _db.SaveChanges();
                    }
                    success = true;
                }
                catch
                {
                    return success;
                }
                return success;
            }
            else
            {
                bool success = false;
                _db.Incomes.Where(i => i.Id == incomeId).FirstOrDefault().IncomeSource = inputIncome.IncomeSource;
                _db.Incomes.Where(i => i.Id == incomeId).FirstOrDefault().IsCash = inputIncome.IsCash;
                _db.Incomes.Where(i => i.Id == incomeId).FirstOrDefault().IsReimbursement = inputIncome.IsReimbursement;
                _db.Incomes.Where(i => i.Id == incomeId).FirstOrDefault().SourceDetails = inputIncome.SourceDetails;
                _db.Incomes.Where(i => i.Id == incomeId).FirstOrDefault().SourceId = inputIncome.SourceId;
                _db.Incomes.Where(i => i.Id == incomeId).FirstOrDefault().Amount = inputIncome.Amount;
                _db.SaveChanges();
                try
                {
                    var checkIncome = _db.Incomes.Where(i => i.Id == incomeId).FirstOrDefault();
                    if (inputIncome.IsReimbursement == true)
                    {
                        _db.Purchases.Where(i => i.Id == inputIncome.PurchaseId).FirstOrDefault().FutureReimbursement = true;
                        _db.SaveChanges();
                    }
                    success = true;
                }
                catch
                {

                }
                return success;
            }
        }

        public bool DeleteIncomeEntry(int incomeId)
        {
            try
            {
                var toDelete = _db.Incomes.Find(incomeId);
                _db.Incomes.Remove(toDelete);
                _db.SaveChanges();
                return true;
            }
            catch
            {

            }
            return false;
        }

        public bool AddUpdateJob(IncomeSource inputJob, int incomeSourceId = -1)
        {
            if (incomeSourceId == -1)
            {
                var jobToAddUpdate = new IncomeSource
                {
                    ActiveJob = true,
                    EstimatedIncome = inputJob.EstimatedIncome,
                    JobOf = inputJob.JobOf,
                    PayFrequency = inputJob.PayFrequency,
                    PositionName = inputJob.PositionName,
                    SourceName = inputJob.SourceName
                };
                try
                {
                    _db.IncomeSources.Add(inputJob);
                    _db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            else
            {
                var jobToAddUpdate = new IncomeSource
                {
                    ActiveJob = true,
                    EstimatedIncome = inputJob.EstimatedIncome,
                    JobOf = inputJob.JobOf,
                    PayFrequency = inputJob.PayFrequency,
                    PositionName = inputJob.PositionName,
                    SourceName = inputJob.SourceName
                };
                try
                {
                    _db.IncomeSources.Find(incomeSourceId).ActiveJob = jobToAddUpdate.ActiveJob;
                    _db.IncomeSources.Find(incomeSourceId).EstimatedIncome = jobToAddUpdate.EstimatedIncome;
                    _db.IncomeSources.Find(incomeSourceId).JobOf = jobToAddUpdate.JobOf;
                    _db.IncomeSources.Find(incomeSourceId).PayFrequency = jobToAddUpdate.PayFrequency;
                    _db.IncomeSources.Find(incomeSourceId).PositionName = jobToAddUpdate.PositionName;
                    _db.IncomeSources.Find(incomeSourceId).SourceName = jobToAddUpdate.SourceName;
                    _db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        public bool DeleteJobEntry(int incomeSourceId)
        {
            try
            {
                var toDelete = _db.IncomeSources.Where(i => i.Id == incomeSourceId).FirstOrDefault();
                _db.IncomeSources.Remove(toDelete);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IncomeSources GetIncomeSource(int incomeSourceId)
        {
            var incomeToReturn = (from ins in _db.IncomeSources
                                  where ins.Id == incomeSourceId
                                  select new IncomeSources
                                  {
                                      IncomeSource = ins.SourceName,
                                      IsCurrentJob = ins.ActiveJob,
                                      JobOf = ins.JobOf,
                                      PayFrequency = ins.PayFrequency,
                                      Position = ins.PositionName
                                  }).FirstOrDefault();
            if (_db.IncomeSources.Find(incomeSourceId).EstimatedIncome != null)
                incomeToReturn.EstimatedIncome = (decimal)_db.IncomeSources.Find(incomeSourceId).EstimatedIncome;
            return incomeToReturn;
        }

        public IncomeLine GetExistingIncome(int incomeId)
        {
            var income = (from inc in _db.Incomes.Where(i => i.Id == incomeId)
                          select new IncomeLine
                          {
                              IncomeId = inc.Id,
                              Amount = inc.Amount,
                              Details = inc.SourceDetails,
                              IncomeDate = inc.Date,
                              IncomeSourceId = inc.IncomeSource.Id,
                              IsCash = inc.IsCash,
                              IsReimbursement = inc.IsReimbursement,
                          }).FirstOrDefault();
            if (income.IsReimbursement)
            {
                income.PurchaseId = Convert.ToInt32(_db.Incomes.Where(i => i.Id == incomeId).FirstOrDefault().PurchaseId);
            }
            return income;
        }
    }
}
