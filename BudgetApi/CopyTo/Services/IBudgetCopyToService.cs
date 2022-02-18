using BudgetApi.CopyTo.Models;
using System;

namespace BudgetApi.CopyTo.Services
{
    public interface IBudgetCopyToService
    {
        void CopyFrom(DateTime monthYear, CopyFromRequest request);
    }
}
