namespace BudgetApi.Incomes.Models;

public class IncomeSources : IncomeSourceLine
{
    public decimal? EstimatedIncome { get; set; }
    public string PayFrequency { get; set; }
}
