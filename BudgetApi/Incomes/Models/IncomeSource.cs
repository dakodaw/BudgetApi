using BudgetApi.Shared;

namespace BudgetApi.Incomes.Models
{
    public class IncomeSource : IncomeSourceLine
    {
        public decimal? EstimatedIncome { get; set; }
        public string PayFrequency { get; set; }
    }
}
