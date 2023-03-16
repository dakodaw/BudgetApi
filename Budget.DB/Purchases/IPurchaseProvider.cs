using BudgetApi.Models;

namespace Budget.DB;
public interface IPurchaseProvider
{
    IEnumerable<Purchase> GetPurchasesByMonthYear(DateTime monthYear);
    Purchase GetPurchase(int purchaseId);
    IEnumerable<Purchase> GetGiftCardPurchases(int giftCardId);
    IEnumerable<Purchase> GetAllGiftCardPurchases();
    IEnumerable<Purchase> GetMonthGiftCardPurchases(DateTime monthYear);
    bool AddUpdatePurchase(Purchase inputPurchase, int purchaseId = -1);
    bool DeletePurchaseEntry(int purchaseId);
}

