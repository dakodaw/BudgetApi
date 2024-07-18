using Budget.Models;
using BudgetApi.Models;

namespace Budget.DB.Incomes;

public class IncomeProvider: IIncomeProvider
{
    BudgetEntities _db;

    public IncomeProvider(BudgetEntities db)
    {
        _db = db;
    }

    public IEnumerable<IncomeSource> GetIncomeSources(bool isActive = true)
    {
        return _db.IncomeSources.Where(income => income.ActiveJob == isActive)
            .Select(x =>
            new IncomeSource()
            {
                Id = x.Id,
                SourceName = x.SourceName,
                PositionName = x.PositionName,
                JobOf = x.JobOf,
                ActiveJob = x.ActiveJob,
                EstimatedIncome = x.EstimatedIncome,
                PayFrequency = x.PayFrequency
            }
        ).OrderBy(i => i.SourceName);
    }

    public IEnumerable<Income> GetIncomes(DateTime monthYear)
    {
        var incomeLines = (from i in _db.Incomes.Where(i => i.Date.Month == monthYear.Month && i.Date.Year == monthYear.Year)
                           join it in _db.IncomeSources on i.SourceId equals it.Id
                           select new Income
                           {
                               Id = i.Id,
                               SourceDetails = i.SourceDetails,
                               Date = i.Date,
                               SourceId = it.Id,
                               Amount = i.Amount,
                               IsReimbursement = i.IsReimbursement,
                               IsCash = i.IsCash
                           });
        foreach (var line in incomeLines)
        {
            if (line.IsReimbursement)
            {
                line.PurchaseId = (int)_db.Incomes.Where(i => i.Id == line.Id).FirstOrDefault().PurchaseId;
            }
        }
        return incomeLines;
    }

    public bool AddUpdateIncome(Income inputIncome, int incomeId = -1)
    {
        if (incomeId == -1)
        {
            bool success = false;
            _db.Incomes.Add(new IncomeEntity
            {
                Amount = inputIncome.Amount,
                Date = inputIncome.Date,
                Id = inputIncome.Id,
                SourceId = inputIncome.SourceId,
                //IncomeSource = 
                IsCash = inputIncome.IsCash,
                IsReimbursement = inputIncome.IsReimbursement,
                PurchaseId = inputIncome.PurchaseId,
                SourceDetails = inputIncome.SourceDetails
            });

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
            //_db.Incomes.Where(i => i.Id == incomeId).FirstOrDefault().IncomeSource = inputIncome.SourceDetails;
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
            var jobToAddUpdate = new IncomeSourceEntity
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
                _db.IncomeSources.Add(jobToAddUpdate);
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

    public int AddIncome(Income inputIncome)
    {
        try
        {
            _db.Incomes.Add(new IncomeEntity
            {
                Amount = inputIncome.Amount,
                Date = inputIncome.Date,
                IsCash = inputIncome.IsCash,
                IsReimbursement = inputIncome.IsReimbursement,
                PurchaseId = inputIncome.PurchaseId,
                SourceDetails = inputIncome.SourceDetails,
                SourceId = inputIncome.SourceId
            });
            _db.SaveChanges();

            var checkIncome = _db.Incomes.Where(i => i.Amount == inputIncome.Amount).FirstOrDefault();
            if (inputIncome.IsReimbursement == true)
            {
                _db.Purchases.Where(i => i.Id == inputIncome.PurchaseId).FirstOrDefault().FutureReimbursement = true;
                _db.SaveChanges();
            }

            return inputIncome.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("New income failed to save: ", ex);
        }
    }

    public bool UpdateIncome(Income inputIncome)
    {
        bool success = false;
        var incomeToUpdate = _db.Incomes.Where(i => i.Id == inputIncome.Id).FirstOrDefault();
        if (incomeToUpdate == default)
            throw new Exception($"Custom Income Not found Exception for {inputIncome.Id}");

        try
        {
            incomeToUpdate.IsCash = inputIncome.IsCash;
            incomeToUpdate.IsReimbursement = inputIncome.IsReimbursement;
            incomeToUpdate.SourceDetails = inputIncome.SourceDetails;
            incomeToUpdate.SourceId = inputIncome.SourceId;
            incomeToUpdate.Amount = inputIncome.Amount;
            incomeToUpdate.Date = inputIncome.Date;

            _db.SaveChanges();

            if (incomeToUpdate.IsReimbursement == true)
            {
                _db.Purchases.Where(i => i.Id == inputIncome.PurchaseId).FirstOrDefault().FutureReimbursement = true;
                _db.SaveChanges();
            }
            success = true;
        }
        catch (Exception ex)
        {
            throw new Exception("Income Update failed because of internal exception: ", ex);
        }

        return success;
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

    public Income GetExistingIncome(int incomeId)
    {
        var income = (from inc in _db.Incomes.Where(i => i.Id == incomeId)
                      select new Income
                      {
                          Id = inc.Id,
                          Amount = inc.Amount,
                          SourceDetails = inc.SourceDetails,
                          Date = inc.Date,
                          SourceId = inc.IncomeSource.Id,
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