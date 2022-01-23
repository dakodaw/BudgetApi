namespace BudgetApi.GiftCards.Models
{
    public class GiftCardSelectLine
    {
        public int Id { get; set; }
        public string Place { get; set; }
        public decimal RemainingAmount { get; set; }
        public string Last4ofCardNumber { get; set; }
    }
}
