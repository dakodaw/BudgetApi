using Budget.Models;
using BudgetApi.Models;

namespace Budget.DB.ReceiptRecordGroups;

public class ReceiptRecordGroupProvider : IReceiptRecordGroupProvider
{
    BudgetEntities _db;

	public ReceiptRecordGroupProvider(BudgetEntities db)
	{
        _db = db;
	}

    public IEnumerable<ReceiptRecordGroup> List()
    {
        return _db.ReceiptRecordGroup.Select(rrg => 
            new ReceiptRecordGroup
            {
                Id = rrg.Id,
                ReceiptRecordId = rrg.ReceiptRecordId,
                BudgetTypeId = rrg.BudgetTypeId,
                Sum = rrg.Amount
            });
    }

    public ReceiptRecordGroup Get(Guid id)
    {
        var receiptRecordGroup = _db.ReceiptRecordGroup
            .Where(i => i.Id == id)
            .FirstOrDefault();

        return new ReceiptRecordGroup
        {
            Id = receiptRecordGroup.Id,
            ReceiptRecordId = receiptRecordGroup.ReceiptRecordId,
            BudgetTypeId = receiptRecordGroup.BudgetTypeId,
            Sum = receiptRecordGroup.Amount
        };
    }

    public Guid Add(ReceiptRecordGroup inputReceiptRecordGroup)
    {
        try
        {
            var jobToAdd = new ReceiptRecordGroupEntity
            {
                ReceiptRecordId = inputReceiptRecordGroup.ReceiptRecordId,
                BudgetTypeId = inputReceiptRecordGroup.BudgetTypeId,
                Amount = inputReceiptRecordGroup.Sum
            };

            _db.ReceiptRecordGroup.Add(jobToAdd);
            _db.SaveChanges();

            return jobToAdd.Id;
        }
        catch(Exception ex) 
        {
            throw new Exception("Failed to Add ReceiptRecordGroup", ex);
        }
    }

    public void Update(ReceiptRecordGroup inputReceiptRecordGroup)
    {
        try
        {
            var receiptRecordGroup = _db.ReceiptRecordGroup.Find(inputReceiptRecordGroup.Id);
            receiptRecordGroup.Id = receiptRecordGroup.Id;
            receiptRecordGroup.ReceiptRecordId = receiptRecordGroup.ReceiptRecordId;
            receiptRecordGroup.BudgetTypeId = receiptRecordGroup.BudgetTypeId;
            receiptRecordGroup.Amount = receiptRecordGroup.Amount;

            _db.SaveChanges();
        }
        catch(Exception ex)
        {
            throw new Exception("Failed to update ReceiptRecordGroup", ex);
        }
    }

    public void Delete(Guid id)
    {
        try
        {
            var toDelete = _db.ReceiptRecordGroup.FirstOrDefault(x => x.Id == id);
            _db.ReceiptRecordGroup.Remove(toDelete);
            _db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to Delete ReceiptRecordGroup {id}", ex);
        }
    }
}

