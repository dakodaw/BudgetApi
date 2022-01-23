using BudgetApi.Models;
using System.Collections.Generic;

namespace BudgetApi.GiftCards.Models
{
    public class GiftCardHistoryBalance
    {
        public string Place { get; set; }
        public string CardNo { get; set; }
        public string AccessCode { get; set; }
        public List<Purchase> History { get; set; }
        public decimal Balance { get; set; }
    }
}
