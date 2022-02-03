using System;

namespace BudgetApi.Copy.Services
{
    public interface IBudgetCopyService
    {
        void CopyBudgetFromPreviousMonth(DateTime monthYear);
    }
}
