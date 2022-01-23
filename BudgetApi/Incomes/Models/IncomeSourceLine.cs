namespace BudgetApi.Incomes.Models
{
    public class IncomeSourceLine
    {
        public int IncomeSourceId { get; set; }
        public string IncomeSource { get; set; }
        public string JobOf { get; set; }
        public string Position { get; set; }
        public bool IsCurrentJob { get; set; }
    }
}
