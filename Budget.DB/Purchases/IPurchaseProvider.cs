using BudgetApi.Models;

namespace Budget.DB;
public interface IPurchaseProvider
{
    IEnumerable<Purchase> GetPurchasesByMonthYear(DateTime monthYear);
    IEnumerable<Purchase> GetPurchasesByReceiptRecordGroup(Guid id);
    Purchase GetPurchase(int purchaseId);
    IEnumerable<Purchase> GetGiftCardPurchases(int giftCardId);
    IEnumerable<Purchase> GetAllGiftCardPurchases();
    IEnumerable<Purchase> GetMonthGiftCardPurchases(DateTime monthYear);
    bool AddUpdatePurchase(Purchase inputPurchase, int purchaseId = -1);
    int AddPurchase(Purchase inputPurchase);
    void UpdatePurchase(Purchase inputPurchase);
    void DeletePurchaseEntry(int purchaseId);
    bool DeletePurchaseEntryObsolete(int purchaseId);
}

