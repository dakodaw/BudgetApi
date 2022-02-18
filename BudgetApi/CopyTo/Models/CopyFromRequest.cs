using System;

namespace BudgetApi.CopyTo.Models
{
    public class CopyFromRequest
    {
        public CopyFromEnum FromMethod { get; set; }
        public DateTime? MonthYear { get; set; }
    }
}
