using BudgetApi.Models;
using BudgetApi.Purchases.Models;
using Microsoft.EntityFrameworkCore;

namespace Budget.DB;
public class PurchaseProvider: IPurchaseProvider
{
    BudgetEntities _db;

    public PurchaseProvider(BudgetEntities db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Purchase>> GetPurchasesByMonthYear(DateTime monthYear)
    {
        return (await _db.Purchases
            .Where(i =>
                i.Date.Month == monthYear.Month
                && i.Date.Year == monthYear.Year)
            .ToListAsync())
            .Select(x => new Purchase
            {
                Amount = x.Amount,
                BudgetingGroupId = x.BudgetingGroupId,
                Date = x.Date,
                Id = x.Id,
                Description = x.Description,
                FutureReimbursement = x.FutureReimbursement,
                GiftCardId = x.GiftCardId,
                PaymentType = x.PaymentType,
                PurchaseTypeId = x.PurchaseTypeId
            });
    }

    public async Task<IEnumerable<Purchase>> GetPurchasesByReceiptRecordGroup(Guid id)
    {
        return (await _db.Purchases
            .Where(i => i.ReceiptRecordGroupId == id)
            .ToListAsync())
            .Select(x => new Purchase
            {
                Amount = x.Amount,
                BudgetingGroupId = x.BudgetingGroupId,
                Date = x.Date,
                Id = x.Id,
                Description = x.Description,
                FutureReimbursement = x.FutureReimbursement,
                GiftCardId = x.GiftCardId,
                PaymentType = x.PaymentType,
                PurchaseTypeId = x.PurchaseTypeId
            });
    }

    public async Task<Purchase> GetPurchase(int purchaseId)
    {
        var purchaseEntity = await _db.Purchases
            .Where(i => i.Id == purchaseId)
            .FirstOrDefaultAsync();

        return purchaseEntity == null
            ? default
            : new Purchase
            {
                Amount = purchaseEntity.Amount,
                BudgetingGroupId = purchaseEntity.BudgetingGroupId,
                Date = purchaseEntity.Date,
                Description = purchaseEntity.Description,
                FutureReimbursement = purchaseEntity.FutureReimbursement,
                GiftCardId = purchaseEntity.GiftCardId,
                Id = purchaseEntity.Id,
                PaymentType = purchaseEntity.PaymentType,
                PurchaseTypeId = purchaseEntity.PurchaseTypeId
            };
    }

    public async Task<IEnumerable<Purchase>> GetGiftCardPurchases(int giftCardId)
    {
        return (await _db.Purchases
            .Where(x => x.GiftCardId == giftCardId)
            .ToListAsync())
            .Select(x => new Purchase
            {
                Amount = x.Amount,
                BudgetingGroupId = x.BudgetingGroupId,
                Date = x.Date,
                Id = x.Id,
                Description = x.Description,
                FutureReimbursement = x.FutureReimbursement,
                GiftCardId = x.GiftCardId,
                PaymentType = x.PaymentType,
                PurchaseTypeId = x.PurchaseTypeId
            });
    }

    public async Task<IEnumerable<Purchase>> GetAllGiftCardPurchases()
    {
        return (await _db.Purchases
            .Where(x => x.PaymentType == PurchaseTypeNames.GiftCard)
            .ToListAsync())
            .Select(x => new Purchase
            {
                Amount = x.Amount,
                BudgetingGroupId = x.BudgetingGroupId,
                Date = x.Date,
                Id = x.Id,
                Description = x.Description,
                FutureReimbursement = x.FutureReimbursement,
                GiftCardId = x.GiftCardId,
                PaymentType = x.PaymentType,
                PurchaseTypeId = x.PurchaseTypeId
            });
    }

    public async Task<IEnumerable<Purchase>> GetMonthGiftCardPurchases(DateTime monthYear)
    {
        return (await _db.Purchases
            .Where(x => x.PaymentType == PurchaseTypeNames.GiftCard
                        && x.Date.Month == monthYear.Date.Month
                        && x.Date.Year == monthYear.Year)
            .ToListAsync())
            .Select(x => new Purchase
            {
                Amount = x.Amount,
                BudgetingGroupId = x.BudgetingGroupId,
                Date = x.Date,
                Id = x.Id,
                Description = x.Description,
                FutureReimbursement = x.FutureReimbursement,
                GiftCardId = x.GiftCardId,
                PaymentType = x.PaymentType,
                PurchaseTypeId = x.PurchaseTypeId
            });
    }

    public async Task<bool> AddUpdatePurchase(Purchase inputPurchase, int purchaseId = -1)
    {
        bool success = false;
        if (purchaseId == -1)
        {
            await _db.Purchases.AddAsync(new PurchaseEntity
            {
                Amount = inputPurchase.Amount,
                BudgetingGroupId = inputPurchase.BudgetingGroupId,
                Date = inputPurchase.Date,
                Description = inputPurchase.Description,
                FutureReimbursement = inputPurchase.FutureReimbursement,
                GiftCardId = inputPurchase.GiftCardId,
                Id = inputPurchase.Id,
                PaymentType = inputPurchase.PaymentType,
                PurchaseTypeId = inputPurchase.PurchaseTypeId
            });

            await _db.SaveChangesAsync();

            try
            {
                var checkPurchase = _db.Purchases.Where(i => i.Amount == inputPurchase.Amount).FirstOrDefaultAsync();
                success = true;
            }
            catch
            {
                throw new Exception("Unable to Add Purchase");
            }
        }
        else
        {
            try
            {
                var checkPurchase = await _db.Purchases.Where(i => i.Id == purchaseId).FirstOrDefaultAsync();
                checkPurchase.Amount = inputPurchase.Amount;
                checkPurchase.Date = inputPurchase.Date;
                checkPurchase.Description = inputPurchase.Description;
                checkPurchase.FutureReimbursement = inputPurchase.FutureReimbursement;
                checkPurchase.GiftCardId = inputPurchase.GiftCardId;
                checkPurchase.PaymentType = inputPurchase.PaymentType;
                checkPurchase.PurchaseTypeId = inputPurchase.PurchaseTypeId;
                await _db.SaveChangesAsync();
                success = true;
            }
            catch (Exception ee)
            {
                success = false;
                throw new Exception("Unable to update Purchase", ee.InnerException);
            }
        }
        return success;
    }

    public async Task<int> AddPurchase(Purchase inputPurchase)
    {
        try
        {
            var newPurchaseEntity = new PurchaseEntity
            {
                Amount = inputPurchase.Amount,
                BudgetingGroupId = inputPurchase.BudgetingGroupId,
                Date = inputPurchase.Date,
                Description = inputPurchase.Description,
                FutureReimbursement = inputPurchase.FutureReimbursement,
                GiftCardId = inputPurchase.GiftCardId,
                Id = inputPurchase.Id,
                PaymentType = inputPurchase.PaymentType,
                PurchaseTypeId = inputPurchase.PurchaseTypeId,
                ReceiptRecordGroupId = inputPurchase.ReceiptRecordGroupId
            };
            await _db.Purchases.AddAsync(newPurchaseEntity);

            await _db.SaveChangesAsync();
            return newPurchaseEntity.Id;
        }
        catch(Exception ex)
        {
            throw new Exception("Unable to Add Purchase", ex);
        }
    }

    public async Task UpdatePurchase(Purchase inputPurchase)
    {
        try
        {
            var purchaseToUpdate = await _db.Purchases.Where(i => i.Id == inputPurchase.Id).FirstOrDefaultAsync();
            purchaseToUpdate.Amount = inputPurchase.Amount;
            purchaseToUpdate.BudgetingGroupId = inputPurchase.BudgetingGroupId;
            purchaseToUpdate.Date = inputPurchase.Date;
            purchaseToUpdate.Description = inputPurchase.Description;
            purchaseToUpdate.FutureReimbursement = inputPurchase.FutureReimbursement;
            purchaseToUpdate.GiftCardId = inputPurchase.GiftCardId;
            purchaseToUpdate.PaymentType = inputPurchase.PaymentType;
            purchaseToUpdate.PurchaseTypeId = inputPurchase.PurchaseTypeId;
            purchaseToUpdate.ReceiptRecordGroupId = inputPurchase.ReceiptRecordGroupId;
                
            await _db.SaveChangesAsync();
        }
        catch (Exception ee)
        {
            throw new Exception("Unable to update Purchase", ee.InnerException);
        }
    }

    public async Task DeletePurchaseEntry(int purchaseId)
    {
        try
        {
            var toDelete = await _db.Purchases.Where(i => i.Id == purchaseId).FirstOrDefaultAsync();
            _db.Purchases.Remove(toDelete);
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to Delete Purchase entry {purchaseId}", ex);
        }
    }

    public async Task<bool> DeletePurchaseEntryObsolete(int purchaseId)
    {
        bool success = false;
        try
        {
            await DeletePurchaseEntry(purchaseId);
            return success;
        }
        catch
        {
            return success;
        }
    }
}

