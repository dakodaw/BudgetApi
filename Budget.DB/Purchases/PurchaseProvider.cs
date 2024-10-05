﻿using BudgetApi.Models;
using BudgetApi.Purchases.Models;

namespace Budget.DB;
public class PurchaseProvider: IPurchaseProvider
{
    BudgetEntities _db;

    public PurchaseProvider(BudgetEntities db)
    {
        _db = db;
    }

    public IEnumerable<Purchase> GetPurchasesByMonthYear(DateTime monthYear)
    {
        return _db.Purchases
            .Where(i =>
                i.Date.Month == monthYear.Month
                && i.Date.Year == monthYear.Year)
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

    public IEnumerable<Purchase> GetPurchasesByReceiptRecordGroup(Guid id)
    {
        return _db.Purchases
            .Where(i => i.ReceiptRecordGroupId == id)
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

    public Purchase GetPurchase(int purchaseId)
    {
        var purchaseEntity = _db.Purchases
            .Where(i => i.Id == purchaseId)
            .FirstOrDefault();

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

    public IEnumerable<Purchase> GetGiftCardPurchases(int giftCardId)
    {
        return _db.Purchases
            .Where(x => x.GiftCardId == giftCardId)
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

    public IEnumerable<Purchase> GetAllGiftCardPurchases()
    {
        return _db.Purchases
            .Where(x => x.PaymentType == PurchaseTypeNames.GiftCard)
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

    public IEnumerable<Purchase> GetMonthGiftCardPurchases(DateTime monthYear)
    {
        return _db.Purchases
            .Where(x => x.PaymentType == PurchaseTypeNames.GiftCard
                        && x.Date.Month == monthYear.Date.Month
                        && x.Date.Year == monthYear.Year)
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

    public bool AddUpdatePurchase(Purchase inputPurchase, int purchaseId = -1)
    {
        bool success = false;
        if (purchaseId == -1)
        {
            _db.Purchases.Add(new PurchaseEntity
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

            _db.SaveChanges();

            try
            {
                var checkPurchase = _db.Purchases.Where(i => i.Amount == inputPurchase.Amount).FirstOrDefault();
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
                var checkPurchase = _db.Purchases.Where(i => i.Id == purchaseId).FirstOrDefault();
                _db.Purchases.Where(i => i.Id == purchaseId).FirstOrDefault().Amount = inputPurchase.Amount;
                _db.Purchases.Where(i => i.Id == purchaseId).FirstOrDefault().Date = inputPurchase.Date;
                _db.Purchases.Where(i => i.Id == purchaseId).FirstOrDefault().Description = inputPurchase.Description;
                _db.Purchases.Where(i => i.Id == purchaseId).FirstOrDefault().FutureReimbursement = inputPurchase.FutureReimbursement;
                _db.Purchases.Where(i => i.Id == purchaseId).FirstOrDefault().GiftCardId = inputPurchase.GiftCardId;
                _db.Purchases.Where(i => i.Id == purchaseId).FirstOrDefault().PaymentType = inputPurchase.PaymentType;
                _db.Purchases.Where(i => i.Id == purchaseId).FirstOrDefault().PurchaseTypeId = inputPurchase.PurchaseTypeId;
                _db.SaveChanges();
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

    public int AddPurchase(Purchase inputPurchase)
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
            _db.Purchases.Add(newPurchaseEntity);

            _db.SaveChanges();
            return newPurchaseEntity.Id;
        }
        catch(Exception ex)
        {
            throw new Exception("Unable to Add Purchase", ex);
        }
    }

    public void UpdatePurchase(Purchase inputPurchase)
    {
        try
        {
            var purchaseToUpdate = _db.Purchases.Where(i => i.Id == inputPurchase.Id).FirstOrDefault();
            purchaseToUpdate.Amount = inputPurchase.Amount;
            purchaseToUpdate.BudgetingGroupId = inputPurchase.BudgetingGroupId;
            purchaseToUpdate.Date = inputPurchase.Date;
            purchaseToUpdate.Description = inputPurchase.Description;
            purchaseToUpdate.FutureReimbursement = inputPurchase.FutureReimbursement;
            purchaseToUpdate.GiftCardId = inputPurchase.GiftCardId;
            purchaseToUpdate.PaymentType = inputPurchase.PaymentType;
            purchaseToUpdate.PurchaseTypeId = inputPurchase.PurchaseTypeId;
            purchaseToUpdate.ReceiptRecordGroupId = inputPurchase.ReceiptRecordGroupId;
                
            _db.SaveChanges();
        }
        catch (Exception ee)
        {
            throw new Exception("Unable to update Purchase", ee.InnerException);
        }
    }

    public void DeletePurchaseEntry(int purchaseId)
    {
        try
        {
            var toDelete = _db.Purchases.Where(i => i.Id == purchaseId).FirstOrDefault();
            _db.Purchases.Remove(toDelete);
            _db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to Delete Purchase entry {purchaseId}", ex);
        }
    }

    public bool DeletePurchaseEntryObsolete(int purchaseId)
    {
        bool success = false;
        try
        {
            DeletePurchaseEntry(purchaseId);
            return success;
        }
        catch
        {
            return success;
        }
    }
}

