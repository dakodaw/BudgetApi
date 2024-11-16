using BudgetApi.CopyTo.Models;
using System;

namespace BudgetApi.CopyTo.Services
{
    public interface IBudgetCopyToService
    {
        void CopyFrom(int groupId, DateTime monthYear, CopyFromRequest request);
    }
}
