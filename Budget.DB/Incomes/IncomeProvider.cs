using Budget.Models;
using BudgetApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Budget.DB.Incomes;

public class IncomeProvider: IIncomeProvider
{
    BudgetEntities _db;

    public IncomeProvider(BudgetEntities db)
    {
        _db = db;
    }

    public async Task<IEnumerable<IncomeSource>> GetIncomeSources(int groupId, bool isActive = true)
    {
        return (await _db.IncomeSources.Where(income => income.BudgetingGroupId == groupId && income.ActiveJob == isActive)
            .ToListAsync())
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

    public async Task<IEnumerable<Income>> GetIncomes(int groupId, DateTime monthYear)
    {
        var incomeLines = await (from i in _db.Incomes.Where(i => i.BudgetingGroupId == groupId && i.Date.Month == monthYear.Month && i.Date.Year == monthYear.Year)
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
                           }).ToListAsync();

        foreach (var line in incomeLines)
        {
            if (line.IsReimbursement)
            {
                line.PurchaseId = (int)(await _db.Incomes.Where(i => i.Id == line.Id).FirstOrDefaultAsync()).PurchaseId;
            }
        }
        return incomeLines;
    }

    public async Task<bool> AddUpdateIncome(int groupId, Income inputIncome, int incomeId = -1)
    {
        if (incomeId == -1)
        {
            bool success = false;
            await _db.Incomes.AddAsync(new IncomeEntity
            {
                Amount = inputIncome.Amount,
                Date = inputIncome.Date,
                Id = inputIncome.Id,
                SourceId = inputIncome.SourceId,
                //IncomeSource = 
                IsCash = inputIncome.IsCash,
                IsReimbursement = inputIncome.IsReimbursement,
                PurchaseId = inputIncome.PurchaseId,
                SourceDetails = inputIncome.SourceDetails,
                BudgetingGroupId = groupId
            });

            await _db.SaveChangesAsync();

            try
            {
                var checkIncome = _db.Incomes.Where(i => i.Amount == inputIncome.Amount).FirstOrDefault();
                if (inputIncome.IsReimbursement == true)
                {
                    (await _db.Purchases.Where(i => i.Id == inputIncome.PurchaseId).FirstOrDefaultAsync()).FutureReimbursement = true;
                    await _db.SaveChangesAsync();
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
            var income = await _db.Incomes.Where(i => i.Id == incomeId).FirstOrDefaultAsync();
            income.IsCash = inputIncome.IsCash;
            income.IsReimbursement = inputIncome.IsReimbursement;
            income.SourceDetails = inputIncome.SourceDetails;
            income.SourceId = inputIncome.SourceId;
            income.Amount = inputIncome.Amount;
            await _db.SaveChangesAsync();

            try
            {
                var checkIncome = await _db.Incomes.Where(i => i.Id == incomeId).FirstOrDefaultAsync();
                if (inputIncome.IsReimbursement == true)
                {
                    (await _db.Purchases.Where(i => i.Id == inputIncome.PurchaseId).FirstOrDefaultAsync()).FutureReimbursement = true;
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

    public async Task<bool> DeleteIncomeEntry(int incomeId)
    {
        try
        {
            var toDelete = await _db.Incomes.FindAsync(incomeId);
            _db.Incomes.Remove(toDelete);
            await _db.SaveChangesAsync();
            return true;
        }
        catch
        {

        }

        return false;
    }

    public async Task<bool> AddUpdateJob(IncomeSource inputJob, int incomeSourceId = -1)
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
                await _db.IncomeSources.AddAsync(jobToAddUpdate);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
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
                var incomeSource = await _db.IncomeSources.FindAsync(incomeSourceId);
                incomeSource.ActiveJob = jobToAddUpdate.ActiveJob;
                incomeSource.EstimatedIncome = jobToAddUpdate.EstimatedIncome;
                incomeSource.JobOf = jobToAddUpdate.JobOf;
                incomeSource.PayFrequency = jobToAddUpdate.PayFrequency;
                incomeSource.PositionName = jobToAddUpdate.PositionName;
                incomeSource.SourceName = jobToAddUpdate.SourceName;
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    public async Task<int> AddIncome(int groupId, Income inputIncome)
    {
        try
        {
            await _db.Incomes.AddAsync(new IncomeEntity
            {
                Amount = inputIncome.Amount,
                Date = inputIncome.Date,
                IsCash = inputIncome.IsCash,
                IsReimbursement = inputIncome.IsReimbursement,
                PurchaseId = inputIncome.PurchaseId,
                SourceDetails = inputIncome.SourceDetails,
                SourceId = inputIncome.SourceId,
                BudgetingGroupId = groupId
            });
            await _db.SaveChangesAsync();

            var checkIncome = await _db.Incomes.Where(i => i.Amount == inputIncome.Amount).FirstOrDefaultAsync();
            if (inputIncome.IsReimbursement == true)
            {
                (await _db.Purchases.Where(i => i.Id == inputIncome.PurchaseId).FirstOrDefaultAsync()).FutureReimbursement = true;
                await _db.SaveChangesAsync();
            }

            return inputIncome.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("New income failed to save: ", ex);
        }
    }

    public async Task<bool> UpdateIncome(Income inputIncome)
    {
        bool success = false;
        var incomeToUpdate = await _db.Incomes.Where(i => i.Id == inputIncome.Id).FirstOrDefaultAsync();
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

            await _db.SaveChangesAsync();

            if (incomeToUpdate.IsReimbursement == true)
            {
                (await _db.Purchases.Where(i => i.Id == inputIncome.PurchaseId).FirstOrDefaultAsync()).FutureReimbursement = true;
                await _db.SaveChangesAsync();
            }
            success = true;
        }
        catch (Exception ex)
        {
            throw new Exception("Income Update failed because of internal exception: ", ex);
        }

        return success;
    }

    public async Task<bool> DeleteJobEntry(int incomeSourceId)
    {
        try
        {
            var toDelete = await _db.IncomeSources.Where(i => i.Id == incomeSourceId).FirstOrDefaultAsync();
            _db.IncomeSources.Remove(toDelete);
            await _db.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<IncomeSource> GetIncomeSource(int incomeSourceId)
    {
        var incomeToReturn = await (from ins in _db.IncomeSources
                              where ins.Id == incomeSourceId
                              select new IncomeSource
                              {
                                  SourceName = ins.SourceName,
                                  ActiveJob = ins.ActiveJob,
                                  JobOf = ins.JobOf,
                                  PayFrequency = ins.PayFrequency,
                                  PositionName = ins.PositionName
                              }).FirstOrDefaultAsync();

        if ((await _db.IncomeSources.FindAsync(incomeSourceId)).EstimatedIncome != null)
            incomeToReturn.EstimatedIncome = (decimal)(await _db.IncomeSources.FindAsync(incomeSourceId)).EstimatedIncome;
        
        return incomeToReturn;
    }

    public async Task<Income> GetExistingIncome(int incomeId)
    {
        var income = await (from inc in _db.Incomes.Where(i => i.Id == incomeId)
                      select new Income
                      {
                          Id = inc.Id,
                          Amount = inc.Amount,
                          SourceDetails = inc.SourceDetails,
                          Date = inc.Date,
                          SourceId = inc.IncomeSource.Id,
                          IsCash = inc.IsCash,
                          IsReimbursement = inc.IsReimbursement,
                      }).FirstOrDefaultAsync();

        if (income.IsReimbursement)
        {
            income.PurchaseId = Convert.ToInt32((await _db.Incomes.Where(i => i.Id == incomeId).FirstOrDefaultAsync()).PurchaseId);
        }

        return income;
    }
}