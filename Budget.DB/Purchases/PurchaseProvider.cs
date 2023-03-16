using BudgetApi.Models;
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
                Date = x.Date,
                Id = x.Id,
                Description = x.Description,
                FutureReimbursement = x.FutureReimbursement,
                GiftCardId = x.GiftCardId,
                PaymentType = x.PaymentType,
                PurchaseTypeId = x.PurchaseTypeId
            });
    }

}

