using BudgetApi.CopyTo.Models;
using System;
using System.Threading.Tasks;

namespace BudgetApi.CopyTo.Services
{
    public interface IBudgetCopyToService
    {
        Task CopyFrom(int groupId, DateTime monthYear, CopyFromRequest request);
    }
}
