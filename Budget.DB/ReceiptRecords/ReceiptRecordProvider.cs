using Budget.Models;
using BudgetApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Budget.DB.Incomes;

public class ReceiptRecordProvider : IReceiptRecordProvider
{
    BudgetEntities _db;

    public ReceiptRecordProvider(BudgetEntities db)
    {
        _db = db;
    }

    public async Task<IEnumerable<ReceiptRecord>> List(int groupId, DateTime? monthYear = null)
    {
        var receiptRecords = monthYear.HasValue
            ? await _db.ReceiptRecord
                .Where(r => r.BudgetingGroupId == groupId
                    && r.Date.Month == monthYear.Value.Month
                    && r.Date.Year == monthYear.Value.Year)
                .ToListAsync()
            : await _db.ReceiptRecord
                .Where(r => r.BudgetingGroupId == groupId)
                .ToListAsync();

        return receiptRecords.Select(x =>
            new ReceiptRecord()
            {
                Id = x.Id,
                BudgetingGroupId = x.BudgetingGroupId,
                Date = x.Date,
                Amount = x.Amount,
                Location = x.Location,
            }).OrderBy(i => i.Date);
    }

    public async Task<ReceiptRecord> Get(Guid id)
    {
        var receiptRecord = await _db.ReceiptRecord
            .FirstOrDefaultAsync(x => x.Id == id);

        if (receiptRecord == default)
            throw new Exception("Failed to get a Receipt Record");

        return new ReceiptRecord()
        {
            Id = receiptRecord.Id,
            BudgetingGroupId = receiptRecord.BudgetingGroupId,
            Date = receiptRecord.Date,
            Amount = receiptRecord.Amount,
            Location = receiptRecord.Location,
        };
    }

    public async Task<Guid> Add(int groupId, ReceiptRecord inputReceiptRecord)
    {
        try
        {
            var receiptRecordEntity = new ReceiptRecordEntity
            {
                Id = inputReceiptRecord.Id,
                BudgetingGroupId = groupId,
                Date = inputReceiptRecord.Date,
                Amount = inputReceiptRecord.Amount,
                Location = inputReceiptRecord.Location
            };

            await _db.ReceiptRecord.AddAsync(receiptRecordEntity);
            await _db.SaveChangesAsync();

            return receiptRecordEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("New ReceiptRecord failed to save: ", ex);
        }
    }

    public async Task Update(ReceiptRecord inputReceiptRecord)
    {
        var receiptRecordToUpdate = await _db.ReceiptRecord
            .Where(i => i.Id == inputReceiptRecord.Id)
            .FirstOrDefaultAsync();

        if (receiptRecordToUpdate == default)
            throw new Exception($"Custom ReceiptRecord Not found Exception for {inputReceiptRecord.Id}");

        try
        {
            receiptRecordToUpdate.Id = inputReceiptRecord.Id;
            receiptRecordToUpdate.BudgetingGroupId = inputReceiptRecord.BudgetingGroupId;
            receiptRecordToUpdate.Date = inputReceiptRecord.Date;
            receiptRecordToUpdate.Amount = inputReceiptRecord.Amount;
            receiptRecordToUpdate.Location = inputReceiptRecord.Location;

            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("ReceiptRecord Update failed because of internal exception: ", ex);
        }
    }

    public async Task Delete(Guid id)
    {
        try
        {
            var toDelete = await _db.ReceiptRecord.Where(i => i.Id == id).FirstOrDefaultAsync();
            _db.ReceiptRecord.Remove(toDelete);
            await _db.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            throw new Exception("ReceiptRecord Failed to Delete", ex);
        }
    }
}