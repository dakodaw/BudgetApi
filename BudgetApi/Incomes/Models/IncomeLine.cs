namespace BudgetApi.Incomes.Models
{
    public class IncomeLine : ShortIncomeLine
    {
        public IncomeSourceLine IncomeSource { get; set; }
        public string Details { get; set; }
        public bool IsReimbursement { get; set; }
        public int PurchaseId { get; set; }
        public bool IsCash { get; set; }
    }
}
