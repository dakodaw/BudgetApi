namespace Budget.Models;

public class IncomeSource
{    
    public int Id { get; set; }
    public string SourceName { get; set; }
    public string JobOf { get; set; }
    public string PositionName { get; set; }
    public bool ActiveJob { get; set; }
    public decimal? EstimatedIncome { get; set; }
    public string PayFrequency { get; set; }
}
