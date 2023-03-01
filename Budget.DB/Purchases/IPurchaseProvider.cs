using BudgetApi.Models;

namespace Budget.DB;
public interface IPurchaseProvider
{
    IEnumerable<Purchase> GetPurchasesByMonthYear(DateTime monthYear);
}

