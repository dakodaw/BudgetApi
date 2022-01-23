using System;

namespace BudgetApi.Budgeting.Models
{
    public class ScenarioInput
    {
        public double initialAmount { get; set; }
        public DateTime startMonth { get; set; }
        public DateTime endMonth { get; set; }
    }
}
