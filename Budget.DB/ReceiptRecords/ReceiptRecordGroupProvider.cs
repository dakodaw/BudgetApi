using Budget.Models;
using BudgetApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Budget.DB.ReceiptRecordGroups;

public class ReceiptRecordGroupProvider : IReceiptRecordGroupProvider
{
    BudgetEntities _db;

	public ReceiptRecordGroupProvider(BudgetEntities db)
	{
        _db = db;
	}

    public async Task<IEnumerable<ReceiptRecordGroup>> List(Guid? recordGroupId)
    {
        var list = await _db.ReceiptRecordGroup
            .Where(x => recordGroupId.HasValue ? x.ReceiptRecordId == recordGroupId.Value : true)
            .ToListAsync();

        return list.Select(rrg => 
            new ReceiptRecordGroup
            {
                Id = rrg.Id,
                ReceiptRecordId = rrg.ReceiptRecordId,
                BudgetTypeId = rrg.BudgetTypeId,
                Sum = rrg.Amount
            });
    }

    public async Task<ReceiptRecordGroup> Get(Guid id)
    {
        var receiptRecordGroup = await _db.ReceiptRecordGroup
            .Where(i => i.Id == id)
            .FirstOrDefaultAsync();

        return new ReceiptRecordGroup
        {
            Id = receiptRecordGroup.Id,
            ReceiptRecordId = receiptRecordGroup.ReceiptRecordId,
            BudgetTypeId = receiptRecordGroup.BudgetTypeId,
            Sum = receiptRecordGroup.Amount
        };
    }

    public async Task<Guid> Add(ReceiptRecordGroup inputReceiptRecordGroup)
    {
        try
        {
            var jobToAdd = new ReceiptRecordGroupEntity
            {
                ReceiptRecordId = inputReceiptRecordGroup.ReceiptRecordId,
                BudgetTypeId = inputReceiptRecordGroup.BudgetTypeId,
                Amount = inputReceiptRecordGroup.Sum
            };

            await _db.ReceiptRecordGroup.AddAsync(jobToAdd);
            await _db.SaveChangesAsync();

            return jobToAdd.Id;
        }
        catch(Exception ex) 
        {
            throw new Exception("Failed to Add ReceiptRecordGroup", ex);
        }
    }

    public async Task Update(ReceiptRecordGroup inputReceiptRecordGroup)
    {
        try
        {
            var receiptRecordGroup = await _db.ReceiptRecordGroup.FindAsync(inputReceiptRecordGroup.Id);
            receiptRecordGroup.ReceiptRecordId = inputReceiptRecordGroup.ReceiptRecordId;
            receiptRecordGroup.BudgetTypeId = inputReceiptRecordGroup.BudgetTypeId;
            receiptRecordGroup.Amount = inputReceiptRecordGroup.Sum;

            await _db.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            throw new Exception("Failed to update ReceiptRecordGroup", ex);
        }
    }

    public async Task Delete(Guid id)
    {
        try
        {
            var toDelete = await _db.ReceiptRecordGroup.FirstOrDefaultAsync(x => x.Id == id);
            _db.ReceiptRecordGroup.Remove(toDelete);
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to Delete ReceiptRecordGroup {id}", ex);
        }
    }
}

