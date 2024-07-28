using Budget.Models;

namespace BudgetApi.Incomes.Models
{
    public class IncomeLine : ShortIncomeLine
    {
        public IncomeSource IncomeSource { get; set; }
        public string Details { get; set; }
        public bool IsReimbursement { get; set; }
        public int? PurchaseId { get; set; }
        public bool IsCash { get; set; }
    }
}
