using Budget.Models;
using BudgetApi.Models;

namespace Budget.DB.Incomes;

public class ReceiptRecordProvider : IReceiptRecordProvider
{
    BudgetEntities _db;

    public ReceiptRecordProvider(BudgetEntities db)
    {
        _db = db;
    }

    public IEnumerable<ReceiptRecord> List()
    {
        return _db.ReceiptRecord
            .Select(x =>
            new ReceiptRecord()
            {
                Id = x.Id,
                BudgetingGroupId = x.BudgetingGroupId,
                Date = x.Date,
                Amount = x.Amount,
                Location = x.Location,
            }).OrderBy(i => i.Date);
    }

    public ReceiptRecord Get(Guid id)
    {
        var receiptRecord = _db.ReceiptRecord
            .FirstOrDefault(x => x.Id == id);

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

    public Guid Add(ReceiptRecord inputReceiptRecord)
    {
        try
        {
            _db.ReceiptRecord.Add(new ReceiptRecordEntity
            {
                Id = inputReceiptRecord.Id,
                BudgetingGroupId = inputReceiptRecord.BudgetingGroupId,
                Date = inputReceiptRecord.Date,
                Amount = inputReceiptRecord.Amount,
                Location = inputReceiptRecord.Location
            });
            _db.SaveChanges();

            return inputReceiptRecord.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("New ReceiptRecord failed to save: ", ex);
        }
    }

    public void Update(ReceiptRecord inputReceiptRecord)
    {
        var receiptRecordToUpdate = _db.ReceiptRecord
            .Where(i => i.Id == inputReceiptRecord.Id)
            .FirstOrDefault();

        if (receiptRecordToUpdate == default)
            throw new Exception($"Custom ReceiptRecord Not found Exception for {inputReceiptRecord.Id}");

        try
        {
            receiptRecordToUpdate.Id = inputReceiptRecord.Id;
            receiptRecordToUpdate.BudgetingGroupId = inputReceiptRecord.BudgetingGroupId;
            receiptRecordToUpdate.Date = inputReceiptRecord.Date;
            receiptRecordToUpdate.Amount = inputReceiptRecord.Amount;
            receiptRecordToUpdate.Location = inputReceiptRecord.Location;

            _db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception("ReceiptRecord Update failed because of internal exception: ", ex);
        }
    }

    public void Delete(Guid id)
    {
        try
        {
            var toDelete = _db.ReceiptRecord.Where(i => i.Id == id).FirstOrDefault();
            _db.ReceiptRecord.Remove(toDelete);
            _db.SaveChanges();
        }
        catch(Exception ex)
        {
            throw new Exception("ReceiptRecord Failed to Delete", ex);
        }
    }
}