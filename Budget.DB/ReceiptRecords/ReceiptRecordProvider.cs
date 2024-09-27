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
                Date = inputReceiptRecord.Date,
                Amount = inputReceiptRecord.Amount,
                Location = inputReceiptRecord.Location
            });
            _db.SaveChanges();

            return inputReceiptRecord.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("New income failed to save: ", ex);
        }
    }

    public void Update(ReceiptRecord inputReceiptRecord)
    {
        var incomeToUpdate = _db.ReceiptRecord
            .Where(i => i.Id == inputReceiptRecord.Id)
            .FirstOrDefault();

        if (incomeToUpdate == default)
            throw new Exception($"Custom Income Not found Exception for {inputReceiptRecord.Id}");

        try
        {
            incomeToUpdate.Id = inputReceiptRecord.Id;
            incomeToUpdate.Date = inputReceiptRecord.Date;
            incomeToUpdate.Amount = inputReceiptRecord.Amount;
            incomeToUpdate.Location = inputReceiptRecord.Location;

            _db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception("Income Update failed because of internal exception: ", ex);
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
            throw new Exception("Income Failed to Delete", ex);
        }
    }
}