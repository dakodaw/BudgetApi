using Budget.Models;

namespace BudgetApi.Incomes.Models;

public class IncomeSources : IncomeSource
{
    public decimal? EstimatedIncome { get; set; }
    public string PayFrequency { get; set; }
}
