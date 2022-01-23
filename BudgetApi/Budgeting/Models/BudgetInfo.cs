using System;

namespace BudgetApi.Budgeting.Models
{
    public class BudgetInfo
    {
        public int BudgetLineId { get; set; }
        public BudgetTypes BudgetType { get; set; }
        public DateTime BudgetDate { get; set; }
        public string BudgetMonthYear { get; set; }
        public decimal Amount { get; set; }
    }
}
