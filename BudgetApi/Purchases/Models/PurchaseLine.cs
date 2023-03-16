using BudgetApi.Models;
using System;

namespace BudgetApi.Purchases.Models
{
    public class PurchaseLine
    {
        public int Id { get; set; }
        public BudgetType PurchaseType { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public bool IsReimbursement { get; set; }
        public string PaymentType { get; set; }
        public string Description { get; set; }
        public int GiftCardId { get; set; }
    }
}
