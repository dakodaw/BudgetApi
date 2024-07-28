using Budget.DB;
using Budget.Models;
using BudgetApi.Incomes.Models;
using BudgetApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetApi.Incomes.Services
{
    public class IncomeSourceService: IIncomeSourceService
    {
        BudgetEntities _db;
        public IncomeSourceService(BudgetEntities db)
        {
            _db = db;
        }

        public List<IncomeSource> GetIncomeSources()
        {
            var incomeSourceLines = (from it in _db.IncomeSources.Where(i => i.ActiveJob == true)
                                     select new IncomeSource
                                     {
                                         Id = it.Id,
                                         SourceName = it.SourceName,
                                         JobOf = it.JobOf,
                                         ActiveJob = it.ActiveJob,
                                         PositionName = it.PositionName,
                                         PayFrequency = it.PayFrequency,
                                         EstimatedIncome = it.EstimatedIncome ?? 0
                                     }).ToList();

            return incomeSourceLines;
        }

        public bool AddUpdateIncomeSource(IncomeSourceEntity inputIncomeSource, int incomeSourceId = -1)
        {
            if (incomeSourceId == -1)
            {
                try
                {
                    var sourceId = AddIncomeSource(inputIncomeSource);
                    return sourceId > 0;
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
                    UpdateIncomeSource(inputIncomeSource);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public int AddIncomeSource(IncomeSourceEntity inputIncomeSource)
        {
            inputIncomeSource.ActiveJob = true;
            try
            {
                _db.IncomeSources.Add(inputIncomeSource);
                _db.SaveChanges();
                return inputIncomeSource.Id;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add Income Source", ex);
            }
        }

        public int AddIncomeSource(IncomeSource inputIncomeSource)
        {
            var newIncomeSource = new IncomeSourceEntity
            {
                ActiveJob = true,
                EstimatedIncome = inputIncomeSource.EstimatedIncome,
                JobOf = inputIncomeSource.JobOf,
                PayFrequency = inputIncomeSource.PayFrequency,
                PositionName = inputIncomeSource.PositionName,
                SourceName = inputIncomeSource.SourceName
            };

            try
            {
                _db.IncomeSources.Add(newIncomeSource);

                _db.SaveChanges();
                return newIncomeSource.Id;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add Income Source", ex);
            }
        }

        public void UpdateIncomeSource(IncomeSourceEntity inputIncomeSource)
        {
            var incomeSourceId = inputIncomeSource.Id;

            try
            {
                _db.IncomeSources.Find(incomeSourceId).ActiveJob = inputIncomeSource.ActiveJob;
                _db.IncomeSources.Find(incomeSourceId).EstimatedIncome = inputIncomeSource.EstimatedIncome;
                _db.IncomeSources.Find(incomeSourceId).JobOf = inputIncomeSource.JobOf;
                _db.IncomeSources.Find(incomeSourceId).PayFrequency = inputIncomeSource.PayFrequency;
                _db.IncomeSources.Find(incomeSourceId).PositionName = inputIncomeSource.PositionName;
                _db.IncomeSources.Find(incomeSourceId).SourceName = inputIncomeSource.SourceName;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Update Income Source", ex);
            }
        }

        public void UpdateIncomeSource(IncomeSource inputIncomeSource)
        {
            try
            {
                var incomeSource = _db.IncomeSources.Find(inputIncomeSource.Id);

                incomeSource.ActiveJob = inputIncomeSource.ActiveJob;
                incomeSource.EstimatedIncome = inputIncomeSource.EstimatedIncome;
                incomeSource.JobOf = inputIncomeSource.JobOf;
                incomeSource.PayFrequency = inputIncomeSource.PayFrequency;
                incomeSource.PositionName = inputIncomeSource.PositionName;
                incomeSource.SourceName = inputIncomeSource.SourceName;

                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Update Income Source", ex);
            }
        }

        public bool DeleteIncomeSource(int incomeSourceId)
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
                throw new Exception($"Failed to delete income source with id {incomeSourceId}", ex);
            }
        }

        public IncomeSource GetIncomeSource(int incomeSourceId)
        {
            var incomeToReturn = (from ins in _db.IncomeSources
                                  where ins.Id == incomeSourceId
                                  select new IncomeSource
                                  {
                                      SourceName = ins.SourceName,
                                      ActiveJob = ins.ActiveJob,
                                      JobOf = ins.JobOf,
                                      PayFrequency = ins.PayFrequency,
                                      PositionName = ins.PositionName
                                  }).FirstOrDefault();
            if (_db.IncomeSources.Find(incomeSourceId).EstimatedIncome != null)
                incomeToReturn.EstimatedIncome = (decimal)_db.IncomeSources.Find(incomeSourceId).EstimatedIncome;
            
            return incomeToReturn;
        }
    }
}
