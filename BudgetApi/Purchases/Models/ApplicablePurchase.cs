namespace BudgetApi.Purchases.Models
{
    public class ApplicablePurchase
    {
        public int Id { get; set; }
        public string PurchaseType { get; set; }
        public decimal Amount { get; set; }
    }
}
