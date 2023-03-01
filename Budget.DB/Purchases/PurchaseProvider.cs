using BudgetApi.Models;

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
}

