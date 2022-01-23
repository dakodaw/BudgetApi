using System;

namespace BudgetApi.Incomes.Models
{
    public class ShortIncomeLine
    {
        public int IncomeId { get; set; }
        public int IncomeSourceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime IncomeDate { get; set; }
    }
}
