using BudgetApi.Models;

namespace Budget.DB;
public interface IPurchaseProvider
{
    Task<IEnumerable<Purchase>> GetPurchasesByMonthYear(DateTime monthYear);
    Task<IEnumerable<Purchase>> GetPurchasesByReceiptRecordGroup(Guid id);
    Task<Purchase> GetPurchase(int purchaseId);
    Task<IEnumerable<Purchase>> GetGiftCardPurchases(int giftCardId);
    Task<IEnumerable<Purchase>> GetAllGiftCardPurchases();
    Task<IEnumerable<Purchase>> GetMonthGiftCardPurchases(DateTime monthYear);
    Task<bool> AddUpdatePurchase(Purchase inputPurchase, int purchaseId = -1);
    Task<int> AddPurchase(Purchase inputPurchase);
    Task UpdatePurchase(Purchase inputPurchase);
    Task DeletePurchaseEntry(int purchaseId);
    Task<bool> DeletePurchaseEntryObsolete(int purchaseId);
}

